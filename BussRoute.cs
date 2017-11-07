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

        public static void Outputtofile(string filepath, BussRoute.RouteWrapper[] route, List<Buss> bussList, Controller controller) //periodically records the state of the route to file
        {

            filepath = Path.GetFullPath(filepath);

            using (StreamWriter output = File.CreateText(filepath))
            {
                string line = "| ";
                for (int count = 0; count < bussList.Count; count++)
                {
                    line += count + "    ";
                }
                output.WriteLine(line);
            }
                while (controller.getState() == false) // wait for simulation to begin
                {
                    Thread.Sleep(1);
                }
            while(controller.getState()==true) // iterate until end of simulation, then conclude current iteration
            {
                for(int count = 0; count < bussList.Count; count++)
                {

                }
            }
        }

    }
}
 