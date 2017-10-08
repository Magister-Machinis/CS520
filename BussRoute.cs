﻿using System.IO;
using System.Threading;
using System.Globalization;
using System;

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

        public static void Outputtofile(string filepath, BussRoute.RouteWrapper[] route) //periodically records the state of the route to file
        {
            int counter = 0;
            Console.WriteLine("Recorder initiated at "+ DateTime.Now);
            while (true)
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
                        output.WriteLine("Initializing record of route states, each iteration will represent the state of the route after a full round of simulation");
                        output.WriteLine("--------------------------");
                        output.WriteLine("======================");
                    }
                }

                using (StreamWriter output = File.AppendText(filepath))
                {
                    
                    output.WriteLine("Begin Simulation Round Record:"+counter+" time is " + DateTime.Now);
                    output.WriteLine("======================");
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
 