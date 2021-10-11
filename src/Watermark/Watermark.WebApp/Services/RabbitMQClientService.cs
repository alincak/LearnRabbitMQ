using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;

namespace Watermark.WebApp.Services
{
  public class RabbitMQClientService : IDisposable
  {
    private readonly ILogger<RabbitMQClientService> _logger;

    private readonly ConnectionFactory _connectionFactory;
    private IConnection _connection;
    private IModel _channel;

    public const string ExchangeName = "ImageDirectExchange";
    public const string RoutingWatermark = "watermark-route-image";
    public const string QueueName = "queue-watermark-image";

    public RabbitMQClientService(ILogger<RabbitMQClientService> logger, ConnectionFactory connectionFactory)
    {
      _logger = logger;
      _connectionFactory = connectionFactory;
    }

    public IModel Connect()
    {
      _connection = _connectionFactory.CreateConnection();

      if (_channel is { IsOpen: true })
      {
        return _channel;
      }

      _channel = _connection.CreateModel();
      _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, true, false);
      _channel.QueueDeclare(QueueName, true, false, false, null);

      _channel.QueueBind(QueueName, ExchangeName, RoutingWatermark);

      _logger.LogInformation("RabbitMQ ile bağlantı kuruldu.");

      return _channel;
    }

    public void Dispose()
    {
      _channel?.Close();
      _channel?.Dispose();

      _connection?.Close();
      _connection?.Dispose();

      _logger.LogInformation("RabbitMQ ile bağlantı koptu.");
    }

  }
}
