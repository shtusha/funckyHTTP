using System.Collections.Generic;
using System.Configuration;
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
        }
    }
}