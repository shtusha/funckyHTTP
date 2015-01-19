using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FunckyApp.Core;

namespace FunckyApp.UnitTests
{
    [TestClass]
    public class TranslationEngineTests
    {
        private TranslationEngine _engine = new TranslationEngine(TranslationEngine.InflationaryEnglishDictionary);

        [TestMethod] public void ThrowOnInvalidInput()
        {
            new Action(() => new TranslationEngine(null)).ShouldThrow<ArgumentNullException>(
                "because null is invalid list of substitutions");

            _engine.Invoking(a => a.Translate(null)).ShouldThrow<ArgumentNullException>("because null is invlid input");
        }

        [TestMethod] public void OriginalPhraseMatchesInput()
        {
            var input = "Once upon a time someone ate three apples twice.";

            var translation = _engine.Translate(input);

            translation.OriginalPhrase.Should().Be(input);
        }

        [TestMethod] public void NonInflatebaleTextShouldMatchOriginal()
        {
            var translation = _engine.Translate("gold");

            translation.InflatedPhrase.Should().Be("gold");
            translation.InflatedPhrase.Should().Be(translation.OriginalPhrase);
            translation.Fragments.Should().HaveCount(1);
            translation.Fragments[0].IsInflated.Should().BeFalse();
        }

        [TestMethod] public void SimpleNumbersAreProperlyTranslated()
        {
            var input = "zero one two three four five six seven eight nine ten " +
                        "eleven twelve thirteen fourteen fifteen sixteen seventeen eighteen nineteen twenty";

            var expected = input
                .Split(' ')
                .Skip(1)
                .Aggregate((prev, curr) => prev + " " + curr)
                + " twenty one";

            var translation = _engine.Translate(input);
            translation.InflatedPhrase.Should().Be(expected);
        }

        [TestMethod] public void CapitalLettersAreProperlyHandled()
        {
            var input = "Once upon a time someone ate Three apples twice and later drank some juice too.";
            var expected = "Twice upon a time sometwo nine Four apples three times and lniner drank some juice three.";

            var translation = _engine.Translate(input);
            translation.InflatedPhrase.Should().Be(expected);

            translation = _engine.Translate(expected);
            translation.InflatedPhrase.Should().Be("Three times upon a time somethree ten Five apples four times and ltenr drank some juice four.");
        }

        [TestMethod] public void MultipleOccurencesAreProperlyTranslated()
        {
            var input = "To be or not to be? The answer tends to be \"To be\".";
            var expected = "Three be or not three be? The answer elevends three be \"Three be\".";

            var translation = _engine.Translate(input);
            translation.InflatedPhrase.Should().Be(expected);

        }

        [TestMethod] public void PhraseEndingWithTranslatableFragmentIsProperlyTranslated()
        {
            var input = "Before, I used to know what I'm here for";
            var expected = "Befive, I used three know what I'm here five";

            var translation = _engine.Translate(input);
            translation.InflatedPhrase.Should().Be(expected);
        }
    }
}
