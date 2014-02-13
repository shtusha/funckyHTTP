using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Roslyn.Compilers;
using Roslyn.Compilers.CSharp;

namespace FunckyApp.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ScriptCompilerService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ScriptCompilerService.svc or ScriptCompilerService.svc.cs at the Solution Explorer and start debugging.
    public class ScriptCompilerService : IScriptCompilerService
    {
        public GetScriptStatsResponse GetScriptStats(GetTokenStatisticsRequest request)
        {
            try
            {
                var syntaxTree = SyntaxTree.ParseText(request.Script);
                var root = syntaxTree.GetRoot();
                var statsCollector = new StatsCollector();

                statsCollector.Visit(root);

                var response = new GetScriptStatsResponse
                {
                    NumericLiteralStatistics = statsCollector.GetNumericLiteralStats(),
                    StringLiteralStatistics = statsCollector.GetStringLiteralStats(),
                    IdentifierStatistics = statsCollector.GetInIdentifierStats(),
                    InvalidTokens = statsCollector.GetInvalidTokens().ToList(),
                };

                response.IsValid = response.InvalidTokens.Count == 0;
                return response;
            }
            catch { throw; }
        }


        public class StatsCollector : SyntaxWalker
        {
            private Dictionary<string, int> numericLiterals = new Dictionary<string, int>();
            private Dictionary<string, int> stringLiterals = new Dictionary<string, int>();
            private Dictionary<string, int> identifiers = new Dictionary<string, int>();
            private List<string> incompleteTokens = new List<string>();

            public override void VisitLiteralExpression(LiteralExpressionSyntax node)
            {
                base.VisitLiteralExpression(node);

                var value = node.Token.ValueText;
                var dict = node.Kind == SyntaxKind.NumericLiteralExpression ? numericLiterals : stringLiterals;
                {
                    dict[value] = 1 + (dict.ContainsKey(value) ? dict[value] : 0); 
                }
            }

            public override void VisitIncompleteMember(IncompleteMemberSyntax node)
            {
                base.VisitIncompleteMember(node);
                incompleteTokens.Add(node.ToFullString());
            }

            public override void VisitVariableDeclaration(VariableDeclarationSyntax node)
            {
                base.VisitVariableDeclaration(node);
                foreach (var identifier in node.Variables.Select(a => a.Identifier.ValueText))
                {
                    identifiers[identifier] = 1 + (identifiers.ContainsKey(identifier) ? identifiers[identifier] : 0); 
                }
            }

            public List<LiteralStats> GetNumericLiteralStats()
            {
                return GetLiteralStats(numericLiterals);
            }

            public List<LiteralStats> GetStringLiteralStats()
            {
                return GetLiteralStats(stringLiterals);
            }

            public List<IdentifierStats> GetInIdentifierStats()
            {
                return identifiers.Select(a => new IdentifierStats
                {
                    Count = a.Value,
                    Name = a.Key

                }).OrderByDescending(a => a.Count).ThenBy(a => a.Name).ToList();
            }

            public IEnumerable<string> GetInvalidTokens()
            {
                return incompleteTokens;
            }



            private List<LiteralStats> GetLiteralStats(Dictionary<string,int> source)
            {
                return source.Select(a => new LiteralStats
                    {
                        Count = a.Value,
                        Value = a.Key
                    }).OrderByDescending(a => a.Count).ThenBy(a => a.Value).ToList();
            }
        }
    }
}
