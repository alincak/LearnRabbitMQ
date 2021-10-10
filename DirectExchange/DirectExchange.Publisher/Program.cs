using RabbitMQ.Client;
using Shared.RabbitMQ;
using System;
using System.Linq;
using System.Text;

namespace DirectExchange.Publisher
{
  class Program
  {
    public enum LogNames
    { 
      Critical = 1,
      Error = 2,
      Warning = 3,
      Info = 4
    }

    static void Main(string[] args)
    {
      var channel = RabbitMQUtil.GetChannel();

      channel.ExchangeDeclare("logs-direct", ExchangeType.Direct, true);

      Publish(channel);

      Console.ReadLine();
    }

    static void Publish(IModel channel)
    {
      Enum.GetNames(typeof(LogNames)).ToList().ForEach(x =>
      {
        var routeKey = $"route-{x}";
        var queueName = $"direct-queue-{x}";
        channel.QueueDeclare(queueName, true, false, false);

        channel.QueueBind(queueName, "logs-direct", routeKey, null);
      });

      Enumerable.Range(1, 50).ToList().ForEach(x =>
      {
        var log = (LogNames)new Random().Next(1, 5);

        var message = $"log-type: {log}";

        var messageBody = Encoding.UTF8.GetBytes(message);

        var routeKey = $"route-{log}";

        channel.BasicPublish("logs-direct", routeKey, null, messageBody);

        Console.WriteLine($"Log gönderilmiştir: {message}");
      });
    }
  }
}
