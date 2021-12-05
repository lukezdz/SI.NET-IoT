using DataExplorerApi.Model;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataExplorerApi.Services
{
    public class GreenhouseService
    {

        private readonly IMongoDatabase database;
        private readonly ILogger<GreenhouseService> _logger;


        public GreenhouseService(IGreenhouseDBSettings settings, ILogger<GreenhouseService> logger)
        {
            _logger = logger;
            var client = new MongoClient(settings.ConnectionAddress);
            database = client.GetDatabase(settings.DatabaseName);
        }

        public List<Message> GetAll(string colName)
        {
            IMongoCollection<Message> collection;
            if (IsCollectionNameValid(colName))
            {
                collection = database.GetCollection<Message>(colName);
            }
            else // can choose default collection \/
            {
                return new List<Message>();
            }
            return collection.Find(message => true).ToList();
        }

        public List<Message> Get(string colName, string sensorId)
        {
            IMongoCollection<Message> collection;
            if (IsCollectionNameValid(colName))
            {
                collection = database.GetCollection<Message>(colName);
            }
            else // can choose default collection here \/
            {
                return new List<Message>();
            }
          //  JsonSerializer.Serialize(collection.Find(message => message.sensorId == sensorId).ToList());
            return collection.Find(message => message.sensorId == sensorId).ToList();
        }

        public List<Message> Get(string colName, string sensorId, int page, int pageSize)
        {
            IMongoCollection<Message> collection;
            if (IsCollectionNameValid(colName))
            {
                collection = database.GetCollection<Message>(colName);
            }
            else // can choose default collection here \/
            {
                return new List<Message>();
            }     

            var message_list = new List<Message>();
            var pageList = QueryByPage(collection, sensorId, page, pageSize);
            foreach (var msg in pageList.Result.readOnlyList)
            {
                message_list.Add(msg);
            }

            return message_list;
        }

        private bool IsCollectionNameValid(string colName)
        {
            if (colName == SensorType.AIR_HUM || colName == SensorType.AIR_TEMP || colName == SensorType.SUB_HUM || colName == SensorType.SUB_TEMP)
                return true;
            return false;
        }
       
        private static async Task<(int totalPages, IReadOnlyList<Message> readOnlyList)> QueryByPage(IMongoCollection<Message> collection, string sensorId, int page, int pageSize)
        {
            var countFacet = AggregateFacet.Create("count",
                PipelineDefinition<Message, AggregateCountResult>.Create(new[]
                {
                    PipelineStageDefinitionBuilder.Count<Message>()
                }));

            var dataFacet = AggregateFacet.Create("data",
                PipelineDefinition<Message, Message>.Create(new[]
                {
                    PipelineStageDefinitionBuilder.Sort(Builders<Message>.Sort.Ascending(x => x.dateTime)),
                    PipelineStageDefinitionBuilder.Skip<Message>((page - 1) * pageSize),
                    PipelineStageDefinitionBuilder.Limit<Message>(pageSize),
                }));

            var filter = Builders<Message>.Filter.Eq(x => x.sensorId, sensorId);

            var aggregation = await collection.Aggregate()
                .Match(filter)
                .Facet(countFacet, dataFacet)
                .ToListAsync();

            var count = aggregation.First()
                .Facets.First(x => x.Name == "count")
                .Output<AggregateCountResult>()
                ?.FirstOrDefault()
                ?.Count ?? 0;

            var totalPages = (int)count / pageSize;

            var data = aggregation.First()
                .Facets.First(x => x.Name == "data")
                .Output<Message>();

            return (totalPages, data);
        }
    }

    public static class SensorType
    {
        public const string AIR_HUM = "air_hum";
        public const string AIR_TEMP = "air_temp";
        public const string SUB_HUM = "sub_hum";
        public const string SUB_TEMP = "sub_temp";
        public const string DEFAULT = AIR_HUM;
    }
}
