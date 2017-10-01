/*
 * information sources:
 * msdn.microsoft.com
 * tutorialspoint.com
 *  https://stackoverflow.com/questions/4892588/rngcryptoserviceprovider-random-number-review (bits and bobs used to build randomeventgenerator)
 * Hosted at:
 * https://github.com/Magister-Machinis/CS520 (amongst my other efforts)
 * 
 * John Fromholtz
 * CS520 Fall 2017
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using GenericTools;

namespace SimulationCore
{
    class Program
    {
 
        static void Main(string[] args)
        {
            Stopwatch littletimer = new Stopwatch(); //timer for use if user select automatic lengthed simulation
            Stopwatch bigtimer = new Stopwatch(); //timer for runtime statistics
            bigtimer.Start();
            littletimer.Start();
            Toolkit Tools = new Toolkit();
            BussRoute route = new BussRoute();
            int NumberofStops = 16;
            double numberofrounds = 0;
            int frequency = 10; // percent chance that random function will trigger

            Console.WriteLine("input number of rounds of simulation, or enter 0 for autocalculation of rounds (may be long!)");
            numberofrounds = Convert.ToDouble(Console.ReadLine());
            littletimer.Stop();

            if (numberofrounds == 0)
            {
                numberofrounds = Convert.ToDouble(littletimer.ElapsedTicks);
                /* 
                 * auto-assign simulation length uses the number of ticks it 
                 * takes for the user to decide to use this option as its value, 
                 * using crypto grade randomness later for spice
                 */
            }
            Console.WriteLine("Running " + numberofrounds + " rounds of simulation");


            while (numberofrounds > 0) //main event loop
            {
                numberofrounds--;
                if (Tools.Eventgenerator(frequency) == true) //deciding whether to add a new buss to the congaline
                {

                }

            }
            bigtimer.Stop();
            TimeSpan rundurationraw = bigtimer.Elapsed;
            string runduration = String.Format("{0:00}:{1:00}:{2:00}.{3.00}", rundurationraw.Hours, rundurationraw.Minutes, rundurationraw.Seconds, rundurationraw.Milliseconds / 10);
            Console.WriteLine("Runtime " + runduration);
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }
    }
}
 