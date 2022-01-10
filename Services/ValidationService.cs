using System;
using ChangeCalculator.Helpers;

namespace ChangeCalculator.Services
{
    public class ValidationService : IValidationService
    {
        public string ValidatePayment(string inputPaymentAmount, string inputProductPrice)
        {
            if (!double.TryParse(inputPaymentAmount, out var paymentAmount))
                return "Invalid payment amount.";

            if (!double.TryParse(inputProductPrice, out var productPrice))
                return "Invalid product price.";

            if(productPrice < 0)
                return "Product price cannot be negative.";

            if (paymentAmount < 0)
                return "Payment amount cannot be negative.";

            if (productPrice > paymentAmount)
                return "Not enough money for purchase.";

            return string.Empty;
        }
    }
}
