
using GeekShopping.OrderAPI.Models.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace GeekShopping.OrderAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddDbContext<MySQLContext>(options =>
				options.UseMySql(
					builder.Configuration["MySQLConnection:MySQLConnectionString"],
					new MySqlServerVersion(new Version(8, 0, 35))));

			//builder.Services.AddScoped<IOrderRepository, OrderRepository>();

			//builder.Services.AddSingleton<IRabbitMQMessageSender, RabbitMQMessageSender>();

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

			builder.Services.AddAuthorization(options =>
			{
				options.AddPolicy("ApiScope", policy =>
				{
					policy.RequireAuthenticatedUser();
					policy.RequireClaim("scope", "geek_shopping");
				});
			});

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new OpenApiInfo { Title = "GeekShopping.OrderAPI", Version = "v1" });
				options.EnableAnnotations();
				options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Description = @"Enter 'Bearer' [space] and your token!",
					Name = "Authorization",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer"
				});
				options.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							},
							Scheme = "oauth2",
							Name = "Bearer",
							In= ParameterLocation.Header
						},
						new List<string> ()
					}
				});
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();
			app.UseAuthentication();
			app.UseAuthorization();
			app.MapControllers();

			app.Run();
		}
	}
}
