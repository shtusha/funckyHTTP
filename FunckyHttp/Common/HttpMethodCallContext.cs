using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Collections;
using System.Xml.XPath;
using System.IO;

namespace FunckyHttp.Common
{
    public class HttpMethodCallContext
    {
        private Lazy<HttpWebResponse> _ResponseLazy;
        private Lazy<byte[]> _ContentLazy;
        private Lazy<XPathDocument> _XMLContentLazy;
        private Lazy<HttpStatusCode> _StatusCodeLazy;
        private Lazy<WebHeaderCollection> _HeadersLazy;
        public Request RequestContext { get; private set; }



        public HttpMethodCallContext(string url, IDictionary<string, string> headers)
            : this(url)
        {
            foreach (var item in headers)
            {
                RequestContext.Headers.Add(item.Key, item.Value);    
            }
        }


        public HttpMethodCallContext(string url)
            : this()
        {
            RequestContext = new Request(url);
        }
            
        private HttpMethodCallContext()
        {

            _ResponseLazy = new Lazy<HttpWebResponse>(() => InvokeHttpRequest(RequestContext.BuildWebRequest()), true);

            _StatusCodeLazy = new Lazy<HttpStatusCode>(() => Response.StatusCode, true);

            _HeadersLazy = new Lazy<WebHeaderCollection>(() => Response.Headers, true);

            _XMLContentLazy = new Lazy<XPathDocument>(()=>Utils.BytesToXML(Content, Response.ContentType), true);

            _ContentLazy = new Lazy<byte[]>(() =>
            {
                using (var ms = new MemoryStream())
                {
                    var responseStream = Response.GetResponseStream();
                    if (responseStream != null)
                    {
                        responseStream.CopyTo(ms);
                    }
                    return ms.ToArray();
                }
            }, true);
        }

        public class Request
        {
            public Request(string url)
            {
                Url = url;
                Headers = new Dictionary<string, string>();
            }

            public string Url { get; private set; }

            public IDictionary<string, string> Headers { get; set; }
            public byte[] Content { get; set; }

            public string Verb { get; set; }

            public HttpWebRequest BuildWebRequest()
            {
                var request = HttpWebRequest.CreateHttp(Url);
                request.Method = Verb;
                if (Content != null)
                {
                    using (var contentWriter = new BinaryWriter(request.GetRequestStream()))
                    {
                        contentWriter.Write(Content);
                    }
                }
                SetHeaders(request);
                return request;
            }

            private void SetHeaders(HttpWebRequest request)
            {
                foreach (var header in Headers)
                {
                    switch (header.Key.ToLower())
                    {
                        case "accept":
                            request.Accept = header.Value;
                            break;
                        case "content-length":
                            int i;
                            if (!int.TryParse(header.Value, out i)) throw new FormatException("Content-Length header must be a valid integer");
                            request.ContentLength = i;
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
        

        private HttpWebResponse Response { get { return _ResponseLazy.Value; } }
        public HttpStatusCode StatusCode { get { return _StatusCodeLazy.Value; } }
        public byte[] Content { get { return _ContentLazy.Value; } }
        public WebHeaderCollection Headers { get { return _HeadersLazy.Value; } }
        public XPathDocument XMLContent { get { return _XMLContentLazy.Value; } }

        private HttpWebResponse InvokeHttpRequest(HttpWebRequest request)
        {
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                return response;
            }
            catch (WebException ex)
            {
                if (ex.Response as HttpWebResponse == null) { throw; }
                return ex.Response as HttpWebResponse;
            }
        }
    }
}
