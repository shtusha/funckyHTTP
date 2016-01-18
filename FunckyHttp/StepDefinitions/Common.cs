using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

        [Given(@"(.*) is saved into \[(.*)\]")]
        [When(@"(.*) is saved into \[(.*)\]")]
        [Then(@"(.*) is saved into \[(.*)\]")]
        public static void SetVariable(IVariableValue source, string variableName)
        {
            ScenarioContextStore.Variables[variableName] = source.Value;
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

    public interface IValueSource<T>
    {
        T Value { get; }
        string Description { get; }
    }
    public class ValueProvider<T> : IValueSource<T> 
    {
        private Func<T> _valueFunc;

        public ValueProvider(Func<T> valueFunc, string description)
        {
            _valueFunc = valueFunc;
            Description = description;
        }

        public T Value => _valueFunc.Invoke();
        public string Description { get; }
    }

    public interface IRegexTarget : IValueSource<string> { }
    public class RegexTargetProvider : ValueProvider<string>, IRegexTarget
    {
        public RegexTargetProvider(Func<string> valueFunc, string description) : base(valueFunc, description){}
    }

    public interface IVariableValue : IValueSource<object> { }
    public class VariableValueProvider :ValueProvider<object>, IVariableValue
    {
        public VariableValueProvider(Func<object> valueFunc, string description) : base(valueFunc, description){}
    }

}
