using GeekShopping.MessageBus;
using GeekShopping.OrderAPI.Messages;
using GeekShopping.OrderAPI.Models;
using GeekShopping.OrderAPI.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading.RateLimiting;

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
			stoppingToken.ThrowIfCancellationRequested();

			var consumer = new EventingBasicConsumer(_channel);

			consumer.Received += (channel, evt) =>
			{
				var content = Encoding.UTF8.GetString(evt.Body.ToArray());

				CheckoutHeaderVO checkoutHeaderVO = JsonSerializer.Deserialize<CheckoutHeaderVO>(content);

				ProcessOrder(checkoutHeaderVO).GetAwaiter().GetResult();

				_channel.BasicAck(evt.DeliveryTag, false);
			};

			_channel.BasicConsume("checkoutqueue", false, consumer);

			return Task.CompletedTask;
		}

		private async Task ProcessOrder(CheckoutHeaderVO checkoutHeaderVO)
		{
			OrderHeader orderHeader = new()
			{
				UserId = checkoutHeaderVO.UserId,
				FirstName = checkoutHeaderVO.FirstName,
				LastName = checkoutHeaderVO.LastName,
				OrderDetails = new List<OrderDetail>(),
				CardNumber = checkoutHeaderVO.CardNumber,
				CouponCode = checkoutHeaderVO.CouponCode,
				CVV = checkoutHeaderVO.CVV,
				DiscountAmount = checkoutHeaderVO.DiscountAmount,
				Email = checkoutHeaderVO.Email,
				ExpiryMonthYear = checkoutHeaderVO.ExpiryMonthYear,
				OrderTime = DateTime.Now.ToUniversalTime(),
				PurchaseAmount = checkoutHeaderVO.PurchaseAmount,
				PaymentStatus = false,
				Phone = checkoutHeaderVO.Phone,
				PurchaseDate = checkoutHeaderVO.DateTime
			};

			foreach (var details in checkoutHeaderVO.CartDetails)
			{
				OrderDetail detail = new()
				{
					ProductId = details.ProductId,
					ProductName = details.Product.Name,
					Price = details.Product.Price,
					Count = details.Count,
				};
				orderHeader.OrderTotalItens += details.Count;

				orderHeader.OrderDetails.Add(detail);
			}

			await _repository.AddOrder(orderHeader);
		}
	}
}
