/*
 * information sources:
 * msdn.microsoft.com
 * tutorialspoint.com
 *  https://stackoverflow.com/questions/4892588/rngcryptoserviceprovider-random-number-review (bits and bobs used to build randomeventgenerator)
 *  https://www.tutorialspoint.com/csharp/csharp_file_io.htm (file IO examples because i ALWAYS screw that up)
 *  https://stackoverflow.com/questions/1360533/how-to-share-data-between-different-threads-in-c-sharp-using-aop (settling some fears about sharing flags, its easier in this than in c++)
 * Hosted at:
 * https://github.com/Magister-Machinis/CS520 5.3 fork(amongst my other efforts)
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
            Toolkit ExpoTools = new Toolkit(2);
            Toolkit UniTools = new Toolkit(3);
            ProcessSim[] Proclist = new ProcessSim[10];
            Thread[] threadlist = new Thread[Proclist.Length];
            Circuit circuit = new Circuit();
            Controller controller = new Controller();
            for (int count = 0; count < Proclist.Length; count++)
            {
                Proclist[count] = new ProcessSim(ExpoTools, (120000 + (UniTools.ReallyRandom() % 120000))); //gives a runtime  between 2 and 4 minutes with uniform distribution
                threadlist[count] = new Thread(() => { Proclist[count].Driver(circuit, ExpoTools, controller); });
                threadlist[count].Start();
            }
            Thread recordthread



            bigtimer.Stop();
            TimeSpan rundurationraw = bigtimer.Elapsed;

            Console.WriteLine("Runtime " + rundurationraw);
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }

    }
    class Recorder
    {
        string filepath;
        public Recorder()
        {
            filepath = @".\output.txt";
            filepath = Path.GetFullPath(filepath);
        }

        public void paperbackwriter(Controller controller)
        {
            
            using (StreamWriter output = File.CreateText(filepath))
            {
                while (controller.getState() == false)
                {
                    Thread.Sleep(1);
                }
                DateTime timer = DateTime.Now;
                output.WriteLine("Begining State record at  "+timer);

                while (controller.getState() == true)
                {

                }
                

            }
        }
    }
}

     

 