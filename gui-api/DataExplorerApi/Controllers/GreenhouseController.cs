using DataExplorerApi.Model;
using DataExplorerApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataExplorerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GreenhouseController : ControllerBase
    {

        private readonly GreenhouseService _service;

        private readonly ILogger<GreenhouseController> _logger;
        public IMongoDatabase database { get; }
      
        public GreenhouseController(ILogger<GreenhouseController> logger, IGreenhouseDBSettings settings, GreenhouseService service)
        {
            var client = new MongoClient(settings.ConnectionAddress);
            database = client.GetDatabase(settings.DatabaseName);
            _logger = logger;
            _service = service;
        }

        [HttpGet]       
        public ActionResult<List<Message>> Get()
        {
            var data = database.GetCollection<Message>("air_temp");
            var result = _service.Get("air_temp");
            _logger.LogDebug(result[0].ToString());
            return result;

        }

        [HttpGet("{sensorType}/{sensorId}", Name = "GetSensor")]        
        public ActionResult<List<Message>> Get(string sensorType, string sensorId)
        {
            _logger.LogDebug("Seek for messages on sensor type=" + sensorType + ", on sensor id=" + sensorId);

            var result = _service.Get(sensorType, sensorId);
            

            return result;

        }
    }
}
