/*
 * 
 * John Fromholtz
 * CS520 Fall 2017
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;


namespace SimulationCore
{
    class Memunit //holds description of each memory bit
    {
        public bool allocated;
        public int location;
        public Memunit(int loc)
        {
            allocated = false;
            location = loc;
        }
    }
    class Program
    {

        static void Main(string[] args)
        {
            
            Stopwatch bigtimer = new Stopwatch(); //timer for runtime statistics
            bigtimer.Start();

            List<List<Memunit>> Memlists = new List<List<Memunit>>; 

            Console.WriteLine("Input size of memory Block: ");
            int memsize = Convert.ToInt32(Console.ReadLine());
            int[] Memarray = new int[memsize];


           



            bigtimer.Stop();
            TimeSpan rundurationraw = bigtimer.Elapsed;

            Console.WriteLine("Runtime " + rundurationraw);
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }

    }
    class Controller //generic holder for inter-process flag values and such
    {
        bool StartStop;

        public Controller()
        {
            StartStop = false;
        }
                
        public bool getState()
        {
            return StartStop;
        }

        public void toggleState()
        {
            if (StartStop == false)
            {
                StartStop = true;
            }
            else
            {
                StartStop = false;
            }
        }
    }
}
 