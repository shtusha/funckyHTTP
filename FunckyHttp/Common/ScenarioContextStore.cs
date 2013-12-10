using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using System.Xml;
using System.Xml.Xsl;
using TechTalk.SpecFlow;


namespace FunckyHttp.Common
{
    public static class ScenarioContextStore
    {
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

        public static string RequestMethod
        {
            get { return ScenarioContext.Current.GetContextItem<string>("http.request.method"); }
            set { ScenarioContext.Current["http.request.method"] = value; }
        }

        public static string RequestUrl
        {
            get { return ScenarioContext.Current.GetContextItem<string>("http.request.url"); }
            set { ScenarioContext.Current["http.request.url"] = value; }
        }

        public static byte[] RequestContent
        {
            get { return ScenarioContext.Current.GetContextItem<byte[]>("http.request.content"); }
            set { ScenarioContext.Current["http.request.content"] = value; }
        }

        public static Lazy<XPathDocument> RequestContentXML
        {
            get { return ScenarioContext.Current.GetContextItem<Lazy<XPathDocument>>("http.request.content.xml"); }
            set { ScenarioContext.Current["http.request.content.xml"] = value; }
        }

        public static byte[] ResponseContent
        {
            get { return ScenarioContext.Current.GetContextItem<byte[]>("http.response.content"); }
            set { ScenarioContext.Current["http.response.content"] = value; }
        }

        public static Lazy<XPathDocument> ResponseContentXML
        {
            get { return ScenarioContext.Current.GetContextItem<Lazy<XPathDocument>>("http.response.content.xml"); }
            set { ScenarioContext.Current["http.response.content.xml"] = value; }
        }

        public static XslCompiledTransform XSLTransform
        {
            get { return ScenarioContext.Current.GetContextItem<XslCompiledTransform>("xml.transformation"); }
            set { ScenarioContext.Current["xml.transformation"] = value; }
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

        public static void StoreUserVariable(string variableName, object value)
        {
            ScenarioContext.Current["variable." + variableName] = value;
        }

        public static object RetrieveUserVariable(string variableName)
        {
            return ScenarioContext.Current.GetContextItem<object>("variable." + variableName);
        }
    }
}
