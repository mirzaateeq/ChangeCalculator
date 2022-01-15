using System.Collections.Generic;
using ChangeCalculator.Helpers;
using ChangeCalculator.Services;
using Moq;
using Xunit;

namespace ChangeCalculator.Tests
{
    public class PaymentServiceTests
    {
        private readonly IPaymentProcessService _paymentProcessService;
        private readonly Mock<IChangeCalculator> _mockChangeCalculator;
        private readonly Mock<IValidationService> _mockValidationService;
        private readonly Mock<ICurrencyManager> _mockCurrencyManager;
        public PaymentServiceTests()
        {
            _mockChangeCalculator = new Mock<IChangeCalculator>();
            _mockValidationService = new Mock<IValidationService>();
            _mockCurrencyManager = new Mock<ICurrencyManager>();

            _paymentProcessService = new PaymentProcessService(_mockChangeCalculator.Object,
                _mockValidationService.Object,
                _mockCurrencyManager.Object);
        }

        [Theory]
        [InlineData("-20", "20")]
        public void Returns_Payment_Error_For_Invalid_Input(string inputPaymentAmount, string inputProductPrice)
        {
            _mockCurrencyManager.Setup(c => c.EvaluateCurrencyValue(inputPaymentAmount)).Returns(inputPaymentAmount);
            _mockCurrencyManager.Setup(c => c.EvaluateCurrencyValue(inputProductPrice)).Returns(inputProductPrice);
            _mockValidationService.Setup(v => v.ValidatePayment(It.IsAny<string>(), It.IsAny<string>()))
                .Returns("Invalid input amount");
            var paymentResult = _paymentProcessService.ProcessPayment(inputPaymentAmount, inputProductPrice);

            Assert.NotNull(paymentResult);
            Assert.False(paymentResult.PaymentSuccessful);
            Assert.Equal("Invalid input amount", paymentResult.ErrorMessage);
        }

        [Theory]
        [InlineData("£20", "£9.6")]
        public void Returns_Payment_Success_Valid_Payment(string inputPaymentAmount, string inputProductPrice)
        {
             var allowedDenominations = new List<double> { 5, 10, 20, 2, 1, 0.1, 0.2, 0.5, 0.05, 0.01 };
            var mockChangeResult = new Dictionary<double, int>
            {
                { 10, 1 }, {0.2, 2}
            };

            _mockCurrencyManager.Setup(c => c.EvaluateCurrencyValue(inputPaymentAmount)).Returns("20");
            _mockCurrencyManager.Setup(c => c.EvaluateCurrencyValue(inputProductPrice)).Returns("9.6");
            _mockCurrencyManager.Setup(c => c.GetAllowedDenominations()).Returns(allowedDenominations);
            _mockCurrencyManager.Setup(c => c.GetFormattedValue(10)).Returns("£10");
            _mockCurrencyManager.Setup(c => c.GetFormattedValue(0.2)).Returns("20p");
            _mockCurrencyManager.Setup(c => c.GetFormattedValue(10.4)).Returns("£10.4");
            _mockValidationService.Setup(v => v.ValidatePayment(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(string.Empty);

            _mockChangeCalculator
                .Setup(c => c.CalculateChange(It.IsAny<double>(), It.IsAny<double>(), allowedDenominations))
                .Returns(mockChangeResult);

            
            var paymentResult = _paymentProcessService.ProcessPayment(inputPaymentAmount, inputProductPrice);

            Assert.NotNull(paymentResult);
            Assert.True(paymentResult.PaymentSuccessful);
            Assert.Equal(2, paymentResult.ChangeDenomination.Count);
            Assert.NotNull(paymentResult.ChangeAmount);
        }
    }
}
