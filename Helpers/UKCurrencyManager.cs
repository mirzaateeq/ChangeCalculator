using System;
using System.Collections.Generic;

namespace ChangeCalculator.Helpers
{
    public class UKCurrencyManager : ICurrencyManager
    {
        public string EvaluateCurrencyValue(string inputAmount)
        {
            if (inputAmount.StartsWith("£"))
                return inputAmount.Replace("£", "");

            if (inputAmount.EndsWith("p"))
            {
                var inputNoSymbol = inputAmount.Replace("p", "").Trim();

                if (double.TryParse(inputNoSymbol, out var outputValue))
                    return Math.Round((outputValue / 100), 2).ToString();
            }

            return inputAmount;
        }

        public IEnumerable<double> GetAllowedDenominations()
        {
            return new List<double> { 5, 10, 20, 2, 1, 0.1, 0.2, 0.5, 0.05, 0.01 };
        }

        public string GetFormattedValue(double inputValue)
        {
            return inputValue >= 1 ? $"£{inputValue}" : $"{inputValue * 100}p";
        }
    }
}
