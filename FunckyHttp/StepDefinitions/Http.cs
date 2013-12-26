using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

using FunckyHttp.Common;
using System.Xml.XPath;

namespace FunckyHttp.StepDefinitions
{
    [Binding]
    public class Http : Steps
    {
        //private Lazy<HttpWebResponse> _RequestInvoker;

        [Given(@"base url is (.*)")]
        public void GivenBaseUrlIs(Wrapped<string> url)
        {
            ScenarioContextStore.BaseUrl = url;
        }

        [Given(@"url is (.*)")]
        public void GivenUrlIs(Wrapped<string> url)
        {

            var _url = url.Value;
            if (_url.StartsWith("/") && ScenarioContextStore.BaseUrl.EndsWith("/"))
            {
                _url = _url.Remove(0, 1);
            }
            else if (!(_url.StartsWith("/") || ScenarioContextStore.BaseUrl.EndsWith("/")))
            {
                _url = _url.Insert(0, "/");
            }

            //TODO handle absolute vs relative urls.
            ScenarioContextStore.HttpCallContext = new HttpMethodCallContext(ScenarioContextStore.BaseUrl + _url, ScenarioContextStore.RequestHeaders);
        }

        [Then(@"response header (.*) should exist")]
        public void ThenResponseHeaderShouldExist(string headerName)
        {
            Assert.IsTrue(ScenarioContextStore.HttpCallContext.Headers.AllKeys.Contains(headerName), 
                "{0} header was expected, but was not returned.", headerName);
        }

        [Then(@"response header (.*) should be (.*)")]
        public void ThenResponseHeaderShouldBe(string headerName, Wrapped<string> headerValue)
        {
            ThenResponseHeaderShouldExist(headerName);
            Assert.AreEqual(headerValue.Value, ScenarioContextStore.HttpCallContext.Headers[headerName]);
        }

        [Then(@"response header (.*) should contain (.*)")]
        public void ThenResponseHeaderShouldContain(string headerName, Wrapped<string> headerValue)
        {
            ThenResponseHeaderShouldExist(headerName);
            Assert.IsTrue(ScenarioContextStore.HttpCallContext.Headers[headerName].Contains(headerValue),
                string.Format("{0} header: '{1}' does not contain '{2}'", 
                    headerName, ScenarioContextStore.HttpCallContext.Headers[headerName], headerValue));
        }

        [Then(@"response Status Code should be (.*)")]
        public void ThenResponseStatusCodeShouldBe(HttpStatusCode statusCode)
        {
            Assert.AreEqual(statusCode, ScenarioContextStore.HttpCallContext.StatusCode);
        }

        [When(@"*submit a (.*) request")]
        public void WhenSubmitARequest(string requestMethod)
        {
            ScenarioContextStore.HttpCallContext.RequestContext.Verb = requestMethod.ToUpper();
        }


        [Given(@"request headers are")]
        public void GivenRequestHeadersAre(Table table)
        {
            ScenarioContextStore.RequestHeaders.Clear();
            foreach (var row in table.Rows)
            {
                ScenarioContextStore.RequestHeaders[row["name"]] = row["value"];
            }

            if(ScenarioContextStore.HttpCallContext != null)
            {
                ScenarioContextStore.HttpCallContext.RequestContext.Headers.Clear();
                foreach (var row in table.Rows)
                {
                    ScenarioContextStore.HttpCallContext.RequestContext.Headers[row["name"]] = row["value"];
                }

            }

        }

        [When(@"*add headers")]
        public void WhenAddHeaders(Table table)
        {
            foreach (var row in table.Rows)
            {
                ScenarioContextStore.HttpCallContext.RequestContext.Headers[row["name"]] = row["value"];
            }
        }


        [Given(@"request content is (.*)")]
        public void GivenRequestContentIs(Wrapped<string> content)
        {
            GivenRequestContentIsMultiline(content);
        }


        //[Given(@"request content is (.*)")]
        public void GivenRequestContentIs(byte[] content)
        {
            ScenarioContextStore.HttpCallContext.RequestContext.Content = content;
        }

        [Given(@"request content is")]
        public void GivenRequestContentIsMultiline(string content)
        {
            GivenRequestContentIs(System.Text.Encoding.Default.GetBytes(content));
        }

    }
}
