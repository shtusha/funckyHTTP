using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using FluentAssertions;

using FunckyHttp.Common;

namespace FunckyHttp.StepDefinitions
{
    [Binding]
    public static class Common  
    {

        [Given(@"all is cool"), When(@"all is cool"),Then(@"all is cool")]
        public static void AllIsCool()
        {
            Debug.WriteLine("All is cool");
        }


        [Then("(.*) should match (.*)")]
        public static void ShouldMatchRegex(IRegexTarget target, Wrapped<string> pattern)
        {
            Debug.WriteLine($"regex.target:\n{target.Description}");
            Debug.WriteLine($"regex.pattern:\n{pattern}");
            target.Value
                .Should()
                .MatchRegex(pattern, $"{target.Description} is expected to match that pattern");
        }
    }
    public interface IRegexTarget
        {
            string Value { get; }
            string Description { get; }
        }

    public class RegexTargetMapper : IRegexTarget
    {
        private Func<string> _map;
        public RegexTargetMapper(Func<string> map, string description)
        {
            _map = map;
            Description = description;
        }
        public string Value { 
            get {
                return _map.Invoke();
            }
        }
        public string Description { get; private set; }
    }
}
