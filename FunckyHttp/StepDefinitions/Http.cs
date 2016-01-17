using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using TechTalk.SpecFlow;
using FunckyHttp.Common;
using System.Xml.XPath;
using FluentAssertions;

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

            ScenarioContextStore.HttpCallContext.Response.Headers.AllKeys
                .Should()
                .Contain(headerName, $"response header {headerName} should exist");

            //Assert.IsTrue(ScenarioContextStore.HttpCallContext.Response.Headers.AllKeys.Contains(headerName), 
            //    "{0} header was expected, but was not returned.", headerName);
        }

        [Then(@"response header (.*) should be (.*)")]
        public void ThenResponseHeaderShouldBe(string headerName, Wrapped<string> expected)
        {
            ThenResponseHeaderShouldExist(headerName);
            ScenarioContextStore.HttpCallContext.Response.Headers[headerName]
                .Should()
                .Be(expected, $"that's what response header {headerName} is expected to be");
            //Assert.AreEqual(expected.Value, ScenarioContextStore.HttpCallContext.Response.Headers[headerName]);
        }

        

        [Then(@"response Status Code should be (.*)")]
        public void ThenResponseStatusCodeShouldBe(HttpStatusCode statusCode)
        {
            ScenarioContextStore.HttpCallContext.Response.StatusCode
                .Should()
                .Be(statusCode, $"that's what response Status Code is expected to be");
            //Assert.AreEqual(statusCode, ScenarioContextStore.HttpCallContext.Response.StatusCode);
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
                Given(string.Format("add a GLOBAL request header {0} : {1}", row["name"], row["value"]));
            }

            if(ScenarioContextStore.HttpCallContext != null)
            {
                ScenarioContextStore.HttpCallContext.Request.Headers.Clear();
                GivenAddHeaders(table);
            }
            Debug.WriteLine("http.request.headers:");
            ScenarioContextStore.RequestHeaders.ToList().ForEach(a=>Debug.WriteLine("{0} : {1}", a.Key, a.Value));

        }

        
        [When(@"*add reqauest headers")]
        public void WhenAddHeaders(Table table)
        {
            foreach (var row in table.Rows)
            {
                Given(string.Format("add a request header {0} : {1}", row["name"], row["value"]));
            }
            Debug.WriteLine("http.request.headers:");
            ScenarioContextStore.HttpCallContext.Request.Headers.ToList().ForEach(a => Debug.WriteLine("{0} : {1}", a.Key, a.Value));
        }

        [Given(@"*add request headers")]
        public void GivenAddHeaders(Table table)
        {
            foreach (var row in table.Rows)
            {
                Given(string.Format("add a request header {0} : {1}", row["name"], row["value"]));
            }
            Debug.WriteLine("http.request.headers:");
            ScenarioContextStore.HttpCallContext.Request.Headers.ToList().ForEach(a => Debug.WriteLine("{0} : {1}", a.Key, a.Value));
        }

        [When(@"*add a request header (.*) : (.*)")]
        public void WhenAddHeader(string name, Wrapped<string> value)
        {
            ScenarioContextStore.HttpCallContext.Request.Headers[name] = value;
        }

        [Given(@"*add a request header (.*) : (.*)")]
        public void GivenAddHeader(string name, Wrapped<string> value)
        {
            ScenarioContextStore.HttpCallContext.Request.Headers[name] = value;
        }

        [Given(@"*add a GLOBAL request header (.*) : (.*)")]
        public void GivenGlobalAddHeader(string name, Wrapped<string> value)
        {
            ScenarioContextStore.RequestHeaders[name] = value;
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
