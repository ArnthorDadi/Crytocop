using System;
using System.Text;
using System.Text.Json;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class QueueService : IQueueService, IDisposable
    {
        private IModel _channel;
        private IConnection _connection;
        private readonly string exchange = "Cryptocop";
        private readonly string queue = "Cryptocop-Queue";

        public QueueService(IConfiguration configuration)
        {
            try
            {
                var factory = new ConnectionFactory();
                factory.Uri = new Uri("amqps://yqrbpfxq:DstEdXfIUOCHv2GzlvSbLRhbiym8RVTt@bonobo.rmq.cloudamqp.com/yqrbpfxq");
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // • QueueService.cs (2%)
        //      • PublishMessage
        //      • Serialize the object to JSON
        //      • Publish the message using a channel created with the RabbitMQ client
        public void PublishMessage(string routingKey, object body)
        {
            _channel.QueueDeclare(queue, true, false, false, null);
            _channel.QueueBind(queue, exchange, routingKey, null);
            _channel.ExchangeDeclare(exchange, ExchangeType.Direct);
            _channel.BasicPublish(
                exchange: exchange,
                routingKey: routingKey,
                mandatory: true,
                basicProperties: null,
                body: ConvertJsonToBytes(body));
        }

        public void Dispose()
        {
            // TODO: Dispose the connection and channel
            _channel.Close();
            _connection.Close();
        }
        private byte[] ConvertJsonToBytes(object obj) => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));
    }
}