using GeekShopping.Email.Models.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.Email.Repositories
{
	public class OrderRepository : IOrderRepository
	{
		private readonly DbContextOptions<MySQLContext> _context;

		public OrderRepository(DbContextOptions<MySQLContext> context)
		{
			_context = context;
		}

		public async Task UpdateOrderPaymentStatus(long orderHeaderId, bool status)
		{
			//await using (var _db = new MySQLContext(_context))
			//{
			//	var orderHeader = await _db.OrderHeaders.FirstOrDefaultAsync(o => o.Id == orderHeaderId);

			//	if (orderHeader != null)
			//	{
			//		orderHeader.PaymentStatus = status;

			//		await _db.SaveChangesAsync();
			//	}
			//}
		}
	}
}
