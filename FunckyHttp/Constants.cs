namespace FunckyHttp
{
    public static class Constants
    {
        public static class Patterns
        {
            public static class ValueSources
            {
                public const string Variable = @"\[(.*)\]";
                public const string File = @"FILE\((.*)\)";
                public const string QueryResult = @"query result";
                public const string RequestBody = @"request content";
                public const string StringLiteral = @"'(.*)'";
            }
        }
    }
}