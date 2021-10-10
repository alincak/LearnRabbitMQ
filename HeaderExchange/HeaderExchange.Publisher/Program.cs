using RabbitMQ.Client;
using Shared.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeaderExchange.Publisher
{
  class Program
  {
    static void Main(string[] args)
    {
      var channel = RabbitMQUtil.GetChannel();

      channel.ExchangeDeclare("header-exchange", ExchangeType.Headers, true);

      var headers = new Dictionary<string, object>
      {
        { "format", "pdf" },
        { "shape2", "a4" }
      };

      var properties = channel.CreateBasicProperties();
      properties.Headers = headers;
      //properties.Persistent = true; mesajların kalıcı olması için

      channel.BasicPublish("header-exchange", "", properties, Encoding.UTF8.GetBytes("Header mesajım."));

      Console.WriteLine("Mesaj gönderilmiştir.");
      Console.ReadLine();
    }

  }
}
