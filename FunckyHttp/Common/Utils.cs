using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml;
using System.IO;
using System.Xml.Xsl;
using TechTalk.SpecFlow;
using Newtonsoft.Json;
using FluentAssertions;
using FluentAssertions.Equivalency;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FunckyHttp.Common
{
    public static class Utils
    {
        private static readonly XslCompiledTransform NamespaceEliminatingTransform;
        
        static Utils()
        {
            var assembly = Assembly.GetAssembly(typeof(Utils));
            var stream = assembly.GetManifestResourceStream("FunckyHttp.resources.DropXmlNamespaces.xslt");
            
            
            using (var reader = new XmlTextReader(stream))
            {
                NamespaceEliminatingTransform = new XslCompiledTransform();
                NamespaceEliminatingTransform.Load(reader);
            }
        }

        public static string BytesToString(this byte[] bytes)
        {
            if (bytes == null) { return null; }
            using (var reader = new StreamReader(new MemoryStream(bytes)))
            {
                return reader.ReadToEnd();
            }
        }

        public static XPathDocument BytesToXML(this byte[] bytes, string contentType)
        {
            if (contentType.ToLower().Contains("json"))
            {
                using (var reader = new StreamReader(new MemoryStream(bytes)))
                {
                    var content = reader.ReadToEnd();
                    var rootElementName = ScenarioContextStore.JsonToXMLRootElementName ?? "root";
                    var doc = JsonConvert.DeserializeXmlNode(string.Format("{{{0}: {1}}}",rootElementName, content));
                    var xpathDoc = new XPathDocument(new XmlNodeReader(doc));
                    
                    Debug.WriteLine("xml.from.json:");
                    Debug.WriteLine(xpathDoc.CreateNavigator().InnerXml);
                    
                    return xpathDoc;


                }
            }
            using (var reader = XmlTextReader.Create(new MemoryStream(bytes), new XmlReaderSettings { ConformanceLevel = ConformanceLevel.Fragment }))
            {
                //option strict
                if (!ScenarioContextStore.DropXmlNamespaces)
                {
                    return  new XPathDocument(reader);
                }

                using (var ms = new MemoryStream())
                using (var writer = XmlWriter.Create(ms))
                {
                    NamespaceEliminatingTransform.Transform(reader, writer);
                    ms.Seek(0, 0);
                    using (var transformedReader = XmlReader.Create(ms))
                    {
                        var xpathDoc = new XPathDocument(transformedReader);

                        Debug.WriteLine("xml.namespace.dropped:");
                        Debug.WriteLine(xpathDoc.CreateNavigator().InnerXml);
                        
                        return xpathDoc;
                    }
                }
            }
        }

        public static void BeNumeric(this StringAssertions assertions, string reason, params object[] reasonArgs)
        {
            decimal value;
            Execute.Assertion
                .ForCondition(decimal.TryParse(assertions.Subject, out value))
                .BecauseOf(reason, reasonArgs)
                .FailWith("{0} should be a numeric string{reason}, but it's not.", assertions.Subject);

        }

        public static void BeParsableToBoolean(this StringAssertions assertions, string reason, params object[] reasonArgs)
        {
            bool value;
            Execute.Assertion
                .ForCondition(bool.TryParse(assertions.Subject, out value))
                .BecauseOf(reason, reasonArgs)
                .FailWith("{0} should be parsable to boolean{reason}, but it's not.", assertions.Subject);

        }

    }
}
