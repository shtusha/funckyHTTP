using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Configuration;
using System.Xml.XPath;
using TechTalk.SpecFlow;
using System.Xml.Xsl;
using System.Xml;
using FluentAssertions;

namespace FunckyHttp.Xml
{
    [Binding]
    public class Transformations
    {
        //TODO: switch to uri instead of file.
        [StepArgumentTransformation(Constants.Patterns.ValueSources.File)]
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

        [StepArgumentTransformation(Constants.Patterns.ValueSources.QueryResult)]
        public IXsltSource XsltSourceFromQueryResult()
        {
            return new XsltSource(Common.Transformations.BytesFromQueryResults());
        }

        [StepArgumentTransformation(@"response")]
        public IXsltSource XsltSourceFromResponseContent()
        {
            return new XsltSource(ScenarioContextStore.HttpCallContext.Response.Content);
        }

        [StepArgumentTransformation(Constants.Patterns.ValueSources.RequestBody)]
        public IXslTransformable XsltTransformableFromRequestContent()
        {
            ScenarioContextStore.HttpCallContext.Request.Content
                .Should()
                .NotBeNull("Request Content is required");
            return new RequestContentTransform();
        }

        [StepArgumentTransformation(Constants.Patterns.ValueSources.RequestBody)]
        public IXsltResult XsltResultToRequestContent()
        {
            return new RequestContentTransform();
        }

        [StepArgumentTransformation(Constants.Patterns.ValueSources.RequestBody)]
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
