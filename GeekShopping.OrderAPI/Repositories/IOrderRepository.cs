using GeekShopping.OrderAPI.Models;

namespace GeekShopping.OrderAPI.Repositories
{
	public interface IOrderRepository
	{
		Task<bool> AddOrder(OrderHeader orderHeader);

		Task UpdateOrderPaymentStatus(long orderHeaderId, bool status);
	}
}
