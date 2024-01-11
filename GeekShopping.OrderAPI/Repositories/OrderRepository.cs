using GeekShopping.OrderAPI.Models;
using GeekShopping.OrderAPI.Models.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.OrderAPI.Repositories
{
	public class OrderRepository : IOrderRepository
	{
		private readonly DbContextOptions<MySQLContext> _context;

		public OrderRepository(DbContextOptions<MySQLContext> context)
		{
			_context = context;
		}

		public async Task<bool> AddOrder(OrderHeader orderHeader)
		{
			if (orderHeader == null)
				return false;

			await using (var _db = new MySQLContext(_context))
			{
				_db.OrderHeaders.Add(orderHeader);

				await _db.SaveChangesAsync();

				return true;
			}
		}

		public async Task UpdateOrderPaymentStatus(long orderHeaderId, bool status)
		{
			await using (var _db = new MySQLContext(_context))
			{
				var orderHeader = await _db.OrderHeaders.FirstOrDefaultAsync(o => o.Id == orderHeaderId);

				if (orderHeader != null)
				{
					orderHeader.PaymentStatus = status;

					await _db.SaveChangesAsync();
				}
			}
		}
	}
}
