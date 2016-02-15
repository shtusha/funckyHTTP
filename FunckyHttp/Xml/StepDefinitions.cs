using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using FluentAssertions;
using TechTalk.SpecFlow;


using FunckyHttp.Common;

namespace FunckyHttp.Xml
{
    [Binding]
    public class StepDefinitions : Steps
    {

        [Given(@"xml namespace aliases are")]
        public void GivenTheFollowingXmlNamespaceAliases(Table table)
        {
            var nsMgr = new XmlNamespaceManager(new NameTable());
            foreach (var row in table.Rows)
            {
                nsMgr.AddNamespace(row["alias"], row["namespace"]);
            }
            ScenarioContextStore.NamespaceManager = nsMgr;
        }


        [Given(@"xslt is (.*)")]
        public void GivenXslTransformationIs(XslCompiledTransform xslt)
        {
            ScenarioContextStore.XSLTransform = xslt;
        }

        [Given(@"xslt is")]
        public void GivenXslTransformationIsMultiline(string xslt)
        {
            var settings = new XmlReaderSettings { ConformanceLevel = ConformanceLevel.Fragment };
            using (var xsltReader = XmlTextReader.Create(new MemoryStream(Encoding.Default.GetBytes(xslt)), settings))
            {
                var transform = new XslCompiledTransform();
                transform.Load(xsltReader);
                GivenXslTransformationIs(transform);
            }
        }


        [When(@"(.*) is transformed into (.*)")]
        public void WhenSourceIsTransformedInto(IXsltSource source, IXsltResult destination)
        {

            ScenarioContextStore.XSLTransform.Should().NotBeNull("XSL Transformation is needed to transform");
            var settings = new XmlReaderSettings { ConformanceLevel = ConformanceLevel.Fragment };
            using (var xmlReader = XmlTextReader.Create(new MemoryStream(source.TransformationSource), settings))
            {
                using (var ms = new MemoryStream())
                {
                    ScenarioContextStore.XSLTransform.Transform(xmlReader, XmlWriter.Create(ms, ScenarioContextStore.XSLTransform.OutputSettings));
                    ms.Position = 0;
                    destination.TransformedResult = ms.ToArray();
                }
            }
        }

        [When(@"(.*) is transformed")]
        public void WhenTransformed(IXslTransformable transformable)
        {
            WhenSourceIsTransformedInto(transformable, transformable);
        }


        [When(@"the following query is run against response: (.*)")]
        public void WhenTheFollowingQueryIsRunAgainstResponse(Wrapped<string> qry)
        {
            ExecuteXpathQuery(ScenarioContextStore.HttpCallContext.Response.XmlContent, qry, "Response Content");
        }

        [When(@"the following query is run against response:")] // multiline
        public void WhenTheFollowingQueryIsRunAgainstResponseMultiline(string qry)
        {
            WhenTheFollowingQueryIsRunAgainstResponse(qry);
        }


        [When(@"the following query is run against request: (.*)")]
        public void WhenTheFollowingQueryIsRunAgainstRequest(Wrapped<string> qry)
        {
            ExecuteXpathQuery(ScenarioContextStore.HttpCallContext.Response.XmlContent, qry, "Request Content");
        }

        [When(@"the following query is run against request:")] // multiline
        public void WhenTheFollowingQueryIsRunAgainstRequestMultiline(string qry)
        {
            WhenTheFollowingQueryIsRunAgainstRequest(qry);
            Debug.WriteLine($"xpath.query.result {ScenarioContextStore.QueryResult}");
        }

        [When(@"query description is '(.*)'")]
        public void QueryDescriptionIs(string alias)
        {
            ScenarioContextStore.QueryDescription = alias;
        }

