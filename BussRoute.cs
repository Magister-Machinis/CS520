using System.IO;

namespace SimulationCore
{
    static class BussRoute //class and subclasses for the route stops as a doubly linked list
    {
        public class RouteWrapper
        {
            public BussStop stop = new BussStop();
            int stopnumber = 0;
            RouteWrapper previous;
            RouteWrapper next;

            public void SetPrev (RouteWrapper subject)
            {
                previous = subject;
            }
            public void SetNext(RouteWrapper subject)
            {
                next = subject;
            }
            public void SetNum (int subject)
            {
                stopnumber = subject;
            }



            public RouteWrapper GetPrev ()
            {
                return previous;
            }
            public RouteWrapper GetNext()
            {
                return next;
            }
            public int Getnum()
            {
                return stopnumber;
            }
        }

        static public RouteWrapper[] Ringify(int size) // makes a circular doubly linked list of the specified size
        {
            RouteWrapper[] Ringaround;
            Ringaround = new RouteWrapper[size];

            for(int count= 0; count < size; count++)
            {
                Ringaround[count] = new RouteWrapper();
            }

            for(int count = 1; count < size; count++)
            {
                Ringaround[count - 1].SetNext(Ringaround[count]);
                Ringaround[count].SetPrev(Ringaround[count - 1]);
                Ringaround[count].SetNum(size);
            }
            Ringaround[0].SetPrev(Ringaround[size-1]);
            Ringaround[size-1].SetNext(Ringaround[0]);

            return Ringaround;
        }

        public static void outputtofile(string filepath, BussRoute.RouteWrapper[] route)
        {
            if (!File.Exists(filepath))
            {
                using (StreamWriter output = File.CreateText(filepath))
                {
                    output.WriteLine("Initializing record of route states, each iteration will represent the state of the route after a full round of simulation");
                    output.WriteLine("--------------------------");
                }
            }

            using (StreamWriter output = File.AppendText(filepath))
            {
                for(int count = 0; count < route.Length; count++)
                {
                    output.WriteLine("Stop number: " + route[count].Getnum());
                    output.WriteLine("Number of busses at stop: " + route[count].stop.getBussNum());
                    output.WriteLine("Number of waiting passengers at stop: " + route[count].stop.getPassNum());
                    output.WriteLine("--------------------------");
                }
            }
            
        }

    }
}
 