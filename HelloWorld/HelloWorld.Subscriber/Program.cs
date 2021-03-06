using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.RabbitMQ;
using System;
using System.Text;
using System.Threading;

namespace HelloWorld.Subscriber
{
  class Program
  {
    static void Main(string[] args)
    {
      var channel = RabbitMQUtil.GetChannel();

      //channel.QueueDeclare("hello-queue", false, false, false);

      channel.BasicQos(0, 1, false);

      var consumer = new EventingBasicConsumer(channel);

      channel.BasicConsume("hello-queue", false, consumer);

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
