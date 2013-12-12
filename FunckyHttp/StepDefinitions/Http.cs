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

        //private readonly string _RootUrl = ConfigurationManager.AppSettings["rootUrl"]; //"http://fnf-rc-ite.cloudapp.net/Fnf.RateCalculator.Web/";
        private Lazy<HttpWebResponse> _RequestInvoker;

        [Given(@"base url is (.*)")]
        public void GivenBaseUrlIs(string url)
        {
            ScenarioContextStore.BaseUrl = url;
        }

        [Given(@"url is (.*)")]
        public void GivenUrlIs(string url)
        {

            var _url = url;
            if (_url.StartsWith("/") && ScenarioContextStore.BaseUrl.EndsWith("/"))
            {
                _url = _url.Remove(0, 1);
            }
            else if (!(_url.StartsWith("/") || ScenarioContextStore.BaseUrl.EndsWith("/")))
            {
                _url = _url.Insert(0, "/");
            }


            ScenarioContextStore.RequestUrl = ScenarioContextStore.BaseUrl + _url;

            //If there are multiple requests per scenario we want to reset every time url changes
            //TODO: think about this...
            _RequestInvoker = new Lazy<HttpWebResponse>(InvokeWebRequest, true);
            ScenarioContextStore.RequestContent = null;
            Utils.ResetRequestXML();


        }

        [Then(@"response header (.*) should exist")]
        public void ThenResponseHeaderShouldExist(string headerName)
        {
            Assert.IsTrue(Response.Headers.AllKeys.Contains(headerName), "{0} header was expected, but was not returned.", headerName);
        }

        [Then(@"response header (.*) should be (.*)")]
        public void ThenResponseHeaderShouldBe(string headerName, string headerValue)
        {
            ThenResponseHeaderShouldExist(headerName);
            Assert.AreEqual(Response.Headers[headerName], headerValue);
        }

        [Then(@"response Status Code should be (.*)")]
        public void ThenResponseStatusCodeShouldBe(HttpStatusCode statusCode)
        {
            Assert.AreEqual(statusCode, Response.StatusCode);
        }

        [When(@"*submit a (.*) request")]
        public void WhenSubmitARequest(string requestMethod)
        {
            ScenarioContextStore.RequestMethod = requestMethod.ToUpper();
        }


        [Given(@"request headers are")]
        public void GivenRequestHeadersAre(Table table)
        {
            ScenarioContextStore.RequestHeaders.Clear();
            foreach (var row in table.Rows)
            {
                ScenarioContextStore.RequestHeaders[row["name"]] = row["value"];
            }

        }

        [When(@"*add headers")]
        public void WhenAddHeaders(Table table)
        {
            foreach (var row in table.Rows)
            {
                ScenarioContextStore.RequestHeaders[row["name"]] = row["value"];
            }
        }

        [Given(@"request content is (.*)")]
        public void GivenRequestContentIs(byte[] content)
        {
            ScenarioContextStore.RequestContent = content;
        }

        [Given(@"request content is")]
        public void WhenRequestContentIsMultiline(string content)
        {
            GivenRequestContentIs(System.Text.Encoding.Default.GetBytes(content));
        }

        public HttpWebResponse Response
        {
            get { return _RequestInvoker.Value; }
        }

        public HttpWebResponse InvokeWebRequest()
        {
            var request = HttpWebRequest.CreateHttp(ScenarioContextStore.RequestUrl);
            request.Method = ScenarioContextStore.RequestMethod;
            if (ScenarioContextStore.RequestContent != null)
            {
                using (var contentWriter = new BinaryWriter(request.GetRequestStream()))
                {
                    contentWriter.Write(ScenarioContextStore.RequestContent);
                }
            }

            SetHeaders(request);
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                var responseStream = response.GetResponseStream();
                using (var ms = new MemoryStream())
                {
                    if (responseStream != null)
                    {
                        responseStream.CopyTo(ms);
                    }
                    ScenarioContextStore.ResponseContent = ms.ToArray();
                    Utils.ResetResponseXML();
                }
                return response;
            }
            catch (WebException ex)
            {
                return ex.Response as HttpWebResponse;
            }
        }

        private void SetHeaders(HttpWebRequest request)
        {
            foreach (var header in ScenarioContextStore.RequestHeaders)
            {
                switch (header.Key.ToLower())
                {
                    case "accept":
                        request.Accept = header.Value;
                        break;
                    case "content-type":
                        request.ContentType = header.Value;
                        break;
                    case "expect":
                        request.Expect = header.Value;
                        break;
                    case "date":
                        DateTime dt;
                        if (!DateTime.TryParse(header.Value, out dt)) throw new FormatException("Date header must be a valid date");
                        request.Date = dt;
                        break;
                    case "host":
                        request.Host = header.Value;
                        break;
                    case "if-modified-since":
                        if (!DateTime.TryParse(header.Value, out dt)) throw new FormatException("If-Modified-Since header must be a valid date");
                        request.IfModifiedSince = dt;
                        break;
                    case "referer":
                        request.Referer = header.Value;
                        break;
                    case "transfer-encoding":
                        request.TransferEncoding = header.Value;
                        break;
                    case "user-agent":
                        request.UserAgent = header.Value;
                        break;                                                

                    default:
                        request.Headers.Add(header.Key, header.Value);
                        break;
                }
            }

        }
    }
}
