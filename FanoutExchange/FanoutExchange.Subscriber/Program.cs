using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.RabbitMQ;
using System;
using System.Text;
using System.Threading;

namespace FanoutExchange.Subscriber
{
  class Program
  {
    static void Main(string[] args)
    {
      var channel = RabbitMQUtil.GetChannel();

      var randomQueueName = channel.QueueDeclare().QueueName;

      /////Kuyruk kalıcı olsun istiyorsak...
      //var randomQueueName = "log-database-save-queue";
      //channel.QueueDeclare(randomQueueName, true, false, false);

      channel.QueueBind(randomQueueName, "logs-fanout", "", null);
      channel.BasicQos(0, 1, false);

      var consumer = new EventingBasicConsumer(channel);

      channel.BasicConsume(randomQueueName, false, consumer);

      Console.WriteLine("Log dinleniyor...");

      consumer.Received += (object sender, BasicDeliverEventArgs e) =>
      {
        var message = Encoding.UTF8.GetString(e.Body.ToArray());
        Console.WriteLine(message);

        Thread.Sleep(1000);

        channel.BasicAck(e.DeliveryTag, false);
      };

      Console.ReadLine();
    }
  }
}
