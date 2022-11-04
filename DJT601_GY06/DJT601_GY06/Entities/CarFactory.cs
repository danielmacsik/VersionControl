using DJT601_GY06.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJT601_GY06.Entities
{
    public class CarFactory : IToyFactory
    {
        public Toy CreateNew() 
        {
            return new Car();
        }
    }
}
