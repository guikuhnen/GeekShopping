using GeekShopping.Email.Models;

namespace GeekShopping.Email.Repositories
{
	public interface IOrderRepository
	{
		Task UpdateOrderPaymentStatus(long orderHeaderId, bool status);
	}
}
