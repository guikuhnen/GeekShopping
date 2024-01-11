using Microsoft.EntityFrameworkCore;

namespace GeekShopping.OrderAPI.Models.Context
{
	public class MySQLContext : DbContext
	{
		public MySQLContext() { }

		public MySQLContext(DbContextOptions<MySQLContext> options) : base(options) { }

		// Order

		public DbSet<OrderHeader> OrderHeaders { get; set; }

		public DbSet<OrderDetail> OrderDetails { get; set; }
	}
}
