using GeekShopping.MessageBus;
using GeekShopping.PaymentAPI.Messages;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace GeekShopping.PaymentAPI.RabbitMQSender
{
	public class RabbitMQMessageSender : IRabbitMQMessageSender
	{
		private readonly string _hostName;
		private readonly string _userName;
		private readonly string _password;
		private readonly string _exchangeName;
		private readonly string _paymentEmailUpdateQueueName;
		private readonly string _paymentOrderUpdateQueueName;
		private IConnection _connection;
		private readonly IConfiguration _config;

		public RabbitMQMessageSender(IConfiguration config)
		{
			_config = config;

			_hostName = _config.GetValue<string>("RabbitMQ:HostName");
			_userName = _config.GetValue<string>("RabbitMQ:UserName");
			_password = _config.GetValue<string>("RabbitMQ:Password");
			_exchangeName = _config.GetValue<string>("RabbitMQ:Exchanges:DirectPaymentUpdate");
			_paymentEmailUpdateQueueName = _config.GetValue<string>("RabbitMQ:Queues:DirectEmailUpdate");
			_paymentOrderUpdateQueueName = _config.GetValue<string>("RabbitMQ:Queues:DirectOrderUpdate");
		}

		public void SendMessage(BaseMessage baseMessage)
		{
			if (ConnectionExists())
			{
				baseMessage.MessageCreated = DateTime.Now.ToUniversalTime();

				using var channel = _connection.CreateModel();
				{
					channel.ExchangeDeclare(_exchangeName, ExchangeType.Direct, durable: false);

					channel.QueueDeclare(_paymentEmailUpdateQueueName, false, false, false, null);
					channel.QueueBind(_paymentEmailUpdateQueueName, _exchangeName, "PaymentEmail");

					channel.QueueDeclare(_paymentOrderUpdateQueueName, false, false, false, null);
					channel.QueueBind(_paymentOrderUpdateQueueName, _exchangeName, "PaymentOrder");

					byte[] body = GetMessageAsByteArray(baseMessage);

					channel.BasicPublish(exchange: _exchangeName, "PaymentEmail", basicProperties: null, body: body);
					channel.BasicPublish(exchange: _exchangeName, "PaymentOrder", basicProperties: null, body: body);
				}
			}
		}

		private byte[] GetMessageAsByteArray(BaseMessage baseMessage)
		{
			var options = new JsonSerializerOptions
			{
				WriteIndented = true
			};

			string json = JsonSerializer.Serialize<UpdatePaymentResultMessage>((UpdatePaymentResultMessage)baseMessage, options);

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
				// Log
				throw;
			}
		}
	}
}
