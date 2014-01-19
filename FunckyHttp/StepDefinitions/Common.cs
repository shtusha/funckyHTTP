﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FunckyHttp.Common;

namespace FunckyHttp.StepDefinitions
{
    [Binding]
    public static class Common  
    {
        [Then("(.*) should match (.*)")]
        public static void ShouldMatchRegex(IRegexTarget target, Wrapped<string> pattern)
        {
            Assert.IsTrue(System.Text.RegularExpressions.Regex.IsMatch(target.Value, pattern),
                string.Format("{0} does not match regex pattern {1}", target.Value, pattern));
        }
    }
    public interface IRegexTarget
        {
            string Value { get; }
        }

    public class RegexTargetMapper : IRegexTarget
    {
        private Func<string> _map;
        public RegexTargetMapper(Func<string> map)
        {
            _map = map;
        }
        public string Value { 
            get {
                return _map.Invoke();
            }
        }
    }
}