using System;
using System.Collections.Generic;
using System.Linq;

namespace ChangeCalculator.Services
{
    public class ChangeCalculator : IChangeCalculator
    {
        public ChangeCalculator()
        {
        }
        public Dictionary<double, int> CalculateChange(double paymentAmount, double productPrice, IEnumerable<double> allowedDenominations)
        {
            var changeResult = new Dictionary<double, int>();

            var changeAmount = Math.Round(paymentAmount - productPrice, 2);
            var denominationList = allowedDenominations.OrderByDescending(d => d).ToList();

            while (changeAmount > 0)
            {
                foreach (var denomination in denominationList)
                {
                    if (changeAmount >= denomination)
                    {
                        if (changeResult.ContainsKey(denomination))
                            changeResult[denomination]++;
                        else
                            changeResult.Add(denomination, 1);

                        changeAmount = Math.Round(changeAmount - denomination, 2);
                        break;
                    }
                }
            }

            return changeResult;
        }
    }
}
