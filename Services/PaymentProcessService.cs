using System;
using System.Collections.Generic;
using ChangeCalculator.Helpers;
using ChangeCalculator.Models;

namespace ChangeCalculator.Services
{
    public class PaymentProcessService : IPaymentProcessService
    {
        private readonly IChangeCalculator _changeCalculator;
        private readonly IValidationService _validationService;
        private readonly ICurrencyManager _currencyManager;

        public PaymentProcessService(IChangeCalculator changeCalculator, 
            IValidationService validationService,
            ICurrencyManager currencyManager)
        {
            _changeCalculator = changeCalculator;
            _validationService = validationService;
            _currencyManager = currencyManager;
        }

        public PaymentResult ProcessPayment(string inputPaymentAmount, string inputProductPrice)
        {
            inputPaymentAmount = _currencyManager.EvaluateCurrencyValue(inputPaymentAmount);
            inputProductPrice = _currencyManager.EvaluateCurrencyValue(inputProductPrice);

            var validationErrors = _validationService.ValidatePayment(inputPaymentAmount, inputProductPrice);
            if (!string.IsNullOrEmpty(validationErrors))
                return new PaymentResult()
                {
                    PaymentSuccessful = false,
                    ErrorMessage = validationErrors
                };

            var paymentAmount = double.Parse(inputPaymentAmount);
            var productPrice = double.Parse(inputProductPrice);
            var allowedDenominations = _currencyManager.GetAllowedDenominations();

            var changeResult = _changeCalculator.CalculateChange(paymentAmount, productPrice, allowedDenominations);

            return new PaymentResult()
            {
                PaymentSuccessful = true,
                ChangeAmount = _currencyManager.GetFormattedValue(Math.Round(paymentAmount - productPrice,2)),
                ChangeDenomination = FormatChangeResult(changeResult)
            };
        }

        private Dictionary<string, int> FormatChangeResult(Dictionary<double, int> changeResult)
        {
            var formattedChangeResult = new Dictionary<string, int>();
            foreach (var (key, value) in changeResult)
            {
                formattedChangeResult.Add(_currencyManager.GetFormattedValue(key), value);
            }

            return formattedChangeResult;
        }
    }
}
