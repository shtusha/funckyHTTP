using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Xml.XPath;
using System.IO;

namespace FunckyHttp
{
    public class HttpMethodCallContext
    {

        private readonly Lazy<ResponseContext> _responseLazy;

        public ResponseContext LastResponse { get; set; }

        public RequestContext Request { get; private set; }



        public ResponseContext Response
        {
            get { return Request.Verb != null ? _responseLazy.Value : LastResponse; }
        }
 
        


        public HttpMethodCallContext(string url, IDictionary<string, string> headers)
            : this(url)
        {
            foreach (var item in headers)
            {
                Request.Headers.Add(item.Key, item.Value);    
            }
        }


        public HttpMethodCallContext(string url)
        {
            Request = new RequestContext(url);
            _responseLazy = new Lazy<ResponseContext>(() => new ResponseContext(Request.BuildWebRequest()), true);
        }
            
        public class ResponseContext
        {
            private Lazy<HttpWebResponse> _ResponseLazy;
            private Lazy<byte[]> _ContentLazy;
            private Lazy<XPathDocument> _xmlContentLazy;
            private Lazy<HttpStatusCode> _StatusCodeLazy;
            private Lazy<WebHeaderCollection> _headersLazy;

            private HttpWebRequest _WebRequest;

            public ResponseContext(HttpWebRequest request)
            {
                _WebRequest = request;

                _ResponseLazy = new Lazy<HttpWebResponse>(() => InvokeHttpRequest(_WebRequest), true);

                _StatusCodeLazy = new Lazy<HttpStatusCode>(() => HttpResponse.StatusCode, true);

                _headersLazy = new Lazy<WebHeaderCollection>(() => HttpResponse.Headers, true);

                _xmlContentLazy = new Lazy<XPathDocument>(() => Content.BytesToXML(HttpResponse.ContentType), true);

                _ContentLazy = new Lazy<byte[]>(() =>
                {
                    using (var ms = new MemoryStream())
                    {
                        var responseStream = HttpResponse.GetResponseStream();
                        if (responseStream != null)
                        {
                            responseStream.CopyTo(ms);
                        }
                        return ms.ToArray();
                    }
                }, true);
            }

            private HttpWebResponse HttpResponse { get { return _ResponseLazy.Value; } }

            public HttpStatusCode StatusCode
            {
                get
                {
                    if (!_StatusCodeLazy.IsValueCreated)
                    {
                        Debug.WriteLine("http.response.statuscode: {0}", _StatusCodeLazy.Value);
                    }
                    return _StatusCodeLazy.Value;
                }
            }

            public byte[] Content
            {
                get
                {
                    if (!_ContentLazy.IsValueCreated)
                    {
                        using (var reader = new StreamReader(new MemoryStream(_ContentLazy.Value)))
                        {
                            Debug.WriteLine("http.response.body:\n{0}", (object)reader.ReadToEnd());                            
                        }
                    }
                    return _ContentLazy.Value;
                }
            }



            public WebHeaderCollection Headers
            {
                get
                {
                    if (!_headersLazy.IsValueCreated)
                    {
                        Debug.WriteLine("http.response.headers:");
                        foreach (var key in _headersLazy.Value.AllKeys)
                        {
                            Debug.WriteLine("{0} : {1}", key, _headersLazy.Value[key]);
                        }
                        
                    }
                    return _headersLazy.Value;
                }
            }

            public XPathDocument XmlContent { get { return _xmlContentLazy.Value; } }

            private HttpWebResponse InvokeHttpRequest(HttpWebRequest request)
            {
                try
                {
                    var response = (HttpWebResponse)request.GetResponse();
                    return response;
                }
                catch (WebException ex)
                {
                    if (ex.Response as HttpWebResponse == null){ throw; }
                    return ex.Response as HttpWebResponse;
                }
            }
        }

        public class RequestContext
        {
            public RequestContext(string url)
            {
                Url = url;
                Headers = new Dictionary<string, string>();
            }

            private string _url;
            public string Url
            {
                get { return _url; }
                private set
                {
                    _url = value;
                    Debug.WriteLine("http.request.url:\n{0}", (object)_url);
                }
            }

            public IDictionary<string, string> Headers { get; private set; }


            private byte[] _content;
            public byte[] Content {
                get { return _content; }
                set
                {
                    _content = value; 
                    Debug.WriteLine("http.request.body:");
                    Debug.WriteLine(_content.BytesToString());
                }}

            private string _verb;

            public string Verb
            {
                get { return _verb; }
                set
                {
                    _verb = value;
                    Debug.WriteLine("http.request.verb: {0}", (object)_verb);
                }
            }

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
    }
}
