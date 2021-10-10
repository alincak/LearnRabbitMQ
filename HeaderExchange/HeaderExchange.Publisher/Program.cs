using RabbitMQ.Client;
using Shared.RabbitMQ;
using Shared.RabbitMQ.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

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

      var product = new Product { Id = 1, Name = "Kalem", Price = 100, Stock = 10 };
      var productJson = JsonSerializer.Serialize(product);

      channel.BasicPublish("header-exchange", "", properties, Encoding.UTF8.GetBytes(productJson));

      Console.WriteLine("Mesaj gönderilmiştir.");
      Console.ReadLine();
    }

  }
}
