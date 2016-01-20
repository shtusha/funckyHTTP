using System;
using System.IO;
using System.Configuration;
using System.Text;
using TechTalk.SpecFlow;

namespace FunckyHttp.Common
{
    [Binding]
    public class Transformations
    {
        [StepArgumentTransformation(Constants.Patterns.ValueSources.StringLiteral)]
        public Wrapped<string> StringFromLiteral(string value)
        {
            return value;
        }


        [StepArgumentTransformation(Constants.Patterns.ValueSources.Variable)]
        public Wrapped<string> StringFromVariable(string variable)
        {
            return ScenarioContextStore.Variables[variable].ToString();
        }



        [StepArgumentTransformation(@"CONFIG\((.*)\)")]
        public Wrapped<string> StringFromConfig(string key)
        {
            return new Wrapped<string>(ConfigurationManager.AppSettings[key]);
        }

        [StepArgumentTransformation(Constants.Patterns.ValueSources.File)]
        public Wrapped<string> StringFromFile(string path)
        {
            var filePath = Path.GetFullPath(Path.Combine(ConfigurationManager.AppSettings["contentPath"], path));

            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            throw new ArgumentException(string.Format("file not found: {0}", filePath ?? "<null>"));
        }

        [StepArgumentTransformation(@"response header (.*)")]
        public Wrapped<string> StringFromHeader(string headerName)
        {
            return ScenarioContextStore.HttpCallContext.Response.Headers[headerName];
        }

        [StepArgumentTransformation(@"query result")]
        public Wrapped<string> StringFromQueryResult()
        {
            return WrappedStringFromQueryResult();
        }

        public static Wrapped<string> WrappedStringFromQueryResult()
        {
            return ScenarioContextStore.QueryResult?.ToString();
        }

        public static byte[] BytesFromQueryResults()
        {
            var stringResult = WrappedStringFromQueryResult();
            return stringResult == null ? null :
                Encoding.Default.GetBytes(stringResult);
        }

        [StepArgumentTransformation(Constants.Patterns.ValueSources.Variable)]
        public IRegexTarget RegexTargetFromVariable(string variable)
        {
            return new RegexTargetProvider(() => ScenarioContextStore.Variables[variable].ToString(), $"[{variable}]");
        }

        [StepArgumentTransformation(@"response header (.*)")]
        public IRegexTarget RegexTargetFromHeader(string headerName)
        {
            return new RegexTargetProvider(
                ()=> ScenarioContextStore.HttpCallContext.Response.Headers[headerName],
                $"response header {headerName}");
        }

        [StepArgumentTransformation(Constants.Patterns.ValueSources.QueryResult)]
        public IRegexTarget RegexTargetFromQueryResult()
        {
            return new RegexTargetProvider(
                () => ScenarioContextStore.QueryResult.ToString(),
                "query result");
        }

        [StepArgumentTransformation(Constants.Patterns.ValueSources.QueryResult)]
        public IVariableValue VariableValueFromQueryResult()
        {
            return new VariableValueProvider(
                () => ScenarioContextStore.QueryResult,
                "query result");
        }

        [StepArgumentTransformation(@"response header (.*)")]
        public IVariableValue VariableValueFromHeader(string headerName)
        {
            return new VariableValueProvider(
                () => ScenarioContextStore.HttpCallContext.Response.Headers[headerName],
                $"response header {headerName}");
        }

        private static string GetFullPath(string path)
        {

            return
                Path.GetFullPath(Path.IsPathRooted(path)
                                     ? path
                                     : Path.Combine(ConfigurationManager.AppSettings["contentPath"], path));
        }
    }
}
