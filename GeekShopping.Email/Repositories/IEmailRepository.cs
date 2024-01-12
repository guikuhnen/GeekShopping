using GeekShopping.Email.Messages;
using GeekShopping.Email.Models;

namespace GeekShopping.Email.Repositories
{
	public interface IEmailRepository
	{
		Task SendAndLogEmail(UpdatePaymentResultMessage resultMessage);
	}
}
