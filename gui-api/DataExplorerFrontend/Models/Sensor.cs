using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataExplorerFrontend.Models
{
    public class Sensor
    {
        public int sensorId { get;  set; }
        public string sensorName { get;  set; }
        public int value { get;  set; }
        public DateTime dateTime { get;  set; }
        public bool isChecked { get; set; }
    }
}
