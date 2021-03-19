using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Interfaces
{
    public interface IStripeService
    {
        public Task<dynamic> PayAsync(string cardNumber, int month, int year, string cvc, long value);
    }
}
