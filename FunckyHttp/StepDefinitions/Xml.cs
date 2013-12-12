using System;
using System.Collections.Generic;
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


        [When(@"the following query is run against response: '(.*)'")]
        public void WhenTheFollowingQueryIsRunAgainstResponse(string qry)
        {
            ExecuteXpathQuery(ScenarioContextStore.ResponseContentXML.Value, qry);
        }

        [When(@"the following query is run against response:")] // multiline
        public void WhenTheFollowingQueryIsRunAgainstResponseMultiline(string qry)
        {
            WhenTheFollowingQueryIsRunAgainstResponse(qry);
        }


        [When(@"the following query is run against request: '(.*)'")]
        public void WhenTheFollowingQueryIsRunAgainstRequest(string qry)
        {
            ExecuteXpathQuery(ScenarioContextStore.RequestContentXML.Value, qry);
        }

        [When(@"the following query is run against request:")] // multiline
        public void WhenTheFollowingQueryIsRunAgainstRequestMultiline(string qry)
        {
            WhenTheFollowingQueryIsRunAgainstRequest(qry);
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


        private void ExecuteXpathQuery(XPathDocument target, string qry)
        {
            ScenarioContextStore.Query = XPathExpression.Compile(qry);
            if (ScenarioContextStore.NamespaceManager != null)
            {
                ScenarioContextStore.Query.SetContext(ScenarioContextStore.NamespaceManager);
            }
            ScenarioContextStore.QueryDescription = null;
            ScenarioContextStore.QueryResult = target.CreateNavigator().Evaluate(ScenarioContextStore.Query);
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
}
