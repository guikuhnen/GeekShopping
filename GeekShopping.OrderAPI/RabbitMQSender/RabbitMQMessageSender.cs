using GeekShopping.OrderAPI.Messages;
using GeekShopping.MessageBus;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace GeekShopping.OrderAPI.RabbitMQSender
{
	public class RabbitMQMessageSender : IRabbitMQMessageSender
	{
		private readonly string _hostName;
		private readonly string _userName;
		private readonly string _password;
		private readonly IConfiguration _config;
		private IConnection _connection;

		public RabbitMQMessageSender(IConfiguration config)
		{
			_config = config;

			_hostName = _config.GetValue<string>("RabbitMQ:HostName");
			_userName = _config.GetValue<string>("RabbitMQ:UserName");
			_password = _config.GetValue<string>("RabbitMQ:Password");
		}

		public void SendMessage(BaseMessage baseMessage, string queueName)
		{
			if (ConnectionExists())
			{
				baseMessage.MessageCreated = DateTime.Now.ToUniversalTime();

				using var channel = _connection.CreateModel();
				{
					channel.QueueDeclare(queue: queueName, false, false, false, arguments: null);

					byte[] body = GetMessageAsByteArray(baseMessage);

					channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
				}
			}
		}

		private byte[] GetMessageAsByteArray(BaseMessage baseMessage)
		{
			var options = new JsonSerializerOptions
			{
				WriteIndented = true
			};

			string json = JsonSerializer.Serialize<PaymentVO>((PaymentVO)baseMessage, options);

			return Encoding.UTF8.GetBytes(json);
		}

		private bool ConnectionExists()
		{
			if (_connection != null)
				return true;

			CreateConnection();

			return _connection != null;
		}

		private void CreateConnection()
		{
			try
			{
				var factory = new ConnectionFactory
				{
					HostName = _hostName,
					UserName = _userName,
					Password = _password
				};

				_connection = factory.CreateConnection();
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
