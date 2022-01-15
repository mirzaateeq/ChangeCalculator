using System;
using System.Collections.Generic;
using ChangeCalculator.Services;
using Xunit;

namespace ChangeCalculator.Tests
{
    public class ChangeCalculatorTests
    {
        private readonly IChangeCalculator _changeCalculator;
        private readonly List<double> _allowedDenominations;
        public ChangeCalculatorTests()
        {
            _changeCalculator = new Services.ChangeCalculator();
            _allowedDenominations = new List<double> {5, 10, 20, 2, 1, 0.1, 0.2, 0.5, 0.05, 0.01};
        }

        
        [Theory]
        [InlineData(0,0)]
        [InlineData(10.5, 10.5)]
        [InlineData(10.4358, 10.4358)]
        [InlineData(12, 12)]
        public void Returns_NoChange_When_Exact_Amount_Paid(double paymentAmount, double productPrice)
        {
            var result = _changeCalculator.CalculateChange(paymentAmount, productPrice, _allowedDenominations);
            Assert.Empty(result);
        }

        [Theory]
        [InlineData(20, 10)]
        [InlineData(20.5, 10.5)]
        [InlineData(20.5554, 10.5554)]
        public void Returns_Single_ExpectedChange(double paymentAmount, double productPrice)
        {
            var result = _changeCalculator.CalculateChange(paymentAmount, productPrice, _allowedDenominations);
            Assert.NotEmpty(result);
            Assert.Single(result);
            Assert.Equal(1, result[10]);
        }

        [Theory]
        [InlineData(20, 14)]
        [InlineData(20.5, 14.5)]
        [InlineData(20.115, 14.115)]
        public void Returns_Two_ExpectedChange_Pounds(double paymentAmount, double productPrice)
        {
            var result = _changeCalculator.CalculateChange(paymentAmount, productPrice, _allowedDenominations);
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);

            Assert.Equal(1, result[5]);
            Assert.Equal(1, result[1]);
        }

        [Theory]
        [InlineData(20, 19.70)]
        [InlineData(20.30, 20)]
        public void Returns_Two_ExpectedChange_Pence(double paymentAmount, double productPrice)
        {
            var result = _changeCalculator.CalculateChange(paymentAmount, productPrice, _allowedDenominations);
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);

            Assert.Equal(1, result[0.2]);
            Assert.Equal(1, result[0.1]);
        }

        [Theory]
        [InlineData(20, 18.40)]
        public void Returns_Three_ExpectedChange(double paymentAmount, double productPrice)
        {
            var result = _changeCalculator.CalculateChange(paymentAmount, productPrice, _allowedDenominations);
            Assert.NotEmpty(result);
            Assert.Equal(3, result.Count);

            Assert.Equal(1, result[1]);
            Assert.Equal(1, result[0.5]);
            Assert.Equal(1, result[0.1]);
        }

    }
}
