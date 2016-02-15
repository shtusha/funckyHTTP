using System;
using System.Configuration;
using FunckyHttp;
using FunckyHttp.Common;
using TechTalk.SpecFlow;

namespace FunckyApp.Tests
{
    [Binding]
    public sealed class Init
    {
        [BeforeScenario]
        public void BeforeScenario()
        {
            ScenarioContextStore.BaseUrl = ConfigurationManager.AppSettings["baseUrl"];
            ScenarioContextStore.DropXmlNamespaces = 
                ConfigurationManager.AppSettings["xml.namespaces.drop"]?.Equals("true", StringComparison.CurrentCultureIgnoreCase) ?? 
                ScenarioContextStore.DropXmlNamespaces;
        }
    }
}
