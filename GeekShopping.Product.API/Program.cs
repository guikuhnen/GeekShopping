using GeekShopping.Product.API.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.Product.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			var connection = builder.Configuration["MySQLConnection:MySQLConnectionString"];

			builder.Services.AddDbContext<MySQLContext>(options =>
				options.UseMySql(
					connection,
					new MySqlServerVersion(new Version(8, 0, 35))));

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
