using Microsoft.EntityFrameworkCore;

namespace GeekShopping.Email.Models.Context
{
	public class MySQLContext : DbContext
	{
		public MySQLContext() { }

		public MySQLContext(DbContextOptions<MySQLContext> options) : base(options) { }

		public DbSet<EmailLog> Emails { get; set; }
	}
}
