using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace FunckyApp.Core
{
    public class TranslationEngine
    {
        public static readonly ReadOnlyDictionary<string, string> InflationaryEnglishDictionary =
            new ReadOnlyDictionary<string, string>(new Dictionary<string, string> {
                {"zero",        "one"},
                {"one",         "two"},
                {"won",         "two"},
                {"two",         "three"},
                {"too",         "three"},
                {"to",          "three"},
                {"three",       "four"},
                {"four",        "five"},
                {"fore",        "five"},
                {"for",         "five"},
                {"five",        "six"},
                {"six",         "seven"},
                {"seven",       "eight"},
                {"eight",       "nine"},
                {"ate",         "nine"},
                {"nine",        "ten"},
                {"ten",         "eleven"},
                {"eleven",      "twelve"},
                {"twelve",      "thirteen"},
                {"thirteen",    "fourteen"},
                {"fourteen",    "fifteen"},
                {"fifteen",     "sixteen"},
                {"sixteen",     "seventeen"},
                {"seventeen",   "eighteen"},
                {"eighteen",    "nineteen"},
                {"nineteen",    "twenty"},
                {"twenty",      "twenty one"},
                {"once",        "twice"},
                {"twice",       "three times"}
            });

        private readonly List<KeyValuePair<string, string>> _substitutions;
        
        //used for optimization;
        private readonly int _minSearchPatternLength;

        public TranslationEngine(IDictionary<string, string> dictionary)
        {
            if (dictionary == null) { throw new ArgumentNullException("dictionary"); }
    
            //we will always process longest substitutions first.
            _substitutions = dictionary.OrderByDescending(a=>a.Key.Length).ToList();

            _minSearchPatternLength = _substitutions.Count > 0
                ? _substitutions.Last().Key.Length
                : int.MaxValue;

        }


        public Translation Translate(string phrase, int inflationRate = 1)
        {
            if (phrase == null)
            {
                throw new ArgumentNullException("phrase");
            }
            if (inflationRate < 1 || inflationRate > 5)
            {
                throw new ArgumentOutOfRangeException("inflationRate", "must be between 1 and 5");
            }

            var translation = FindFragments(phrase, 0).Aggregate(
                new Translation(),
                (trans, nextFragment) => trans.AddFragment(nextFragment));

            for (int i = 1; i < inflationRate; i++)
            {
                translation = FindFragments(translation.InflatedPhrase, 0).Aggregate(
                        new Translation(),
                        (trans, nextFragment) => trans.AddFragment(nextFragment));
            }

            translation.InflationRate = inflationRate;
            return translation;
        }

        private IEnumerable<Fragment> FindFragments(string input, int iteration)
        {
            if (iteration == _substitutions.Count || input.Length < _minSearchPatternLength )
            {
                yield return new Fragment(input); //original fragment
                yield break;
            }

            var substitution = _substitutions[iteration++];

            int lastMatchIndex = 0;
            foreach (Match match in Regex.Matches(input, substitution.Key, RegexOptions.IgnoreCase))
            {
                if (match.Index > lastMatchIndex) //process section of the string betweeen matches
                {
                    int length = match.Index - lastMatchIndex;
                    var newInput = input.Substring(lastMatchIndex, length);

                    foreach (var fragment in FindFragments(newInput, iteration))
                    {
                        yield return fragment;
                    }
                }

                lastMatchIndex = match.Index + match.Length;
                yield return new Fragment(match.Value, substitution.Value); //translated fragment
            }

            var remainder = input.Substring(lastMatchIndex, input.Length - lastMatchIndex);

            if (remainder.Length > 0) //something is left after the last match
            {
                foreach (var fragment in FindFragments(remainder, iteration))
                {
                    yield return fragment;
                }      
            }
        }
    }
}