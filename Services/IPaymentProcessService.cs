using ChangeCalculator.Models;

namespace ChangeCalculator.Services
{
    public interface IPaymentProcessService
    {
        PaymentResult ProcessPayment(string paymentAmount, string productPrice);
    }
}
