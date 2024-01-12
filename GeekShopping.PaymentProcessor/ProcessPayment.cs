using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeekShopping.PaymentProcessor
{
	public class ProcessPayment : IProcessPayment
	{
		public async Task<bool> PaymentProcessor()
		{
			// Mock
			return await Task.FromResult(true);
		}
	}
}
