using Microsoft.EntityFrameworkCore;

namespace GeekShopping.CartAPI.Models.Context
{
	public class MySQLContext : DbContext
	{
		public MySQLContext() { }

		public MySQLContext(DbContextOptions<MySQLContext> options) : base(options) { }

		public DbSet<Product> Products { get; set; }

		// Cart
		public DbSet<CartHeader> CartHeaders { get; set; }
		public DbSet<CartDetail> CartDetails { get; set; }
	}
}
