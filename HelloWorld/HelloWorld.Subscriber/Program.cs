using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace HelloWorld.Subscriber
{
  class Program
  {
    static void Main(string[] args)
    {
      ConnectionFactory factory = new ConnectionFactory();
      factory.UserName = "guest";
      factory.Password = "guest";

      var endpoints = new System.Collections.Generic.List<AmqpTcpEndpoint> {
                              new AmqpTcpEndpoint("localhost")
                            };

      using var connection = factory.CreateConnection(endpoints);

      var channel = connection.CreateModel();

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
