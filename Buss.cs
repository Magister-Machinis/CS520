using System;
using System.Collections.Generic;
using System.Linq;
namespace SimulationCore
{
    class Buss //generic holder class for the busses along with a stack of riders
    {


        List<Rider> passenger;
        public Rider getPassenger() //views top of rider stack
        {
            return passenger[0];
        }

        public Rider popPassenger(int adder)
        {
            Rider toppass = passenger[1];
            passenger.RemoveAt(1);
            return toppass;
        }

        public void stackPassenger(Rider newpass)
        {
            passenger.Add(newpass);
        }
    }
}
 