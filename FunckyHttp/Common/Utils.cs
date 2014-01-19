using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using System.Xml;
using System.IO;
using TechTalk.SpecFlow;
using Newtonsoft.Json;

namespace FunckyHttp.Common
{
    public static class Utils
    {
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
            else
            {
                using (var reader = XmlTextReader.Create(new MemoryStream(bytes), new XmlReaderSettings { ConformanceLevel = ConformanceLevel.Fragment }))
                {
                    return new XPathDocument(reader);
                }
            }
        }
    }
}
