using System.Diagnostics;
using TechTalk.SpecFlow;
using FunckyHttp.Common;

namespace FunckyHttp.Xml
{
    [Binding]
    public class Tags
    {
        private const string RootElemnetNameTag = "rootElementName";
        [BeforeScenario(RootElemnetNameTag)]
        public static void BeforeJsonToXMLScenario()
        {
            var tags = ScenarioContext.Current.ScenarioInfo.Tags;
            for (int i = 0; i < tags.Length; i++)
            {
                if(tags[i] == RootElemnetNameTag && tags.Length > i+1)
                {
                    ScenarioContextStore.JsonToXMLRootElementName = tags[i + 1];
                    return;
                }
            }
        }

        [BeforeScenario("xml.namespaces.keep")]
        public static void KeepXmlNamespaces()
        {
            Debug.WriteLine("xml.namespaces.drop: false");
            ScenarioContextStore.DropXmlNamespaces = false;
        }

        [BeforeScenario("xml.namespaces.drop")]
        public static void DropXmlNamespaces()
        {
            ScenarioContextStore.DropXmlNamespaces = true;
            Debug.WriteLine("xml.namespaces.drop: true");
        }
    }
}