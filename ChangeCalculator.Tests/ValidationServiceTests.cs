using System;
using System.Collections.Generic;
using System.Text;
using ChangeCalculator.Services;
using Xunit;

namespace ChangeCalculator.Tests
{
    public class ValidationServiceTests
    {
        private readonly IValidationService _validationService;
        public ValidationServiceTests()
        {
            _validationService = new ValidationService();
        }

        [Theory]
        [InlineData("TwentyThreePounds", "20")]
        [InlineData("223.78.98", "20")]
        [InlineData("22.@22", "20")]
        public void Returns_Invalid_Payment_Error(string paymentAmount, string productPrice)
        {
            var validationError = _validationService.ValidatePayment(paymentAmount, productPrice);
            Assert.NotEmpty(validationError);
            Assert.Equal("Invalid payment amount.", validationError);
        }

        [Theory]
        [InlineData("20", "TwentyThreePounds" )]
        [InlineData("20","223.78.98")]
        [InlineData("20","22.@22")]
        public void Returns_Invalid_ProductPrice_Error(string paymentAmount, string productPrice)
        {
            var validationError = _validationService.ValidatePayment(paymentAmount, productPrice);
            Assert.NotEmpty(validationError);
            Assert.Equal("Invalid product price.", validationError);
        }

        [Theory]
        [InlineData("-20", "12")]
        [InlineData("-12.55", "22.3")]
        [InlineData("-0.23", "22")]
        public void Returns_Invalid_Negative_PaymentAmount_Error(string paymentAmount, string productPrice)
        {
            var validationError = _validationService.ValidatePayment(paymentAmount, productPrice);
            Assert.NotEmpty(validationError);
            Assert.Equal("Payment amount cannot be negative.", validationError);
        }

        [Theory]
        [InlineData("20", "-12")]
        [InlineData("12.55", "-0.36")]
        [InlineData("0.23", "-22.09")]
        public void Returns_Invalid_Negative_ProductPrice_Error(string paymentAmount, string productPrice)
        {
            var validationError = _validationService.ValidatePayment(paymentAmount, productPrice);
            Assert.NotEmpty(validationError);
            Assert.Equal("Product price cannot be negative.", validationError);
        }

        [Theory]
        [InlineData("20", "22")]
        [InlineData("12.55", "13.60")]
        [InlineData("23.99978", "23.99988")]
        public void Returns_Not_Enough_Money_Error(string paymentAmount, string productPrice)
        {
            var validationError = _validationService.ValidatePayment(paymentAmount, productPrice);
            Assert.NotEmpty(validationError);
            Assert.Equal("Not enough money for purchase.", validationError);
        }

        [Theory]
        [InlineData("22", "20")]
        [InlineData("22.55", "13.60")]
        [InlineData("24.99978", "23.99988")]
        public void Returns_No_Error_For_Valid_PaymentInput(string paymentAmount, string productPrice)
        {
            var validationError = _validationService.ValidatePayment(paymentAmount, productPrice);
            Assert.Empty(validationError);
        }
    }
}
