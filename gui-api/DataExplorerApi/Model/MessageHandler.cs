using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DataExplorerApi.Model
{
    public class MessageHandler : IRequestHandler<Message>
    {
        private readonly ILogger<MessageHandler> _logger;
        public IMongoDatabase database { get; }
             

        public MessageHandler(ILogger<MessageHandler> logger, IGreenhouseDBSettings settings)
        { 
            _logger = logger;

        
            var client = new MongoClient(settings.ConnectionAddress);
            database = client.GetDatabase(settings.DatabaseName);

        }

        public Task<Unit> Handle(Message request, CancellationToken cancellationToken)
        {

            _logger.LogInformation("---- Received message -> id {id} : sensorID {sensor} : sensorType {sensorType} : value {value} : dateTime {dateTime} ----", request._id, request.sensorId, request.sensorType, request.value, request.dateTime);

            _ = putRequestInDBAsync(request);
            
            return Task.FromResult(Unit.Value);
        }

        private async Task putRequestInDBAsync(Message request)
        {      
                        
            var collection = database.GetCollection<Message>(request.sensorType);
            await collection.InsertOneAsync(request);

        }

    }

}
