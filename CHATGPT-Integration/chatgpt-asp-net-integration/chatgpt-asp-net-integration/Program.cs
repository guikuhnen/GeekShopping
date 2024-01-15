using ChatGPT.ASP.Net.Integration.Extensions;

var appName = "ChatGPT ASP.NET 8 Integration";

var builder = WebApplication.CreateBuilder(args);

builder.AddSerilog();
builder.AddChatGpt(builder.Configuration);

// Add services to the container.
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwagger(appName);
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwaggerDoc(appName);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
