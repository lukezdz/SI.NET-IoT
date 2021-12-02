using MediatR;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataExplorerApi.Model
{
    public class Message : IRequest<Unit>
    {
        [BsonId]      
        public ObjectId _id { get; set; }

        [BsonElement("sensor_id")]
        public String sensorId { get; set; }

        [BsonElement("sensor_type")]
        public String sensorType { get; set; }

        [BsonElement("value")]
        public String value { get; set; }

        [BsonElement("dateTime")]
        public String dateTime { get; set; }
    }
}
