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
            }

            bigtimer.Stop();
            TimeSpan rundurationraw = bigtimer.Elapsed();
            string runduration = String.Format("{0:00}:{1:00}:{2:00}.{3.00}", rundurationraw.Hours, rundurationraw.Minutes, rundurationraw.Seconds, rundurationraw.Milliseconds / 10);
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }
    }
}
 