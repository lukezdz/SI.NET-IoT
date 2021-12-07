using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataExplorerFrontend.Models;
using DataExplorerFrontend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace DataExplorerFrontend.Pages
{
    public class AirHumidityModel : PageModel
    {
        public string sensorName;
        public string PageTitle { get; set; }
        public IEnumerable<Sensor> Sensors { get; set; }
        private readonly ILogger<AirHumidityModel> _logger;
        private readonly SensorRepository sensorRepository;
        private readonly string volumePath = "/mnt/volume";

        public AirHumidityModel(ILogger<AirHumidityModel> logger, SensorRepository sensorRepository)
        {
            _logger = logger;
            this.sensorRepository = sensorRepository;
        }
        public void OnPostCsv(string sensorName)
        {
            try
            {
                this.sensorName = sensorName;
                PageTitle = SensorRepository.MapPageTitle(sensorName);
                Sensors = sensorRepository.GetAllSensors();
                StreamWriter sw = new StreamWriter(volumePath + "/" + sensorName + ".csv", false);
                //headers    
                sw.Write("sensorID,sensorName,value,dateTime");
                sw.Write(sw.NewLine);

                foreach (Sensor sensor in Sensors)
                {
                    sw.Write(sensor.sensorId);
                    sw.Write(",");
                    sw.Write(sensor.sensorName);
                    sw.Write(",");
                    sw.Write(sensor.value);
                    sw.Write(",");
                    sw.Write(sensor.dateTime);
                    sw.Write(sw.NewLine);
                }
                sw.Close();
                ViewData["Message"] = string.Format("CSV file succefully generated!");
            }
            catch
            {
                ViewData["Message"] = string.Format("There was an error during CSV generating!");
            }
            
        }


        public async Task OnGetAsync(string sensorName)
        {
            this.sensorName = sensorName;
            PageTitle = SensorRepository.MapPageTitle(sensorName);

            using (var client = new System.Net.Http.HttpClient())
            {
                var request = new System.Net.Http.HttpRequestMessage();
                request.RequestUri = new Uri("http://data_explorer_api:80/api/" + sensorName + "?page=1&pageSize=1000" );
                  // localhost:5000/api

                var response = await client.SendAsync(request);
                string responseContentInString = await response.Content.ReadAsStringAsync();
                JObject responseContentInJSON = (JObject)JObject.Parse(responseContentInString);
                loadNewData(responseContentInJSON);
                
            }
        }
        private void loadNewData(JObject jObject)
        {
            sensorRepository.ClearSensors();
            var tmpSensorList = SensorRepository.mapJsonToSensor(sensorName, jObject);
            foreach(Sensor sensor in tmpSensorList)
            {
                sensorRepository.AddSensor(sensor);
            }
            Sensors = sensorRepository.GetAllSensors();              
        }

        
    }
}
