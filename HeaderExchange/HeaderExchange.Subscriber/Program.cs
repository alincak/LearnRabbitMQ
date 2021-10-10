using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.RabbitMQ;
using Shared.RabbitMQ.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace HeaderExchange.Subscriber
{
  class Program
  {
    static void Main(string[] args)
    {
      var channel = RabbitMQUtil.GetChannel();

      channel.BasicQos(0, 1, false);

      var consumer = new EventingBasicConsumer(channel);

      var queueName = channel.QueueDeclare().QueueName;

      var headers = new Dictionary<string, object>
      {
        { "format", "pdf" },
        { "shape", "a4" },
        //{ "x-match", "all" }
        { "x-match", "any" }
      };

      channel.QueueBind(queueName, "header-exchange", string.Empty, headers);

      channel.BasicConsume(queueName, false, consumer);

      Console.WriteLine("Log dinleniyor...");

      consumer.Received += (object sender, BasicDeliverEventArgs e) =>
      {
        var json = Encoding.UTF8.GetString(e.Body.ToArray());
        var product = JsonSerializer.Deserialize<Product>(json);

        Console.WriteLine($"Gelen mesaj: {product.Id}, {product.Name}, {product.Price}, {product.Stock}");

        Thread.Sleep(1000);

        channel.BasicAck(e.DeliveryTag, false);
      };

      Console.ReadLine();
    }
  }
}
