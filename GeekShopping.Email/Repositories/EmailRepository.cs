using GeekShopping.Email.Messages;
using GeekShopping.Email.Models;
using GeekShopping.Email.Models.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.Email.Repositories
{
	public class EmailRepository : IEmailRepository
	{
		private readonly DbContextOptions<MySQLContext> _context;

		public EmailRepository(DbContextOptions<MySQLContext> context)
		{
			_context = context;
		}

		public async Task SendAndLogEmail(UpdatePaymentResultMessage resultMessage)
		{
			EmailLog emailLog = new()
			{
				Email = resultMessage.Email,
				Log = $"Order - {resultMessage.OrderId} has been created successfully!",
				SendDate = DateTime.Now.ToUniversalTime()
			};

			await using (var _db = new MySQLContext(_context))
			{
				_db.Emails.Add(emailLog);

				await _db.SaveChangesAsync();
			}
		}
	}
}
