using CsvHelper;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Queue.Protocol;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Jobsity.Chat.Bot
{
    public class GetExternalStockQuote
    {
        [FunctionName("GetExternalStockQuote")]
        public async Task Run([RabbitMQTrigger("SendingCommandToBot", ConnectionStringSetting = "RabbitMQConnectionString")] string message, ILogger log)
        {
            if (string.IsNullOrWhiteSpace(message)) return;
            
            try
            {
                var StooqURL = Environment.GetEnvironmentVariable("StooqUrl");
                var StooqConstant = Environment.GetEnvironmentVariable("StooqConstant");

                var url = StooqURL.Replace(StooqConstant, message);
                var messageInterpreted = false;

                HttpClient client = new HttpClient();

                using (var msg = new HttpRequestMessage(HttpMethod.Get, new Uri(url)))
                {
                    msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/csv"));
                    using (var resp = await client.SendAsync(msg))
                    {
                        resp.EnsureSuccessStatusCode();

                        using (var stream = await resp.Content.ReadAsStreamAsync())
                        using (var streamReader = new StreamReader(stream))
                        using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                        {
                            var records = csvReader.GetRecords<StooqCsv>().ToArray();
                            if (records.Any())
                            {
                                var firstRecord = records.FirstOrDefault();
                                var NoInformation = Environment.GetEnvironmentVariable("StooqNoInformationIdentifier");
                                if (firstRecord.Date != NoInformation)
                                {
                                    messageInterpreted = SendInterpretedMessageToQueue(firstRecord, log);
                                }
                            }
                        }
                    }
                }

                if (!messageInterpreted)
                {
                    SendUninterpretedMessageToQueue(message, log);
                }
            }
            catch (Exception ex)
            {
                SendErrorToQueue(ex, message, log);
            }
        }

        private bool SendInterpretedMessageToQueue(StooqCsv info, ILogger log)
        {
            var queueName = Environment.GetEnvironmentVariable("RabbitMQInterpretedMessagesQueueName");
            var resultMessage = $"{info.Symbol} quote is ${info.Open} per share.";
            var result = PushMessageToQueue(resultMessage, queueName);
            log.LogInformation(info.Symbol, info);
            return result;
        }

        private void SendUninterpretedMessageToQueue(string uninterpretedMessage, ILogger log)
        {
            var queueName = Environment.GetEnvironmentVariable("RabbitMQUninterpretedMessagesQueueName");
            PushMessageToQueue(uninterpretedMessage, queueName);
            log.LogWarning($"Uninterpreted message: {uninterpretedMessage}");
        }

        private void SendErrorToQueue(Exception err, string message, ILogger log)
        {
            log.LogError(err, message);
            var botException = new BotException(err, message);
            var errMessage = JsonConvert.SerializeObject(botException);
            var queueName = Environment.GetEnvironmentVariable("RabbitMQErrorQueueName");
            PushMessageToQueue(errMessage, queueName);
        }

        private bool PushMessageToQueue(string message, string queueName)
        {
            var hostName = Environment.GetEnvironmentVariable("RabbitMQHost");
            var userName = Environment.GetEnvironmentVariable("RabbitMQUser");
            var password = Environment.GetEnvironmentVariable("RabbitMQPassword");

            ConnectionFactory connectionFactory = new ConnectionFactory() { HostName = hostName, UserName = userName, Password = password };
            using (IConnection connection = connectionFactory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: queueName,
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var messageBody = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "", routingKey: queueName, body: messageBody, basicProperties: null);
                }
            }

            return true;
        }
    }
}
