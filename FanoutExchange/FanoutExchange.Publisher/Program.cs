using RabbitMQ.Client;
using Shared.RabbitMQ;
using System;
using System.Linq;
using System.Text;

namespace FanoutExchange.Publisher
{
  class Program
  {
    static void Main(string[] args)
    {
      var channel = RabbitMQUtil.GetChannel();

      channel.ExchangeDeclare("logs-fanout", ExchangeType.Fanout, false);

      Publish(channel);

      Console.ReadLine();
    }

    static void Publish(IModel channel)
    {
      Enumerable.Range(1, 50).ToList().ForEach(x =>
      {
        var message = $"log {x}";

        var messageBody = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish("logs-fanout", "", null, messageBody);

        Console.WriteLine($"Mesajınız gönderilmiştir: {message}");
      });
    }
  }
}
