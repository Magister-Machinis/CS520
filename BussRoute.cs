using System.IO;
using System.Threading;
using System.Globalization;
using System;
using System.Collections.Generic;

namespace SimulationCore
{
    static class BussRoute //class and subclasses for the route stops as a doubly linked list
    {
        public class RouteWrapper
        {
            public BussStop stop = new BussStop();
            int stopnumber = -1; //flag vaulue, seeing a stop with -1 in records indicates a problem
            RouteWrapper previous;
            RouteWrapper next;
            List<Buss> leavingList = new List<Buss>(); //queue of busses traveling to the next stop, without this to enforce linear order, the busses will eventually drift out of sequence

            public void addBuss(Buss newbuss)
            {
                leavingList.Add(newbuss);
            }

            public Buss getFrontbuss()
            {
                return leavingList[0];
            }

            public Buss popBuss()
            {
                Buss tempholder = leavingList[0];
                leavingList.RemoveAt(0);
                return tempholder;
            }

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
                Ringaround[count].SetNum(count);
            }
            Ringaround[0].SetPrev(Ringaround[size-1]);
            Ringaround[0].SetNum(0);
            Ringaround[size-1].SetNext(Ringaround[0]);

            return Ringaround;
        }

        public static void Outputtofile(string filepath, BussRoute.RouteWrapper[] route, List<Buss> bussList, Controller controller) //periodically records the state of the route to file
        {
            int counter = 0;
            while(controller.getState()==false) // waiting for simulation to start
            {
                Thread.Sleep(1);
            }
            Console.WriteLine("Recorder initiated at "+ DateTime.Now);
            while (controller.getState()==true) //will safely wrap up last iteration when simulation ends now
            {
                for (int sleepcount = 1; sleepcount < 30; sleepcount++) //takes a snapshot of current route state, cant do it in one sleep command because it takes an int32 input for milliseconds
                {
                    Thread.Sleep(10000);
                }
                Console.WriteLine("taking snapshot at "+DateTime.Now);
                if(!File.Exists(filepath))
                {
                    using (StreamWriter output = File.CreateText(filepath))
                    {
                        output.WriteLine("Initializing record of route and buss states, each iteration will represent the state of the route after a full round of simulation");
                        output.WriteLine("--------------------------");
                        output.WriteLine("======================");
                    }
                }

                using (StreamWriter output = File.AppendText(filepath))
                {
                    
                    output.WriteLine("Begin Simulation Round Record:"+counter+" time is " + DateTime.Now);
                    output.WriteLine("======================");
                    output.WriteLine("--------------------------");
                    output.WriteLine("current state of busses");
                    for(int count = 0; count < bussList.Count-1; count++)
                    {
                        output.WriteLine("Buss number: " + bussList[count].GetNum());
                        output.WriteLine("At or has left stop number: " + (bussList[count].getStop()).Getnum());
                        output.WriteLine("In transit between stops? " + bussList[count].getTravel());
                        
                    }
                    output.WriteLine("--------------------------");
                    output.WriteLine("Current state of route");
                    output.WriteLine("--------------------------");
                    for (int count = 0; count < route.Length; count++)
                    {
                        output.WriteLine("Stop number: " + route[count].Getnum());
                        output.WriteLine("Number of busses at stop: " + route[count].stop.getBussNum());
                        output.WriteLine("Number of waiting passengers at stop: " + route[count].stop.getPassNum());
                        output.WriteLine("--------------------------");
                    }
                    output.WriteLine("======================");
                    counter++;
                }
            }   
        }

    }
}
 