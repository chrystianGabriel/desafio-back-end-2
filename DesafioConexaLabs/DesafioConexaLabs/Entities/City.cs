using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioConexaLabs.Entities
{
    public class City
    {
        public string Name { get; set; }
        public float Temperature { get; set; }

        public City(string name, float temperature)
        {
            this.Name = name;
            this.Temperature = temperature;
        }
    }
}
