using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioConexaLabs.Entities
{
    public class Track
    {
        public string Name { get; private set; }

        public Track(string songName)
        {
            this.Name = songName;
        }
    }
}
