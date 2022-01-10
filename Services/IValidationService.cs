namespace ChangeCalculator.Services
{
    public interface IValidationService
    {
        string ValidatePayment(string inputPaymentAmount, string inputProductPrice);
    }
}
