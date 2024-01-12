using GeekShopping.OrderAPI.Messages;
using GeekShopping.OrderAPI.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace GeekShopping.OrderAPI.MessageConsumer
{
	public class RabbitMQPaymentConsumer : BackgroundService
	{
		private readonly OrderRepository _repository;
		private readonly IConfiguration _config;
		private IConnection _connection;
		private IModel _channel;
		private readonly string _exchangeName;
		private readonly string _paymentOrderUpdateQueueName;

		public RabbitMQPaymentConsumer(OrderRepository repository, IConfiguration config)
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
			_paymentOrderUpdateQueueName = _config.GetValue<string>("RabbitMQ:Queues:DirectOrderUpdate");

			_connection = factory.CreateConnection();
			_channel = _connection.CreateModel();

			_channel.ExchangeDeclare(_exchangeName, ExchangeType.Direct, durable: false);

			_channel.QueueDeclare(_paymentOrderUpdateQueueName, false, false, false, null);
			_channel.QueueBind(_paymentOrderUpdateQueueName, _exchangeName, "PaymentOrder");
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			stoppingToken.ThrowIfCancellationRequested();

			var consumer = new EventingBasicConsumer(_channel);

			consumer.Received += (channel, evt) =>
			{
				var content = Encoding.UTF8.GetString(evt.Body.ToArray());

				UpdatePaymentResultVO updatePaymentResultVO = JsonSerializer.Deserialize<UpdatePaymentResultVO>(content);

				UpdatePaymentStatus(updatePaymentResultVO).GetAwaiter().GetResult();

				_channel.BasicAck(evt.DeliveryTag, false);
			};

			_channel.BasicConsume(_paymentOrderUpdateQueueName, false, consumer);

			return Task.CompletedTask;
		}

		private async Task UpdatePaymentStatus(UpdatePaymentResultVO updatePaymentResultVO)
		{
			try
			{
				await _repository.UpdateOrderPaymentStatus(updatePaymentResultVO.OrderId, updatePaymentResultVO.Status);
			}
			catch (Exception)
			{
				// Log
				throw;
			}
		}
	}
}
