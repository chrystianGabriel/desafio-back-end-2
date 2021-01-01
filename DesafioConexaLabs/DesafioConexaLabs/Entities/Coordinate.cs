using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioConexaLabs.Entities
{
    public class Coordinate
    {
        public string Latitude { get; }
        public string Longitude { get; }

        public Coordinate(string latitude, string longitude)
        {
            this.Latitude = latitude.Replace(",",".");
            this.Longitude = longitude.Replace(",", ".");
        }

        public bool IsValid()
        {
            if (float.TryParse(Latitude, out _) && float.TryParse(Longitude, out _))
            {
                return true;
            }
            return false;
        }

    }
}
