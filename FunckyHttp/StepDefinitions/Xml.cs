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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;


using FunckyHttp.Common;

namespace FunckyHttp.StepDefinitions
{
    [Binding]
    public class Xml : Steps
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
            }
        }


        [When(@"(.*) is transformed into (.*)")]
        public void WhenSourceIsTransformedInto(IXsltSource source, IXsltResult destination)
        {
            Assert.IsNotNull(ScenarioContextStore.XSLTransform, "XSL Transformation is null");
            var settings = new XmlReaderSettings { ConformanceLevel = ConformanceLevel.Fragment };
            //Debug.WriteLine("XML content prior to transform:{0}{1}", Environment.NewLine, source.TransformationSource.BytesToXML("xml").CreateNavigator().InnerXml);

            //using (var xmlReader = XmlTextReader.Create(new MemoryStream(ScenarioContextStore.HttpCallContext.RequestContext.Content), settings))
            using (var xmlReader = XmlTextReader.Create(new MemoryStream(source.TransformationSource), settings))
            {
                using (var ms = new MemoryStream())
                {
                    ScenarioContextStore.XSLTransform.Transform(xmlReader, XmlWriter.Create(ms, ScenarioContextStore.XSLTransform.OutputSettings));
                    ms.Position = 0;
                    destination.TransformedResult = ms.ToArray();
                    //ScenarioContextStore.HttpCallContext.RequestContext.Content = ms.ToArray();
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
            Debug.WriteLine(string.Format("xpath.query.result {0}", ScenarioContextStore.QueryResult));
        }

        [When(@"query description is '(.*)'")]
        public void QueryDescriptionIs(string alias)
        {
            ScenarioContextStore.QueryDescription = alias;
        }

        [Then(@"the result should be (true|false)")]
        public void ThenTheResultShouldBe(bool expected)
        {
            if (ScenarioContextStore.QueryResult is bool)
            {
                bool actual = (bool)ScenarioContextStore.QueryResult;
                Assert.AreEqual(expected, actual, GetQueryDescription());
            }

            else
                Assert.Fail("Query result type and expected value type missmatch.");
        }

        [Then(@"the result should be ([-]?[0-9]*\.?[0-9]+)")]
        public void ThenTheResultShouldBe(decimal expected)
        {
            decimal actual;
            if (decimal.TryParse(ScenarioContextStore.QueryResult.ToString(), out actual))
                Assert.AreEqual(expected, actual, GetQueryDescription());
            else
                Assert.Fail("Query result type and expected value type missmatch.");
        }

        [Then(@"the result should be '(.*)'")]
        public void ThenTheResultShouldBe(string expected)
        {
            var actual = ScenarioContextStore.QueryResult as string;
            if (actual != null)
                Assert.AreEqual(expected, actual, GetQueryDescription());
            else
                Assert.Fail("Query result type and expected value type missmatch.");
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
            Debug.WriteLine("xpath.query.input: HTTPRequest");
                
            ScenarioContextStore.Query = XPathExpression.Compile(qry);
            if (ScenarioContextStore.NamespaceManager != null)
            {
                ScenarioContextStore.Query.SetContext(ScenarioContextStore.NamespaceManager);
            }
            ScenarioContextStore.QueryDescription = null;
            ScenarioContextStore.QueryResult = target.CreateNavigator().Evaluate(ScenarioContextStore.Query);

            Debug.WriteLine("xpath.query.result: {0}", ScenarioContextStore.QueryResult);
        }


        private void RunAssertions(Table table, string target)
        {
            foreach (var row in table.Rows)
            {
                if (row["expected"].Equals("N/A", StringComparison.CurrentCultureIgnoreCase)) { continue; }

                When(string.Format("the following query is run against {0}: {1}", target, row["query"]));
                When(string.Format("query description is '{0}'", row["name"]));
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
