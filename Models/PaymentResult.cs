using System.Collections.Generic;

namespace ChangeCalculator.Models
{
    public class PaymentResult
    {
        public bool PaymentSuccessful { get; set; }
        public string ChangeAmount { get; set; }
        public Dictionary<string, int> ChangeDenomination { get; set; }

        public string ErrorMessage { get; set; }
    }
}
