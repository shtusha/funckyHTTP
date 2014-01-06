using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using TechTalk.SpecFlow;
using FunckyHttp.StepDefinitions;

namespace FunckyHttp.Common
{
    [Binding]
    public class Transformations
    {
        [StepArgumentTransformation(@"'(.*)'")]
        public Wrapped<string> StringFromLiteral(string value)
        {
            return value;
        }

        [StepArgumentTransformation(@"FILE\((.*)\)")]
        public Wrapped<string> StringFromFile(string path)
        {
            var filePath = Path.GetFullPath(Path.Combine(ConfigurationManager.AppSettings["contentPath"], path));

            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            throw new ArgumentException(string.Format("file not found: {0}", filePath ?? "<null>"));
        }

        [StepArgumentTransformation(@"FILE\((.*)\)")]
        public byte[] BytesFromFile(string path)
        {
            var filePath = Path.GetFullPath(Path.Combine(ConfigurationManager.AppSettings["contentPath"], path));

            if (File.Exists(filePath))
            {
                return File.ReadAllBytes(filePath);
            }
            throw new ArgumentException(string.Format("file not found: {0}", filePath ?? "<null>"));
        }

        [StepArgumentTransformation(@"response header (.*)")]
        public IRegexTarget RegexTargetFromHeader(string headerName)
        {
            return new RegexTargetMapper(()=> ScenarioContextStore.HttpCallContext.Headers[headerName]);
        }

        [StepArgumentTransformation(@"query result")]
        public IRegexTarget RegexTargetFromQueryResult()
        {
            return new RegexTargetMapper(() => ScenarioContextStore.QueryResult.ToString());
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
