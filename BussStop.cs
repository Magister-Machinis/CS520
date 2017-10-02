
using System.Collections.Generic;

namespace SimulationCore
{
  
    class BussStop // generic class for the buss stops
    {
        List<Rider> waitingQueue;

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


    }
}
 