using ChangeCalculator.Helpers;
using Xunit;

namespace ChangeCalculator.Tests
{
    public class UKCurrencyManagerTests
    {
        private readonly ICurrencyManager _ukCurrencyManager;
        public UKCurrencyManagerTests()
        {
            _ukCurrencyManager = new UKCurrencyManager();
        }

        [Theory]
        [InlineData("£40", "40")]
        [InlineData("£40.5", "40.5")]
        [InlineData(" £40.5 ", "40.5")]
        public void Evaluates_Correct_Pound_Value(string inputAmount, string expectedOutput)
        {
            var currencyValue = _ukCurrencyManager.EvaluateCurrencyValue(inputAmount);
            Assert.NotEmpty(currencyValue);
            Assert.Equal(expectedOutput, currencyValue);
        }

        [Theory]
        [InlineData("40p", "0.4")]
        [InlineData("10p", "0.1")]
        [InlineData("5p", "0.05")]
        [InlineData(" 5p ", "0.05")]
        public void Evaluates_Correct_Pence_Value(string inputAmount, string expectedOutput)
        {
            var currencyValue = _ukCurrencyManager.EvaluateCurrencyValue(inputAmount);
            Assert.NotEmpty(currencyValue);
            Assert.Equal(expectedOutput, currencyValue);
        }

        [Theory]
        [InlineData("TwentyPound")]
        [InlineData("p10")]
        [InlineData("5£")]
        public void Returns_Same_Value_When_Invalid_UseOfSymbols(string inputAmount)
        {
            var currencyValue = _ukCurrencyManager.EvaluateCurrencyValue(inputAmount);
            Assert.NotEmpty(currencyValue);
            Assert.Equal(inputAmount, currencyValue);
        }

        [Theory]
        [InlineData(0.5, "50p")]
        [InlineData(10, "£10")]
        [InlineData(0.01, "1p")]
        [InlineData(10.5, "£10.5")]
        public void Returns_Correct_Formatted_Amount_String(double inputAmount, string expectedOutput)
        {
            var formattedAmount = _ukCurrencyManager.GetFormattedValue(inputAmount);

            Assert.NotNull(formattedAmount);
            Assert.NotEmpty(formattedAmount);
            Assert.Equal(expectedOutput, formattedAmount);
        }

        [Fact]
        public void Returns_Allowed_Denomination_List()
        {
            var denominationList = _ukCurrencyManager.GetAllowedDenominations();
            Assert.NotEmpty(denominationList);
        }
    }
}
