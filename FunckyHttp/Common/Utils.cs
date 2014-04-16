using System;
using System.Collections.Generic;
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

        public static XPathDocument BytesToXML(this byte[] bytes, string contentType)
        {
            if (contentType.ToLower().Contains("json"))
            {
                using (var reader = new StreamReader(new MemoryStream(bytes)))
                {
                    var content = reader.ReadToEnd();
                    var rootElementName = ScenarioContextStore.JsonToXMLRootElementName ?? "root";
                    var doc = JsonConvert.DeserializeXmlNode(string.Format("{{{0}: {1}}}",rootElementName, content));
                    return new XPathDocument(new XmlNodeReader(doc));
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
                        return new XPathDocument(transformedReader);
                    }
                }
            }
        }
    }
}
