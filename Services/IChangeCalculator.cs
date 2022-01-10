using System.Collections.Generic;

namespace ChangeCalculator.Services
{
    public interface IChangeCalculator
    {
        Dictionary<double, int> CalculateChange(double paymentAmount, double productPrice, IEnumerable<double> allowedDenominations);
    }
}
