using GeekShopping.MessageBus;
using GeekShopping.OrderAPI.Repositories;
using RabbitMQ.Client;

namespace GeekShopping.OrderAPI.MessageConsumer
{
	public class RabbitMQMessageConsumer : BackgroundService
	{
		private readonly OrderRepository _repository;
		private readonly IConfiguration _config;
		private IConnection _connection;
		private IModel _channel;

		public RabbitMQMessageConsumer(OrderRepository repository, IConfiguration config)
		{
			_repository = repository;
			_config = config;

			var factory = new ConnectionFactory
			{
				HostName = _config.GetValue<string>("RabbitMQ:HostName"),
				UserName = _config.GetValue<string>("RabbitMQ:UserName"),
				Password = _config.GetValue<string>("RabbitMQ:Password")
			};

			_connection = factory.CreateConnection();
			_channel = _connection.CreateModel();
			_channel.QueueDeclare(queue: "checkoutqueue", false, false, false, arguments: null);
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			throw new NotImplementedException();
		}
	}
}
