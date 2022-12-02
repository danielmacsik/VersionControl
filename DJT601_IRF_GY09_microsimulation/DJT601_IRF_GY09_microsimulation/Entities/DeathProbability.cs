using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJT601_IRF_GY09_microsimulation.Entities
{
    class DeathProbability
    {
        public Gender Gender { get; set; }
        public int Age { get; set; }
        public double DthProbability { get; set; }
    }
}
