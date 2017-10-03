
using System.Collections.Generic;

namespace SimulationCore
{
  
    class BussStop // generic class for the buss stops with queues for waiting passengers and waiting busses
    {
        List<Rider> waitingQueue;
        List<Buss> waitingRides;

        public BussStop()
        {
            waitingQueue = new List<Rider>();
            waitingRides = new List<Buss>();
        }

        public void addBuss(Buss newbuss)
        {
            waitingRides.Add(newbuss);
        }

        public Buss getBuss()
        {
            return waitingRides[0];
        }

        public Buss popBuss()
        {
            Buss tempbuss = waitingRides[0];
            waitingRides.RemoveAt(0);
            return tempbuss;
        }

        public Rider getFront()
        {
            return waitingQueue[0];
        }
        public Rider popFront()
        {
            Rider nextup = waitingQueue[0];
            waitingQueue.RemoveAt(0);
            return nextup;
        }

        public void addWaiter(Rider newguy)
        {
            waitingQueue.Add(newguy);
        }

        public int getBussNum()
        {
            return waitingRides.Count;
        }

        public int getPassNum()
        {
            return waitingQueue.Count;
        }
    }
}
 