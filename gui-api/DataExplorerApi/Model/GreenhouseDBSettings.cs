using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataExplorerApi.Model
{
    public class GreenhouseDBSettings : IGreenhouseDBSettings
    {      
        public string ConnectionAddress { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IGreenhouseDBSettings
    {
        string ConnectionAddress { get; set; }
        string DatabaseName { get; set; }
    }
}
