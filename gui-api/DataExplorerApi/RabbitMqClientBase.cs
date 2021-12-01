using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataExplorerApi
{
    public abstract class RabbitMqClientBase : IDisposable
    {   
       
        protected readonly string LoggerQueue = $"sensors-queue";
  

        protected IModel Channel { get; private set; }
        private IConnection _connection;
        private readonly ConnectionFactory _connectionFactory;
        private readonly ILogger<RabbitMqClientBase> _logger;

        protected RabbitMqClientBase(ConnectionFactory connectionFactory, ILogger<RabbitMqClientBase> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
 
            ConnectToRabbitMq();
        }

        private void ConnectToRabbitMq()
        {
            if (_connection == null || _connection.IsOpen == false)
            {
                _connection = _connectionFactory.CreateConnection();
            }
            
            if (Channel == null || Channel.IsOpen == false)
            {
                _logger.LogDebug("CREATING CHANNEL TO QUEUE");
                Channel = _connection.CreateModel();
                /*  Channel.ExchangeDeclare(
                      exchange: LoggerExchange,
                      type: "direct",
                      durable: true,
                      autoDelete: false);*/            
                Channel.QueueDeclare(
                    queue: LoggerQueue,
                    durable: true,
                    exclusive: false,
                    autoDelete: false);

                /*Channel.QueueBind(
                    queue: LoggerQueue,
                    exchange: LoggerExchange,
                    routingKey: LoggerQueueAndExchangeRoutingKey);*/
          
            }
        }

        public void Dispose()
        {
            _logger.LogDebug("DISPOSE?!");
            try
            {
                Channel?.Close();
                Channel?.Dispose();
                Channel = null;

                _connection?.Close();
                _connection?.Dispose();
                _connection = null;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Cannot dispose RabbitMQ channel or connection");
            }
        }
    }
}
