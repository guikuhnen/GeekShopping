using Microsoft.EntityFrameworkCore;

namespace GeekShopping.Product.API.Model.Context
{
	public class MySQLContext : DbContext
	{
		public MySQLContext() { }

		public MySQLContext(DbContextOptions<MySQLContext> options) : base(options) { }
	}
}
