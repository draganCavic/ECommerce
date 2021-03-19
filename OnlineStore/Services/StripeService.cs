using OnlineStore.Interfaces;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Services
{
    public class StripeService : IStripeService
    {
        public async Task<dynamic> PayAsync(string cardNumber, int month, int year, string cvc, long value)
        {
			try
			{

				var optionsToken = new TokenCreateOptions
				{
					Card = new TokenCardOptions
					{
						Number = cardNumber,
						ExpMonth = month,
						ExpYear = year,
						Cvc = cvc
					}
				};

				var serviceToken = new Stripe.TokenService();
				Token stripeToken = await serviceToken.CreateAsync(optionsToken);

				var option = new ChargeCreateOptions
				{
					Amount = value,
					Currency = "eur",
					Description = "droga",
					Source = stripeToken.Id
				};

				var service = new ChargeService();
				Charge charge = await service.CreateAsync(option);

				if (charge.Paid)
				{
					return "Success";
				}
				else
				{
					return "Failed";
				}
			}
			catch (Exception e)
			{

				return e.Message;
			}
		}
    }
}
