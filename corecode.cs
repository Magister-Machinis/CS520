/*
 * information sources:
 * msdn.microsoft.com
 * tutorialspoint.com
 * 
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
            double numberofrounds =0 ;

            Console.WriteLine("input number of rounds of simulation, or enter 0 for autocalculation of rounds (may be long!)");
            numberofrounds = Convert.ToDouble(Console.ReadLine());
            littletimer.Stop();
            
            if (numberofrounds == 0)
            {
                numberofrounds = Convert.ToDouble(littletimer.ElapsedTicks); 
                /* auto-assign simulation length uses the number of ticks it 
                 * takes for the user to decide to use this option as its value, 
                 * was the fastest truly random value I could think of */
            }
            Console.WriteLine("Running " + numberofrounds + " rounds of simulation");

            while (numberofrounds > 0)
            {
                numberofrounds--;

            }
            bigtimer.Stop();
            TimeSpan rundurationraw = bigtimer.Elapsed;
            string runduration = String.Format("{0:00}:{1:00}:{2:00}.{3.00}", rundurationraw.Hours, rundurationraw.Minutes, rundurationraw.Seconds, rundurationraw.Milliseconds / 10);
            Console.WriteLine("Runtime " + runduration);
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }
    }

    class Buss //generic holder class for the busses
    {
        int passenger;
        public int getPassenger()
        {
            return passenger;
        }

        public void addPassenger(int adder)
        {
            passenger = passenger + adder;
        }

        public void setPassener(int newnum)
        {
            passenger = newnum;
        }
    }
}
 