using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using FunckyHttp;
using FunckyHttp.Common;
using TechTalk.SpecFlow;

namespace FunckyHttp
{
    [Binding]
    public sealed class Init
    {
        [BeforeScenario]
        public void BeforeScenario()
        {
            ScenarioContextStore.Variables = new Dictionary<string, object>();
            ScenarioContextStore.GlobalRequestHeaders = new Dictionary<string, string>();
            ScenarioContextStore.NamespaceManager = new XmlNamespaceManager(new NameTable());
            ScenarioContextStore.DropXmlNamespaces = true;
        }
    }
}