        [Then(@"the result should be (true|false)")]
        public void ThenTheResultShouldBe(bool expected)
        {
            ScenarioContextStore.QueryResult.ToString()
                            .Should()
                            .BeParsableToBoolean($"that's the expected type of result for { GetQueryDescription() }");
            var actual = bool.Parse(ScenarioContextStore.QueryResult.ToString());
            actual
                .Should()
                .Be(expected, $"that's the expected result for {GetQueryDescription()}");
        }

        [Then(@"the result should be ([-]?[0-9]*\.?[0-9]+)")]
        public void ThenTheResultShouldBe(decimal expected)
        {
            ScenarioContextStore.QueryResult.ToString()
                .Should()
                .BeNumeric($"that's the expected type of result for { GetQueryDescription() }");
            var actual = decimal.Parse(ScenarioContextStore.QueryResult.ToString());
            actual
                   .Should()
                   .Be(expected, $"that's the expected result for {GetQueryDescription()}");
        }

        [Then(@"the result should be '(.*)'")]
        public void ThenTheResultShouldBe(string expected)
        {
            ScenarioContextStore.QueryResult
                .Should()
                .BeOfType<string>($"that's the expected type of result for { GetQueryDescription() }");

            ScenarioContextStore.QueryResult.ToString()
                .Should()
                .Be(expected, $"that's the expected result for { GetQueryDescription() }");

        }


        [Then(@"the following assertions against response should pass:")]
        public void ThenTheFollowingResponseAssertionsShouldPass(Table table)
        {
            RunAssertions(table, "response");
        }

        [Then(@"the following assertions against request should pass:")]
        public void ThenTheFollowingRequestAssertionsShouldPass(Table table)
        {
            RunAssertions(table, "request");
        }


        private string GetQueryDescription()
        {
            return new StringBuilder().AppendFormat("\nQuery: {1}{0}", ScenarioContextStore.Query.Expression,
                                                    ScenarioContextStore.QueryDescription == null
                                                        ? string.Empty
                                                        : string.Format("{0}\n", ScenarioContextStore.QueryDescription))
                                      .ToString();
        }


        private void ExecuteXpathQuery(XPathDocument target, string qry, string queryInputDescription)
        {
            Debug.WriteLine("xpath.query: {0}", (object)qry);
            Debug.WriteLine("xpath.query.input: {0}", (object)queryInputDescription);
                
            ScenarioContextStore.Query = XPathExpression.Compile(qry);
            if (ScenarioContextStore.NamespaceManager != null)
            {
                ScenarioContextStore.Query.SetContext(ScenarioContextStore.NamespaceManager);
            }
            ScenarioContextStore.QueryDescription = null;
            
            var result = target.CreateNavigator().Evaluate(ScenarioContextStore.Query);

            var nodeiterator = result as XPathNodeIterator;
            if (nodeiterator != null && nodeiterator.Count == 1)
            {
                nodeiterator.MoveNext();
                ScenarioContextStore.QueryResult = nodeiterator.Current.Value;
            }
            else
            {
                ScenarioContextStore.QueryResult = result;
            }

            Debug.WriteLine("xpath.query.result: {0}", ScenarioContextStore.QueryResult);
        }


        private void RunAssertions(Table table, string target)
        {

            foreach (var row in table.Rows)
            {
                if (row["expected"].Equals("N/A", StringComparison.CurrentCultureIgnoreCase)) { continue; }

                When(string.Format("the following query is run against {0}: {1}", target, row["query"]));
                
                if (table.ContainsColumn("name"))
                {
                    When(string.Format("query description is '{0}'", row["name"]));
                }

                Then(string.Format("the result should be {0}", row["expected"]));
            }

        }
    }

    public interface IXsltSource
    {
        byte[] TransformationSource { get; }
    }

    public class XsltSource : IXsltSource
    {
        public XsltSource(byte[] source)
        {
            TransformationSource = source;
        }

        public byte[] TransformationSource { get; private set; }
    }

    public interface IXsltResult
    {
        byte[] TransformedResult { set; }
    }


    public interface IXslTransformable: IXsltSource, IXsltResult { }

}
