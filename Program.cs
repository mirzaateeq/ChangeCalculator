using System;
using ChangeCalculator.Helpers;
using ChangeCalculator.Models;
using ChangeCalculator.Services;

namespace ChangeCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            var continuePurchase = true;
            while (continuePurchase)
            {
                Console.WriteLine("Enter Product Price in UK currency (examples: £20, £10.5, 50p) : ");
                var inputProductPrice = Console.ReadLine();

                Console.WriteLine("Enter Payment Amount in UK currency (examples: £20, £10.5, 50p) : ");
                var inputPaymentAmount = Console.ReadLine();

                Console.WriteLine("Processing Payment...");

                PaymentResult paymentResult;

                try
                {
                    IPaymentProcessService paymentProcessService =
                        new PaymentProcessService(new Services.ChangeCalculator(), new ValidationService(),
                            new UKCurrencyManager());

                    paymentResult = paymentProcessService.ProcessPayment(inputPaymentAmount, inputProductPrice);

                }
                catch (Exception e)
                {
                    Console.WriteLine($"Unexpected error while processing payment.{e.Message}");
                    return;
                }

                if (paymentResult.PaymentSuccessful)
                {
                    Console.WriteLine("Purchase Successful.");

                    if (paymentResult.ChangeDenomination.Count > 0)
                    {
                        Console.WriteLine($"Total Change to return = {paymentResult.ChangeAmount}");
                        Console.WriteLine($"Your change is: ");
                        foreach (var (key, value) in paymentResult.ChangeDenomination)
                        {
                            Console.WriteLine($"{value} x {key}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No change to return.");
                    }
                }
                else
                {
                    Console.WriteLine("Purchase failed. Details below.");
                    Console.WriteLine(paymentResult.ErrorMessage);
                }

                Console.WriteLine("Make another purchase ? (Y/N)");

                var inputContinue = Console.ReadLine();
                continuePurchase = inputContinue == "Y" || inputContinue == "y";
            }
        }
    }
}
