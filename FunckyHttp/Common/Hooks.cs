using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using TechTalk.SpecFlow;

namespace FunckyHttp.Common
{
    [Binding]
    public class Hooks
    {
        [BeforeScenario()]
        public static void Init()
        {
            ScenarioContextStore.RequestHeaders = new Dictionary<string, string>();
            ScenarioContextStore.BaseUrl = ConfigurationManager.AppSettings["baseUrl"];
            ScenarioContextStore.NamespaceManager = new XmlNamespaceManager(new NameTable());
        }


        private const string RootElemnetNameTag = "rootElementName";
        [BeforeScenario("rootElementName")]
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
    }
}