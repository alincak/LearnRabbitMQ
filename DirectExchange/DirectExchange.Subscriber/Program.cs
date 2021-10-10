using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.RabbitMQ;
using System;
using System.IO;
using System.Text;
using System.Threading;

namespace DirectExchange.Subscriber
{
  class Program
  {
    static void Main(string[] args)
    {
      var channel = RabbitMQUtil.GetChannel();

      channel.BasicQos(0, 1, false);

      var consumer = new EventingBasicConsumer(channel);

      var queueName = "direct-queue-Critical";

      channel.BasicConsume(queueName, false, consumer);

      Console.WriteLine("Log dinleniyor...");

      consumer.Received += (object sender, BasicDeliverEventArgs e) =>
      {
        var message = Encoding.UTF8.GetString(e.Body.ToArray());
        Console.WriteLine(message);

        Thread.Sleep(1000);

        //File.AppendAllText("log-critical.txt", message + "\n");

        channel.BasicAck(e.DeliveryTag, false);
      };

      Console.ReadLine();
    }
  }
}
