/*
 * information sources:
 * msdn.microsoft.com
 * tutorialspoint.com
 *  https://stackoverflow.com/questions/4892588/rngcryptoserviceprovider-random-number-review (bits and bobs used to build randomeventgenerator)
 *  https://www.tutorialspoint.com/csharp/csharp_file_io.htm (file IO examples because i ALWAYS screw that up)
 *  https://stackoverflow.com/questions/1360533/how-to-share-data-between-different-threads-in-c-sharp-using-aop (settling some fears about sharing flags, its easier in this than in c++)
 * Hosted at:
 * https://github.com/Magister-Machinis/CS520 (amongst my other efforts)
 * 
 * John Fromholtz
 * CS520 Fall 2017
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
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
            List<Thread> threadlist = new List<Thread>();
            int NumberofStops;
            Console.WriteLine("Input number of stops in the route to be simulated: ");
            NumberofStops = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine(NumberofStops);
            BussRoute.RouteWrapper[] route = BussRoute.Ringify(NumberofStops);
            double numberofrounds = 0;
            int footTraffic = 50; // percent chance of a new passenger appearing at a stop
            int NumberofBusses = 10; // how many busses are going to be in this simulation

            Console.WriteLine("input number of rounds of simulation, or enter 0 for autocalculation of rounds (may be long!)");
            numberofrounds = Convert.ToDouble(Console.ReadLine());
            littletimer.Stop();

            string path = Path.GetFullPath(".\\output.txt");

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

            for (int count = 0; count < NumberofBusses; count++) //adding busses to the starting line
            {
                Buss newbuss = new Buss();
                newbuss.setStop(route[0]);
                route[0].stop.addBuss(newbuss);
                bussList.Add(newbuss);
            }
            Thread populating = new Thread(() => Rider.Repopulate(footTraffic, route)); //activating the background thread that adds in people
            populating.Start();
            Thread snapshot = new Thread(() => BussRoute.Outputtofile(path, route)); //activating the background thread that records all of this
            snapshot.Start();
            for(int buscount = 0; buscount < bussList.Count; buscount++) //sending off each buss as a thread, intentionally doing an asynchronous activation to mix them up a bit
            {
                Thread.Sleep(Tools.ReallyRandom());
                Thread bussymcbussface = new Thread(() => bussList[buscount].BussDriver(numberofrounds));                
                bussymcbussface.Start();
                threadlist.Add(bussymcbussface);
            }

            foreach(Thread item in threadlist) //waiting on busses to finish their rounds before killing the background threads
            {
                item.Join();
            }
            populating.Abort();
            snapshot.Abort();

            /* initial sketching of linear simulation
            while (numberofrounds > 0) //main event loop
            {
                numberofrounds--;
                BussRoute.Outputtofile(path, route); //recording current state of the route
                for (int count = 0; count < NumberofStops; count++) //randomly adds in new riders at each stop
                {
                    if (Tools.Eventgenerator(footTraffic) == true)
                    {
                        Rider anotherone = new Rider();                      
                        route[count].stop.addWaiter(anotherone);
                    }
                }

                
                

                
                for (int count = NumberofStops - 1; count >= 0; count--) // walking backwards through the stops to process the busses as they crawl around the route
                {
                    while (route[count].stop.getBussNum() != 0)
                    {
                        Rider currentguy;
                        if (route[count].stop.getPassNum() > 0) //checking if there is anyone waiting at the stop, else assigning a flag value
                        {
                            currentguy = route[count].stop.getFront();
                        }
                        else
                        {
                            currentguy = null;
                        }
                        Buss currentbuss = route[count].stop.getBuss();
                        bool flagval = false;
                        if (currentguy != null)
                        {
                            if (Tools.Eventgenerator(currentguy.getattention()) == true) //buss loads passengers until no passengers left or one doesnt want to get on
                            {
                                route[count].stop.popFront();
                                currentguy.toggleState();
                                currentbuss.stackPassenger(currentguy);
                            }
                            else
                            {
                                flagval = true;
                            }
                        }
                        else
                        {
                            flagval = true;
                        }

                        if(flagval == true)
                        {
                            route[count].stop.popBuss();
                            BussRoute.RouteWrapper nextstop = route[count].GetNext();
                            nextstop.stop.addBuss(currentbuss);
                        }
                    }
                }
                */

        

        bigtimer.Stop();
        TimeSpan rundurationraw = bigtimer.Elapsed;
            
        Console.WriteLine("Runtime " + rundurationraw);
        Console.WriteLine("Press any key to exit");
        Console.ReadLine();
        }

    }
}
 