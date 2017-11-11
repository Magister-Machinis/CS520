﻿using System.IO;
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


        public static void RoundnRound(List<Buss> bussList, Controller controller) //controls roundrobining of threads
        {
            int quanta = 1;
            while (controller.getState() == false) //waiting for simulation to start
            {
                Thread.Sleep(1);
            }
            while (controller.getState() == true)
            {
                for (int count = 0; count < bussList.Count; count++)
                {

                    bussList[count].ToggleSleep();
                    Thread.Sleep(quanta);
                    bussList[count].ToggleSleep();
                    
                    for(int precount = 0; precount < count; precount++) //simulating wait times for all other threads
                    {
                        bussList[precount].TickWait();
                    }
                    for(int postcount = count+1;postcount<bussList.Count; postcount++)
                    {
                        bussList[postcount].TickWait();
                    }

                }
            }
        }
        public static void Outputtofile(string filepath, BussRoute.RouteWrapper[] route, List<Buss> bussList, Controller controller) //periodically records the state of the route to file
        {

            filepath = Path.GetFullPath(filepath);

            using (StreamWriter output = File.CreateText(filepath))
            {
                output.WriteLine("N= Not running, W= Waiting, A= Active");
                string line = "|";
                for (int count = 0; count < bussList.Count; count++)
                {
                    line += "  "+count + "  ";
                }
                line += "|";
                output.WriteLine(line);

                while (controller.getState() == false) // wait for simulation to begin
                {
                    Thread.Sleep(1);
                }
                int counter = 0;
                while (controller.getState() == true) // iterate until end of simulation, then conclude current iteration
                {
                    line = "Time: " + counter+"|";
                    counter += 1;
                    for (int count = 0; count < bussList.Count; count++)
                    {
                        if (bussList[count].getSleep() == true)
                        {
                            line += "  W  ";
                        }
                        else if (bussList[count].getTravel() == true)
                        {
                            line += "  A  ";
                        }
                        else if (bussList[count].getTravel() == false & bussList[count].getSleep() == false)
                        {
                            line += "  N  ";
                        }

                        line += "|";
                        
                    }
                    output.WriteLine(line);
                    Thread.Sleep(1);
                }

                
                int[] numnums = new int[bussList.Count];
                for (int count = 0; count < bussList.Count; count++)
                {
                    for (int innercount = 0; innercount <= count; innercount++)
                    {
                        numnums[count] += (bussList[innercount].getBurst() + bussList[innercount].getWait());
                    }
                    output.WriteLine("Process " + count + ":");
                    output.WriteLine("  Burst Time: " + bussList[count].getBurst());
                    output.WriteLine("  Turnaround Time: " + (bussList[count].getBurst() + bussList[count].getWait()));
                    output.WriteLine("  Wait Time: " + bussList[count].getWait());

                }
                int avg = 0;
                for (int count = 0; count < numnums.Length; count++)
                {
                    avg += numnums[count] / numnums.Length;
                }
                output.WriteLine("Average time to completion is: "+avg);
            }
        }

    }
}
 