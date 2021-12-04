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
    [Route("/api")]
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

        [HttpGet("{sensorType}")]
        public ActionResult<List<Message>> Get(string sensorType)
        {
            var result = _service.GetAll(sensorType);

            if (result.Count > 0)
                return result;
      
            return NotFound();

        }

        [HttpGet("{sensorType}/{sensorId}")]
        public ActionResult<List<Message>> Get([FromQuery] String page, [FromQuery] String pageSize, string sensorType, string sensorId)
        {
            if (sensorType == null && sensorId == null)
                return NotFound();

            if(page == null && pageSize == null) {
                _logger.LogDebug("Seek for messages on sensor type=" + sensorType + ", on sensor id=" + sensorId);
                var result = _service.Get(sensorType, sensorId);
                if (result.Count() > 0)
                    return result;
            }
            else if (page != null && page.Length > 0  && pageSize != null && pageSize.Length > 0) {
                _logger.LogDebug("Seek for messages on sensor type=" + sensorType + ", on sensor id=" + sensorId + ", pagination: page=" + page + ", pageSize=" + pageSize);

                int _page;
                int _pageSize;

                try  {
                    _page = int.Parse(page);
                    _pageSize = int.Parse(pageSize);
                } catch (FormatException) {
                    _logger.LogDebug("INCORRECT DATA, page and pageSize should be int! Were page=" + page + ", pageSize=" + pageSize);
                    return BadRequest();                
                }
                var result = _service.Get(sensorType, sensorId, _page, _pageSize);
               
                if (result.Count > 0)
                    return result;
            } else {
                return BadRequest();
            }           

            return NotFound();

        }
    }
}
