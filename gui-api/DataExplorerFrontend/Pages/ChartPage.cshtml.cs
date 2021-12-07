using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DataExplorerFrontend.Models;
using DataExplorerFrontend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DataExplorerFrontend.Pages
{
    public class ChartPageModel : PageModel
    {
        public int sensorId;
        public string sensorName;
        public string PageTitle { get; set; }
        public IEnumerable<Sensor> Sensors { get; set; }
        public List<Tuple<int,DateTime>> filteredSensors;
        private readonly SensorRepository sensorRepository;

        public ChartPageModel(SensorRepository sensorRepository)
        {
            this.sensorRepository = sensorRepository;
        }
        public void OnPost(string sensorName, int sensorId)
        {
            this.sensorId = sensorId;
            this.sensorName = sensorName;
            PageTitle = MapPageTitle(sensorName);
            Sensors = sensorRepository.GetSensorWithID(sensorId);
            filteredSensors = new List<Tuple<int, DateTime>>();
            foreach (var sensor in Sensors)
            {
                Tuple<int, DateTime> tmpTuple = new Tuple<int, DateTime>(sensor.value, sensor.dateTime);
                filteredSensors.Add(tmpTuple);
            }
        }

        public void OnGet(string sensorName)
        {
            this.sensorName = sensorName;
            this.sensorId = 1;
            PageTitle = MapPageTitle(sensorName);
            Sensors = sensorRepository.GetSensorWithID(this.sensorId);
            filteredSensors = new List<Tuple<int, DateTime>>();
            foreach (var sensor in Sensors)
            {
                Tuple<int, DateTime> tmpTuple = new Tuple<int, DateTime>(sensor.value, sensor.dateTime);
                filteredSensors.Add(tmpTuple);
            }

        }

        private static string MapPageTitle(string sensorName)
        {
            switch (sensorName)
            {
                case "air_hum":
                    return "Air Humidity";
                case "sub_hum":
                    return "Substrate Humidity";
                case "sub_temp":
                    return "Substrate Temperature";
                case "air_temp":
                    return "Air Temperature";
                default:
                    return "";
            }
        }
    }
}
