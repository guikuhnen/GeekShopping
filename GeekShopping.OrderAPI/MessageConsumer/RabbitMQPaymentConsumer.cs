﻿using GeekShopping.MessageBus;
using GeekShopping.OrderAPI.Messages;
using GeekShopping.OrderAPI.Models;
using GeekShopping.OrderAPI.RabbitMQSender;
using GeekShopping.OrderAPI.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading.RateLimiting;
using System.Xml.Linq;

namespace GeekShopping.OrderAPI.MessageConsumer
{
	public class RabbitMQPaymentConsumer : BackgroundService
	{
		private readonly OrderRepository _repository;
		private readonly IConfiguration _config;
		private IConnection _connection;
		private IModel _channel;

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

			_connection = factory.CreateConnection();
			_channel = _connection.CreateModel();
			_channel.QueueDeclare(queue: _config.GetValue<string>("RabbitMQ:Queues:OrderPaymentResult"), false, false, false, arguments: null);
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

			_channel.BasicConsume(_config.GetValue<string>("RabbitMQ:Queues:OrderPaymentResult"), false, consumer);

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
