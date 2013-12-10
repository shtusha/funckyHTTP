using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

using FunckyHttp.Common;

namespace FunckyHttp.StepDefinitions
{
    [Binding]
    class Common
    {
        [Then("(.*) should match the following regex (.*)")]
        public void ThenShouldMatchRegex(RegexTarget target, Wrapped<string> pattern)
        {
            Assert.IsTrue(System.Text.RegularExpressions.Regex.IsMatch(target.Value, pattern));
        }

        public class RegexTarget
        {
            public string Value;
        }
    }
}
