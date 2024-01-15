using OpenAI_API;

namespace ChatGPT.ASP.Net.Integration.Extensions
{
	public static class ChatGptExtensions
	{
		public static WebApplicationBuilder AddChatGpt(this WebApplicationBuilder builder, IConfiguration configuration)
		{
			var key = configuration["ChatGpt:Key"];
			var chat = new OpenAIAPI(key);

			builder.Services.AddSingleton(chat);

			return builder;
		}
	}
}
