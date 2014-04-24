using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Xml;
using TechTalk.SpecFlow;
using System.Text;
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
            ScenarioContextStore.DropXmlNamespaces = (ConfigurationManager.AppSettings["xml.namespaces.drop"] ?? "true")
                .Equals("true", StringComparison.CurrentCultureIgnoreCase);
        }


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