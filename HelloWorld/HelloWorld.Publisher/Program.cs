using RabbitMQ.Client;
using Shared.RabbitMQ;
using System;
using System.Linq;
using System.Text;

namespace HelloWorld.Publisher
{
  class Program
  {
    static void Main(string[] args)
    {
      var channel = RabbitMQUtil.GetChannel();

      channel.QueueDeclare("hello-queue", false, false, false);

      //HelloWord(channel);
      WorkQueue(channel);

      Console.ReadLine();
    }

    static void HelloWord(IModel channel)
    {
      var message = "hello world";

      var messageBody = Encoding.UTF8.GetBytes(message);

      channel.BasicPublish(string.Empty, "hello-queue", null, messageBody);

      Console.WriteLine("Mesajınız gönderilmiştir.");
    }

    static void WorkQueue(IModel channel)
    {
      Enumerable.Range(1, 50).ToList().ForEach(x =>
      {
        var message = $"Message {x}";

        var messageBody = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(string.Empty, "hello-queue", null, messageBody);

        Console.WriteLine($"Mesajınız gönderilmiştir: {message}");
      });
    }

  }
}
