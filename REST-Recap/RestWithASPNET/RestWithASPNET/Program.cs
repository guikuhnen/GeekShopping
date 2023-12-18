using RestWithASPNET.Services;
using RestWithASPNET.Services.Implementations;

namespace RestWithASPNET
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();

			// Dependency Injection
			builder.Services.AddScoped<IPersonService, PersonService>();

			var app = builder.Build();

			// Configure the HTTP request pipeline.

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
