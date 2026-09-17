using System.Globalization;
using TechOneCore.Services.Interface;
using TechOneCore.Services.Service;

namespace TechOneTest
{
    /// <summary>
    /// build nuit test for the NumberToWordsConverter service, to ensure the service works as expected.
    /// Reason for using NUnit is that it is a popular and widely used testing framework for .NET.
    /// Also it is a part of my old projects, so I am familiar with it and can use it effectively.
    /// </summary>
    public class NumberToWordsConverterTests
    {
        private INumberToWordsConverter _converter;

        [SetUp]
        public void Setup()
        {
            _converter = new NumberToWordsConverter();
        }

        private string Convert(string input)
        {
            return _converter.ConvertValueToString(
                decimal.Parse(input, CultureInfo.InvariantCulture));
        }

        
        [Test]
        public void ConvertValueToString_BriefExample_ReturnsExpectedWords()
        {
            Assert.That(Convert("100.05"),
                Is.EqualTo("ONE HUNDRED DOLLARS AND FIVE CENTS"));
        }

        [TestCase("0", "ZERO DOLLARS")]
        [TestCase("1", "ONE DOLLAR")]
        [TestCase("9", "NINE DOLLARS")]
        public void ConvertValueToString_SingleDigits_ReturnsExpectedWords(string input, string expected)
        {
            Assert.That(Convert(input), Is.EqualTo(expected));
        }

        [TestCase("10", "TEN DOLLARS")]       
        [TestCase("19", "NINETEEN DOLLARS")]
        [TestCase("20", "TWENTY DOLLARS")]
        [TestCase("99", "NINETY-NINE DOLLARS")]
        public void ConvertValueToString_TeensAndTens_ReturnsExpectedWords(string input, string expected)
        {
            Assert.That(Convert(input), Is.EqualTo(expected));
        }

        [TestCase("100.00", "ONE HUNDRED DOLLARS")]
        [TestCase("101", "ONE HUNDRED AND ONE DOLLARS")]
        [TestCase("999.0", "NINE HUNDRED AND NINETY-NINE DOLLARS")]
        public void ConvertValueToString_Hundreds_ReturnsExpectedWords(string input, string expected)
        {
            Assert.That(Convert(input), Is.EqualTo(expected));
        }

        [TestCase("1010", "ONE THOUSAND AND TEN DOLLARS")]
        [TestCase("1000100", "ONE MILLION ONE HUNDRED DOLLARS")]
        [TestCase("1000000005", "ONE BILLION AND FIVE DOLLARS")]
        [TestCase("1000002450269", "ONE TRILLION TWO MILLION FOUR HUNDRED AND FIFTY THOUSAND TWO HUNDRED AND SIXTY-NINE DOLLARS")]
        public void ConvertValueToString_Scales_ReturnsExpectedWords(string input, string expected)
        {
            Assert.That(Convert(input), Is.EqualTo(expected));
        }

        [TestCase("0.01", "ZERO DOLLARS AND ONE CENT")]
        [TestCase("0.45", "ZERO DOLLARS AND FORTY-FIVE CENTS")]
        [TestCase("1.01", "ONE DOLLAR AND ONE CENT")]
        public void ConvertValueToString_Cents_ReturnsExpectedWords(string input, string expected)
        {
            Assert.That(Convert(input), Is.EqualTo(expected));
        }

        [TestCase("-1", "MINUS ONE DOLLAR")]
        [TestCase("-0.50", "MINUS ZERO DOLLARS AND FIFTY CENTS")]
        [TestCase("-120.75",
            "MINUS ONE HUNDRED AND TWENTY DOLLARS AND SEVENTY-FIVE CENTS")]
        public void ConvertValueToString_NegativeAmounts_ReturnsExpectedWords(string input, string expected)
        {
            Assert.That(Convert(input), Is.EqualTo(expected));
        }

        
        [TestCase("123.456", "ONE HUNDRED AND TWENTY-THREE DOLLARS AND FORTY-SIX CENTS")]
        [TestCase("0.005", "ZERO DOLLARS AND ONE CENT")]
        [TestCase("0.125", "ZERO DOLLARS AND THIRTEEN CENTS")]
        public void ConvertValueToString_MoreThanTwoDecimals_RoundsAwayFromZero(string input, string expected)
        {
            Assert.That(Convert(input), Is.EqualTo(expected));
        }

        
        [Test]
        public void ConvertValueToString_CentsRollingOver_BecomesADollar()
        {
            Assert.That(Convert("0.999"), Is.EqualTo("ONE DOLLAR"));
        }

        [Test]
        public void ConvertValueToString_LongMaxValue_IsSupported()
        {
            var result = Convert("9223372036854775807");

            Assert.Multiple(() =>
            {
                Assert.That(result, Does.StartWith("NINE QUINTILLION"));
                Assert.That(result, Does.EndWith("DOLLARS"));
            });
        }

        [Test]
        public void ConvertValueToString_AboveLongMaxValue_ThrowsArgumentOutOfRange()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => _converter.ConvertValueToString(9223372036854775808m));
        }

        [Test]
        public void ConvertValueToString_Null_ThrowsArgumentNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => _converter.ConvertValueToString(null));
        }
    }
}