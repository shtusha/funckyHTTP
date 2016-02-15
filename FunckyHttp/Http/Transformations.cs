using System;
using System.IO;
using System.Text;
using System.Configuration;
using TechTalk.SpecFlow;

namespace FunckyHttp.Http
{
    [Binding]
    public class Transformations
    {
        [StepArgumentTransformation(Constants.Patterns.ValueSources.File)]
        public byte[] BytesFromFile(string path)
        {
            var filePath = Path.GetFullPath(Path.Combine(ConfigurationManager.AppSettings["contentPath"], path));

            if (File.Exists(filePath))
            {
                return File.ReadAllBytes(filePath);
            }
            throw new ArgumentException(string.Format("file not found: {0}", filePath ?? "<null>"));
        }

        [StepArgumentTransformation(Constants.Patterns.ValueSources.StringLiteral)]
        public byte[] BytesFromstringLiteral(string value)
        {
            return value == null ? null : Encoding.Default.GetBytes(value);
        }

        [StepArgumentTransformation(Constants.Patterns.ValueSources.QueryResult)]
        public byte[] BytesFromQueryResults()
        {
            return Common.Transformations.BytesFromQueryResults();
        }

    }
}
