using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using System.Xml;
using System.Xml.Xsl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using TechTalk.SpecFlow;


namespace FunckyHttp.Common
{
    public static class ScenarioContextStore
    {
        public static string JsonToXMLRootElementName
        {
            get { return ScenarioContext.Current.GetContextItem<string>("xml.fromJson.rootElementName"); }
            set { ScenarioContext.Current["xml.fromJson.rootElementName"] = value; }
        }

        public static bool StripXmlNamespaces 
        {
            get { return (bool)ScenarioContext.Current["xml.eliminatenamespaces"]; }
            set { ScenarioContext.Current["xml.eliminatenamespaces"] = value; }
        }

        public static XPathExpression Query
        {
            get { return ScenarioContext.Current.GetContextItem<XPathExpression>("xpath.query"); }
            set { ScenarioContext.Current["xpath.query"] = value; }
        }
        public static string QueryDescription
        {
            get { return ScenarioContext.Current.GetContextItem<string>("xpath.query.description"); }
            set { ScenarioContext.Current["xpath.query.description"] = value; }
        }

        public static object QueryResult
        {
            get { return ScenarioContext.Current.GetContextItem<object>("xpath.query.result"); }
            set { ScenarioContext.Current["xpath.query.result"] = value; }
        }

        public static XmlNamespaceManager NamespaceManager
        {
            get { return ScenarioContext.Current.GetContextItem<XmlNamespaceManager>("xml.namespacemanager"); }
            set { ScenarioContext.Current["xml.namespacemanager"] = value; }
        }

        public static Dictionary<string, string> RequestHeaders
        {
            get { return ScenarioContext.Current.GetContextItem<Dictionary<string, string>>("http.request.headers"); }
            set { ScenarioContext.Current["http.request.headers"] = value; }
        }

        public static HttpMethodCallContext HttpCallContext
        {
            get { return ScenarioContext.Current.GetContextItem<HttpMethodCallContext>("http.call"); }
            set { ScenarioContext.Current["http.call"] = value; }
        }

        public static string BaseUrl
        {
            get { return ScenarioContext.Current.GetContextItem<string>("http.baseurl"); }
            set { ScenarioContext.Current["http.baseurl"] = value; }
        }

        public static T GetContextItem<T>(this SpecFlowContext context, string key) where T : class
        {
            if (context.ContainsKey(key))
            {
                return context[key] == null ? null : context.Get<T>(key);
            }
            return null;
        }

        public static XslCompiledTransform XSLTransform { get; set; }
    }
}
