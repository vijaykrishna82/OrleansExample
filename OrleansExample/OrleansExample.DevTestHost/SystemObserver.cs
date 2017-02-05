using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrleansExample.GrainInterfaces;

namespace OrleansExample.DevTestHost
{
    public class SystemObserver : ISystemObserver
    {
        public void HighTemperature(double currentAverageTemperature)
        {

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Observed a High Temperature: {0}", currentAverageTemperature);
            Console.ResetColor();
        }
    }
}
