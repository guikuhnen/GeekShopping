using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace GeekShopping.APIGateway
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();

			builder.Services.AddAuthentication("Bearer")
				.AddJwtBearer("Bearer", options =>
				{
					options.Authority = builder.Configuration["ServiceUrls:IdentityServer"];
					options.TokenValidationParameters = new TokenValidationParameters()
					{
						ValidateAudience = false
					};
				});

			builder.Services.AddOcelot();

			var app = builder.Build();

			app.UseAuthentication();

			app.UseOcelot();

			app.Run();
		}
	}
}
