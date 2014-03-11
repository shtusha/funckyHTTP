using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
            Uri uri;
            if (Uri.TryCreate(url, UriKind.Absolute, out uri) ||
                Uri.TryCreate(new Uri(ScenarioContextStore.BaseUrl), url, out uri))
            {
                HttpMethodCallContext.ResponseContext lastResponse = null;
                if (ScenarioContextStore.HttpCallContext != null)
                {
                    lastResponse = ScenarioContextStore.HttpCallContext.Response;
                }

                ScenarioContextStore.HttpCallContext = new HttpMethodCallContext(uri.AbsoluteUri, ScenarioContextStore.RequestHeaders)
                {
                    LastResponse = lastResponse
                };
                
                //need this so that we can use properties of previous response to build next request.
            }
            else
            {
                throw new ArgumentException(string.Format("Invalid url: {0}. Base Url: {1}", url ?? "<null>", ScenarioContextStore.BaseUrl ?? "<null>" ));
            }

        }

        [Then(@"response header (.*) should exist")]
        public void ThenResponseHeaderShouldExist(string headerName)
        {
            Assert.IsTrue(ScenarioContextStore.HttpCallContext.Response.Headers.AllKeys.Contains(headerName), 
                "{0} header was expected, but was not returned.", headerName);
        }

        [Then(@"response header (.*) should be (.*)")]
        public void ThenResponseHeaderShouldBe(string headerName, Wrapped<string> expected)
        {
            ThenResponseHeaderShouldExist(headerName);
            Assert.AreEqual(expected.Value, ScenarioContextStore.HttpCallContext.Response.Headers[headerName]);
        }

        [Then(@"response Status Code should be (.*)")]
        public void ThenResponseStatusCodeShouldBe(HttpStatusCode statusCode)
        {
            Assert.AreEqual(statusCode, ScenarioContextStore.HttpCallContext.Response.StatusCode);
        }

        [When(@"*submit a (.*) request")]
        public void WhenSubmitARequest(string requestMethod)
        {
            ScenarioContextStore.HttpCallContext.Request.Verb = requestMethod.ToUpper();
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
                ScenarioContextStore.HttpCallContext.Request.Headers.Clear();
                foreach (var row in table.Rows)
                {
                    ScenarioContextStore.HttpCallContext.Request.Headers[row["name"]] = row["value"];
                }

            }

        }

        [When(@"*add headers")]
        public void WhenAddHeaders(Table table)
        {
            foreach (var row in table.Rows)
            {
                ScenarioContextStore.HttpCallContext.Request.Headers[row["name"]] = row["value"];
            }
        }

        [Given(@"request content is (.*)")]
        public void GivenRequestContentIs(byte[] content)
        {
            ScenarioContextStore.HttpCallContext.Request.Content = content;
        }

        [Given(@"request content is")]
        public void GivenRequestContentIsMultiline(string content)
        {
            GivenRequestContentIs(System.Text.Encoding.Default.GetBytes(content));
        }

    }
}
