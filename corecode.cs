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

            List<Buss> bussList = new List<Buss>();

            int NumberofStops;
            Console.WriteLine("Input number of stops in the route to be simulated: ");
            NumberofStops = Convert.ToInt32(Console.ReadLine());

            BussRoute.RouteWrapper[] route = BussRoute.Ringify(NumberofStops);
            double numberofrounds = 0;
            int frequency = 10; // percent chance that random function will trigger a buss appearing or leaving circuit
            int footTraffic = 25; // percent chance of a new passenger appearing at a stop
            
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
            Console.WriteLine("Running " + numberofrounds + " rounds of simulation with a route of " + NumberofStops + " stops.");



            while (numberofrounds > 0) //main event loop
            {
                numberofrounds--;
                for(int count = 0; count < NumberofStops; count++) //randomly adds in new riders at each stop
                {
                    if(Tools.Eventgenerator(footTraffic) == true)
                    {
                        Rider anotherone = new Rider();
                        route[count].stop.addWaiter(anotherone);
                    }
                }


                if (Tools.Eventgenerator(frequency) == true) //deciding whether to add a new buss to the congaline
                {
                    Buss newbuss = new Buss();
                    newbuss.setStop(route[0]);
                    route[0].stop.addBuss(newbuss);
                    bussList.Add(newbuss);
                }

                for(int count = NumberofStops-1; count >=0; count --) // walking backwards through the stops to process the busses as they crawl around the route
                {
                    while (route[count].stop.getBussNum() != 0)
                    {
                        Rider currentguy = route[count].stop.getFront();
                        Buss currentbuss = route[count].stop.getBuss();
                        Rider frontpassenger = currentbuss.getPassenger();
                        
                        if (Tools.Equals(currentguy.getattention()) == true && route[count].stop.getPassNum() > 0) //buss loads passengers until no passengers left or one doesnt want to get on
                        {
                            route[count].stop.popFront();
                            currentbuss.stackPassenger(currentguy);
                        }
                        else
                        {
                            route[count].stop.popBuss();
                            BussRoute.RouteWrapper nextstop = route[count].GetNext();
                            nextstop.stop.addBuss(currentbuss);
                        }
                    }
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
 