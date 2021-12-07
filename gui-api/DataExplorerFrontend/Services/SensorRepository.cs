using DataExplorerFrontend.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataExplorerFrontend.Services
{
    public class SensorRepository
    {

        private List<Sensor> sensors;

        public SensorRepository()
        {
            sensors = new List<Sensor>();
        }

        public void AddSensor(Sensor sensor)
        {
            this.sensors.Add(sensor);
        }

        public void ClearSensors()
        {
            this.sensors.Clear();
        }

        public IEnumerable<Sensor> GetAllSensors()
        {
            return sensors;
        }

        public IEnumerable<Sensor> GetSensorWithID(int id)
        {
            return sensors.Where(sensor => sensor.sensorId.Equals(id));
        }

        public static IList<Sensor> mapJsonToSensor(string sensorName, JObject jObject)
        {
            JArray items = (JArray)jObject["items"];
            IList<Sensor> sensorsList = items.ToObject<IList<Sensor>>();
            foreach (Sensor sensor in sensorsList)
            {
                sensor.sensorName = sensorName;
            }
            return sensorsList;
        }

        public static string MapPageTitle(string sensorName)
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
