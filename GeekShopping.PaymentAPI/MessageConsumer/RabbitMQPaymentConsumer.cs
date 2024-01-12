using GeekShopping.PaymentAPI.Messages;
using GeekShopping.PaymentAPI.RabbitMQSender;
using GeekShopping.PaymentProcessor;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace GeekShopping.PaymentAPI.MessageConsumer
{
	public class RabbitMQPaymentConsumer : BackgroundService
	{
		private readonly IProcessPayment _processPayment;
		private IRabbitMQMessageSender _messageSender;
		private readonly IConfiguration _config;
		private IConnection _connection;
		private IModel _channel;

		public RabbitMQPaymentConsumer(IProcessPayment processPayment, IRabbitMQMessageSender messageSender, IConfiguration config)
		{
			_processPayment = processPayment;
			_messageSender = messageSender;
			_config = config;

			var factory = new ConnectionFactory
			{
				HostName = _config.GetValue<string>("RabbitMQ:HostName"),
				UserName = _config.GetValue<string>("RabbitMQ:UserName"),
				Password = _config.GetValue<string>("RabbitMQ:Password")
			};

			_connection = factory.CreateConnection();
			_channel = _connection.CreateModel();
			_channel.QueueDeclare(queue: _config.GetValue<string>("RabbitMQ:Queues:OrderPaymentProcess"), false, false, false, arguments: null);
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			stoppingToken.ThrowIfCancellationRequested();

			var consumer = new EventingBasicConsumer(_channel);

			consumer.Received += (channel, evt) =>
			{
				var content = Encoding.UTF8.GetString(evt.Body.ToArray());

				PaymentMessage paymentMessageVO = JsonSerializer.Deserialize<PaymentMessage>(content);

				ProcessPayment(paymentMessageVO).GetAwaiter().GetResult();

				_channel.BasicAck(evt.DeliveryTag, false);
			};

			_channel.BasicConsume(_config.GetValue<string>("RabbitMQ:Queues:OrderPaymentProcess"), false, consumer);

			return Task.CompletedTask;
		}

		private async Task ProcessPayment(PaymentMessage paymentMessageVO)
		{
			var result = _processPayment.PaymentProcessor();

			UpdatePaymentResultMessage paymentResultMessage = new()
			{
				MessageCreated = DateTime.Now.ToUniversalTime(),
				Status = result,
				OrderId = paymentMessageVO.OrderId,
				Email = paymentMessageVO.Email
			};

			try
			{
				_messageSender.SendMessage(paymentResultMessage, _config.GetValue<string>("RabbitMQ:Queues:OrderPaymentResult"));
			}
			catch (Exception)
			{
				// Log
				throw;
			}
		}
	}
}
