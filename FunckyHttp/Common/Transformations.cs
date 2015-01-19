using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Security.Cryptography;
using System.Xml.XPath;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;
using FunckyHttp.StepDefinitions;
using System.Xml.Xsl;
using System.Xml;

namespace FunckyHttp.Common
{
    [Binding]
    public class Transformations
    {
        [StepArgumentTransformation(@"'(.*)'")]
        public Wrapped<string> StringFromLiteral(string value)
        {
            return value;
        }

        [StepArgumentTransformation(@"CONFIG\((.*)\)")]
        public Wrapped<string> StringFromConfig(string key)
        {
            return new Wrapped<string>(ConfigurationManager.AppSettings[key]);
        }



        [StepArgumentTransformation(@"FILE\((.*)\)")]
        public Wrapped<string> StringFromFile(string path)
        {
            var filePath = Path.GetFullPath(Path.Combine(ConfigurationManager.AppSettings["contentPath"], path));

            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            throw new ArgumentException(string.Format("file not found: {0}", filePath ?? "<null>"));
        }

        [StepArgumentTransformation(@"response header (.*)")]
        public Wrapped<string> StringFromHeader(string headerName)
        {
            return ScenarioContextStore.HttpCallContext.Response.Headers[headerName];
        }

        [StepArgumentTransformation(@"FILE\((.*)\)")]
        public byte[] BytesFromFile(string path)
        {
            var filePath = Path.GetFullPath(Path.Combine(ConfigurationManager.AppSettings["contentPath"], path));

            if (File.Exists(filePath))
            {
                return File.ReadAllBytes(filePath);
            }
            throw new ArgumentException(string.Format("file not found: {0}", filePath ?? "<null>"));
        }

        [StepArgumentTransformation("'(.*)'")]
        public byte[] BytesFromstringLiteral(string value)
        {
            return value == null ? null : Encoding.Default.GetBytes(value);
        }

        [StepArgumentTransformation(@"query result")]
        public Wrapped<string> StringFromQueryResult()
        {
            if (ScenarioContextStore.QueryResult == null) { return null; }
            return ScenarioContextStore.QueryResult.ToString();
        }

        [StepArgumentTransformation(@"query result")]
        public byte[] BytesFromQueryResults()
        {
            var stringResult = StringFromQueryResult();
            return stringResult == null ? null :
                Encoding.Default.GetBytes(stringResult);
        }


        [StepArgumentTransformation(@"response header (.*)")]
        public IRegexTarget RegexTargetFromHeader(string headerName)
        {
            return new RegexTargetMapper(()=> ScenarioContextStore.HttpCallContext.Response.Headers[headerName]);
        }

        [StepArgumentTransformation(@"query result")]
        public IRegexTarget RegexTargetFromQueryResult()
        {
            return new RegexTargetMapper(() => ScenarioContextStore.QueryResult.ToString());
        }

        //TODO: switch to uri instead of file.
        [StepArgumentTransformation(@"FILE\((.*)\)")]
        public XslCompiledTransform XsltFromFile(string path)
        {
            var uri = new Uri(GetFullPath(path)).AbsoluteUri;
            Debug.WriteLine("xml.transform.source: {0}", (object)uri);
            var settings = new XmlReaderSettings { ConformanceLevel = ConformanceLevel.Fragment };
            //TODO: Cache
            LogXSLTContent(uri, settings);
            using (var xsltReader = XmlTextReader.Create(uri, settings))
            {
                             
                var transform = new XslCompiledTransform();
                transform.Load(xsltReader);
                return transform;
            }
        }

        private static void LogXSLTContent(string uri, XmlReaderSettings settings)
        {
            using (var xsltReader = XmlTextReader.Create(uri, settings))
            {
                Debug.WriteLine("xml.transform: {0}", (object)new XPathDocument(xsltReader).CreateNavigator().InnerXml);
            }
        }

        class RequestContentTransform : IXslTransformable
        {
            public byte[] TransformationSource { get { return ScenarioContextStore.HttpCallContext.Request.Content; } }
            public byte[] TransformedResult { set { ScenarioContextStore.HttpCallContext.Request.Content = value; } }

        }

        [StepArgumentTransformation(@"query result")]
        public IXsltSource XsltSourceFromQueryResult()
        {
            return new XsltSource(BytesFromQueryResults());
        }

        [StepArgumentTransformation(@"response")]
        public IXsltSource XsltSourceFromResponseContent()
        {
            return new XsltSource(ScenarioContextStore.HttpCallContext.Response.Content);
        }

        [StepArgumentTransformation(@"request content")]
        public IXslTransformable XsltTransformableFromRequestContent()
        {
            Assert.IsNotNull(ScenarioContextStore.HttpCallContext.Request.Content, "Request Content is null");
            return new RequestContentTransform();
        }

        [StepArgumentTransformation(@"request content")]
        public IXsltResult XsltResultToRequestContent()
        {
            return new RequestContentTransform();
        }

        [StepArgumentTransformation(@"request content")]
        public IXsltSource XsltSourceFromRequestContent()
        {
            return XsltTransformableFromRequestContent();
        }

        private static string GetFullPath(string path)
        {

            return
                Path.GetFullPath(Path.IsPathRooted(path)
                                     ? path
                                     : Path.Combine(ConfigurationManager.AppSettings["contentPath"], path));
        }
    }
}
