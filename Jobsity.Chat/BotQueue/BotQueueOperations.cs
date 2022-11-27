using Jobsity.Chat.Domain.Security;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Jobsity.Chat.BotQueue
{
    public class BotQueueOperations
    {
        private QueueConfigurations queueConfigurations { get; set; }

        public BotQueueOperations(QueueConfigurations queueConfigurations)
        {
            this.queueConfigurations = queueConfigurations;
        }

        public event EventHandler<string>? MessageReceived;

        public void PushMessageToQueue(string message)
        {
            ConnectionFactory connectionFactory = new ConnectionFactory() { HostName = queueConfigurations.HostName, UserName = queueConfigurations.UserName, Password = queueConfigurations.Password };
            using (IConnection connection = connectionFactory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: queueConfigurations.SubmissionQueueName,
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var messageBody = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "", routingKey: queueConfigurations.SubmissionQueueName, body: messageBody, basicProperties: null);
                }
            }
        }

        public void ReceiveMessageFromQueue()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory() { HostName = queueConfigurations.HostName, UserName = queueConfigurations.UserName, Password = queueConfigurations.Password };
            IConnection connection = connectionFactory.CreateConnection();
            IModel channel = connection.CreateModel();

            channel.QueueDeclare(queue: queueConfigurations.ReceivingQueueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);

                if (!string.IsNullOrWhiteSpace(message))
                {
                    MessageReceived?.Invoke(this, message);
                }
                channel.BasicAck(ea.DeliveryTag, false);
            };

            channel.BasicConsume(queue: queueConfigurations.ReceivingQueueName,
                                 autoAck: false,
                                 consumer: consumer);
        }
    }
}
