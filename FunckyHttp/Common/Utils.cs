using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using System.Xml;
using System.IO;

using TechTalk.SpecFlow;

namespace FunckyHttp.Common
{
    public static class Utils
    {
        public static void ResetRequestXML()
        {
            ScenarioContextStore.RequestContentXML = new Lazy<XPathDocument>(() => BytesToXML(ScenarioContextStore.RequestContent), true);
        }

        public static void ResetResponseXML()
        {
            ScenarioContextStore.ResponseContentXML = new Lazy<XPathDocument>(() => BytesToXML(ScenarioContextStore.ResponseContent), true);
        }

        private static XPathDocument BytesToXML(byte[] bytes)
        {
            using (var reader = XmlTextReader.Create(new MemoryStream(bytes), new XmlReaderSettings { ConformanceLevel = ConformanceLevel.Fragment }))
            {
                return new XPathDocument(reader);
            }
        }
    }
}
