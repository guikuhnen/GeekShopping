using GeekShopping.Email.Messages;
using GeekShopping.Email.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace GeekShopping.Email.MessageConsumer
{
	public class RabbitMQPaymentConsumer : BackgroundService
	{
		private readonly EmailRepository _repository;
		private readonly IConfiguration _config;
		private IConnection _connection;
		private IModel _channel;
		private readonly string _exchangeName;
		private readonly string _paymentEmailUpdateQueueName;

		public RabbitMQPaymentConsumer(EmailRepository repository, IConfiguration config)
		{
			_repository = repository;
			_config = config;

			var factory = new ConnectionFactory
			{
				HostName = _config.GetValue<string>("RabbitMQ:HostName"),
				UserName = _config.GetValue<string>("RabbitMQ:UserName"),
				Password = _config.GetValue<string>("RabbitMQ:Password")
			};
			_exchangeName = _config.GetValue<string>("RabbitMQ:Exchanges:DirectPaymentUpdate");
			_paymentEmailUpdateQueueName = _config.GetValue<string>("RabbitMQ:Queues:DirectEmailUpdate");

			_connection = factory.CreateConnection();
			_channel = _connection.CreateModel();

			_channel.ExchangeDeclare(_exchangeName, ExchangeType.Direct, durable: false);

			_channel.QueueDeclare(_paymentEmailUpdateQueueName, false, false, false, null);
			_channel.QueueBind(_paymentEmailUpdateQueueName, _exchangeName, "PaymentEmail");
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			stoppingToken.ThrowIfCancellationRequested();

			var consumer = new EventingBasicConsumer(_channel);

			consumer.Received += (channel, evt) =>
			{
				var content = Encoding.UTF8.GetString(evt.Body.ToArray());

				UpdatePaymentResultMessage updatePaymentResultMessage = JsonSerializer.Deserialize<UpdatePaymentResultMessage>(content);

				ProcessLogs(updatePaymentResultMessage).GetAwaiter().GetResult();

				_channel.BasicAck(evt.DeliveryTag, false);
			};

			_channel.BasicConsume(_paymentEmailUpdateQueueName, false, consumer);

			return Task.CompletedTask;
		}

		private async Task ProcessLogs(UpdatePaymentResultMessage updatePaymentResultMessage)
		{
			try
			{
				await _repository.SendAndLogEmail(updatePaymentResultMessage);
			}
			catch (Exception)
			{
				// Log
				throw;
			}
		}
	}
}
