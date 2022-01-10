using System;
using System.Collections.Generic;
using System.Text;

namespace ChangeCalculator.Helpers
{
    public interface ICurrencyManager
    {
        /// <summary>
        /// Evaluates input amount values on the basis of currency symbols
        /// </summary>
        /// <param name="inputAmount"></param>
        /// <returns></returns>
        string EvaluateCurrencyValue(string inputAmount);

        /// <summary>
        /// Returns list of allowed denominations for a currency
        /// </summary>
        /// <returns></returns>
        IEnumerable<double> GetAllowedDenominations();

        /// <summary>
        /// Returns currency formatted amount
        /// </summary>
        /// <param name="inputValue"></param>
        /// <returns></returns>
        string GetFormattedValue(double inputValue);
    }
}
