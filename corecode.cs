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

using System.Diagnostics;
using System.IO;
using System.Threading;
using GenericTools;
using System.Collections.Generic;

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
            Console.WriteLine("initializing uniform toolkit");
            Toolkit UniTools = new Toolkit(3);
            Toolkit Expotools = new Toolkit(2);




            List<ProcessSim> rawProclist = new List<ProcessSim>();
            ProcessSim[] Proclist = new ProcessSim[10];
            Thread[] threadlist = new Thread[10];
            Circuit circuit = new Circuit();
            Controller controller = new Controller();
            for (int count = 0; count < Proclist.Length; count++)
            {
                Console.WriteLine("Creating process " + count);
                double temp = (120000 + ((UniTools.ReallyRandom()) % 120000));
                Console.WriteLine("Run time will be " + temp);
                ProcessSim temper = new ProcessSim(temp, Expotools); //gives a runtime  between 2 and 4 minutes with uniform distribution
                Proclist[count] = temper;
                Console.WriteLine("Launching process" + count);
                Thread temp2 = new Thread(() => { temper.Driver(circuit, controller); });
                threadlist[count] = temp2;
                temp2.Start();
                Thread.Sleep(500);

            }




            Recorder recorder = new Recorder();
            Thread recordthread = new Thread(() => { recorder.paperbackwriter(controller, Proclist); });
            recordthread.Start();

            controller.toggleState();
            for (int count = 0; count < Proclist.Length; count++)
            {
                Console.WriteLine("Waiting on process" + count);
                threadlist[count].Join();
            }
            controller.toggleState();
            recordthread.Join();



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

        public void paperbackwriter(Controller controller, ProcessSim[] proclist)
        {
            double cpucount = 0;
            double usecount = 0;
            using (StreamWriter output = File.CreateText(filepath))
            {
                while (controller.getState() == false)
                {
                    Thread.Sleep(1);
                }
                Stopwatch runtime = new Stopwatch();

                output.WriteLine("Begining State record at  "+ DateTime.Now);

                while (controller.getState() == true)
                {
                    //usecount++;
                    //output.WriteLine(" ");
                    //output.WriteLine("Current timestamp: " + DateTime.Now);
                    //for (int count = 0; count < proclist.Length; count ++)
                    //{
                    //    if(proclist[count].currentqueue == "CPU")
                    //    {
                    //        cpucount++;
                    //    }
                    //    output.WriteLine("Process " + count + " is currently in " + proclist[count].currentqueue);
                    //}
                    //Thread.Sleep(5);
                }
                output.WriteLine(" ");
                
                runtime.Stop();
                TimeSpan rundurationraw = runtime.Elapsed;
                double runtimes = 0;
                double waittimes = 0;
                double turnarounds = 0;
                for(int count = 0; count < proclist.Length; count++)
                {
                    if (count % 1000 == 0) //restricting state records to once a second or so
                    {
                        output.WriteLine("Process " + count);
                        output.WriteLine("    Execution time is: " + proclist[count].runlength);
                        output.WriteLine("    Wait time is " + proclist[count].waittime);
                        output.WriteLine("    Turnaround time is " + (proclist[count].waittime + proclist[count].runlength));
                        output.WriteLine(" ");

                    }
                    runtimes += (proclist[count].runlength / proclist.Length);
                    waittimes += (proclist[count].waittime / proclist.Length);
                    turnarounds += ((proclist[count].runlength + proclist[count].waittime)/ proclist.Length);
                    cpucount += proclist[count].cpuuse;
                    Thread.Sleep(1);
                }
                output.WriteLine("Average run time is: " + runtimes);
                output.WriteLine("Average wait time is: " + waittimes);
                output.WriteLine("Average turnaround is: " + turnarounds);
                output.WriteLine("Simulation Time " + rundurationraw);


            }
        }
    }
}

     

 