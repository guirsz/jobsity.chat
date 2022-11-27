using Jobsity.Chat.Domain.Security;
using Jobsity.Chat.SignalR;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Jobsity.Chat.BotQueue
{
    public class BotQueueOperations
    {
        private QueueConfigurations queueConfigurations { get; set; }
        private string botName { get; set; }
        private IServiceProvider serviceProvider { get; set; }

        public BotQueueOperations(QueueConfigurations queueConfigurations, IServiceProvider serviceProvider)
        {
            this.queueConfigurations = queueConfigurations;
            this.serviceProvider = serviceProvider;
            botName = $"{RandomString(10)}{Guid.NewGuid()}";
        }

        public bool IsThisUserTheBot(string userName) => userName == botName;

        public void PushMessageToQueue(string message)
        {
            ConnectionFactory connectionFactory = new ConnectionFactory() { HostName = queueConfigurations.HostName };
            using (IConnection connection = connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
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
            var chatHub = serviceProvider.GetService<ChatHub>();
            ConnectionFactory connectionFactory = new ConnectionFactory() { HostName = queueConfigurations.HostName };
            using (IConnection connection = connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: queueConfigurations.ReceivingQueueName,
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    channel.BasicQos(prefetchSize: 0, prefetchCount: 3, global: false);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += async (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);

                        if (!string.IsNullOrWhiteSpace(message))
                            await chatHub.PostMessage(botName, message);

                        channel.BasicAck(ea.DeliveryTag, false);
                    };

                    channel.BasicConsume(queue: queueConfigurations.ReceivingQueueName,
                                         autoAck: false,
                                         consumer: consumer);
                }
            }
        }

        private string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
