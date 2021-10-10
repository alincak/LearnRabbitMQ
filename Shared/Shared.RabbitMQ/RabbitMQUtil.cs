using RabbitMQ.Client;

namespace Shared.RabbitMQ
{
  public class RabbitMQUtil
  {
    public static IModel GetChannel()
    {
      ConnectionFactory factory = new()
      {
        UserName = "guest",
        Password = "guest"
      };

      var endpoints = new System.Collections.Generic.List<AmqpTcpEndpoint> {
                              new AmqpTcpEndpoint("localhost")
                            };

      var connection = factory.CreateConnection(endpoints);

      return connection.CreateModel();
    }
  }
}
