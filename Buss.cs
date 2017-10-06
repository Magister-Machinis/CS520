using System;
using System.Collections.Generic;
using System.Linq;
using GenericTools;
namespace SimulationCore
{
    class Buss //generic holder class for the busses along with a stack of riders
    {

        BussRoute.RouteWrapper currentstop;
        List<Rider> passenger;
        int Bussnum;
        Toolkit Tools = new Toolkit();

        public Buss()
        {
            passenger = new List<Rider>();
            Bussnum = Tools.ReallyRandom();
        }
        public BussRoute.RouteWrapper getStop()
        {
            return currentstop;
        }

        public void setStop(BussRoute.RouteWrapper newstop)
        {
            currentstop = newstop;
        }
        public Rider getPassenger() //views top of rider stack
        {
            return passenger[0];
        }

        public Rider popPassenger(int adder)
        {
            Rider toppass = passenger[0];
            passenger.RemoveAt(0);
            return toppass;
        }

        public void stackPassenger(Rider newpass)
        {
            passenger.Add(newpass);
        }
    }
}
 