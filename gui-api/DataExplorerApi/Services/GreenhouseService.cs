using DataExplorerApi.Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataExplorerApi.Services
{
    public class GreenhouseService
    {

        private readonly IMongoDatabase database;


        public GreenhouseService(IGreenhouseDBSettings settings)
        {
            var client = new MongoClient(settings.ConnectionAddress);
            database = client.GetDatabase(settings.DatabaseName);            
        }

        public List<Message> Get(string colName)
        {
            var collection = database.GetCollection<Message>(colName);

            return collection.Find(message => true).ToList();
        }

        public List<Message> Get(string colName, string sensorId)
        {
            IMongoCollection<Message> collection;
            if (IsCollectionNameValid(colName))
            {
               collection = database.GetCollection<Message>(colName);
            } else
            {
                collection = database.GetCollection<Message>(SensorType.DEFAULT);
            }      
            return collection.Find(message => message.sensorId == sensorId).ToList();
        }

        private bool IsCollectionNameValid(string colName)
        {
            if (colName == SensorType.AIR_HUM || colName == SensorType.AIR_TEMP || colName == SensorType.SUB_HUM || colName == SensorType.SUB_TEMP)
                return true;
            return false;
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
