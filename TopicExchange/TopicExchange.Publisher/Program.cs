using RabbitMQ.Client;
using Shared.RabbitMQ;
using System;
using System.Linq;
using System.Text;

namespace TopicExchange.Publisher
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

      channel.ExchangeDeclare("logs-topic", ExchangeType.Topic, true);

      Publish(channel);

      Console.ReadLine();
    }

    static void Publish(IModel channel)
    {
      var rnd = new Random();
      Enumerable.Range(1, 50).ToList().ForEach(x =>
      {
        var log1 = (LogNames)rnd.Next(1, 5);
        var log2 = (LogNames)rnd.Next(1, 5);
        var log3 = (LogNames)rnd.Next(1, 5);

        var routeKey = $"{log1}.{log2}.{log3}";

        var message = $"log-type: {routeKey}";
        var messageBody = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish("logs-topic", routeKey, null, messageBody);

        Console.WriteLine($"Log gönderilmiştir: {message}");
      });
    }

  }
}
