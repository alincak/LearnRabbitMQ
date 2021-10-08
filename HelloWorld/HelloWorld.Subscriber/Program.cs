using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

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

      var consumer = new EventingBasicConsumer(channel);

      channel.BasicConsume("hello-queue", true, consumer);

      consumer.Received += (object sender, BasicDeliverEventArgs e) =>
      {
        var message = Encoding.UTF8.GetString(e.Body.ToArray());
        Console.WriteLine(message);
      };

      Console.ReadLine();
    }
  }
}
