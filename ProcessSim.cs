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

using System.Threading;
using GenericTools;
using System.Collections.Generic;
using System;

namespace SimulationCore
{
    class Circuit
    {
        public List<ProcessSim> Readyqueue;
        public List<ProcessSim> IOqueue;
        public ProcessSim[] IOspace;
        public ProcessSim[] CPUspace;

        public Circuit()
        {
            Readyqueue = new List<ProcessSim>();
            IOqueue = new List<ProcessSim>();
            IOspace = new ProcessSim[1];
            CPUspace = new ProcessSim[1];
            CPUspace[0] = null;
            IOspace[0] = null;
        }

    }
    class ProcessSim
    {
        public string currentqueue;
        public bool incrit;
        public double identnum;
        public double runlength;
        public double runtime;
        public double waittime;
        public double cpuuse;
        public Toolkit ExpoTool;
        public ProcessSim(double runlenght, Toolkit expoTool)
        {
            incrit = false;
            currentqueue = "start";
            ExpoTool = expoTool;
            
            identnum = ExpoTool.ReallyRandom();
            
            runlength = runlenght;
            runtime = 0;
            waittime = 0;
            cpuuse = 0;
        }

        public void Driver(Circuit circuit, Controller controller)
        {
            Toolkit expotool = ExpoTool;

            while (controller.getState() == false)
            {
                Thread.Sleep(1);
            }
            DateTime starttime = DateTime.Now;
            

            while (runtime < runlength)
            {
                
                ReadyandCPU(circuit, expotool);
                circuit.CPUspace[0] = null;
                if (runtime < runlength)
                {
                    IOsequence(circuit, expotool);
                    circuit.IOspace[0] = null;
                }
            }
            this.currentqueue = "Finished";
            Console.WriteLine("Process " + this.identnum + " is finished");
        }

        void IOsequence(Circuit circuit, Toolkit expotool)
        {
            circuit.IOqueue.Add(this);
            this.currentqueue = "IO Queue";
            Console.WriteLine("Process " + this.identnum + " entering io queue");
            
            while (circuit.IOqueue[0].identnum != this.identnum) //waiting in line on IO queue
            {
                Thread.Sleep(1);
                this.waittime++;
            }
            while(circuit.IOspace[0] != null) //waiting for IO device to become available
            {
                Thread.Sleep(1);
                this.waittime++;
            }
            circuit.IOspace[0] =this;
            circuit.IOqueue.Remove(this);
            this.currentqueue = "IO space";
            Console.WriteLine("Process " + this.identnum + " entering io space");
            Thread.Sleep(60);
            this.runtime += 60;

        
        }
        void ReadyandCPU(Circuit circuit, Toolkit expotool)
        {
            circuit.Readyqueue.Add(this);
            this.currentqueue = "Ready";
            Console.WriteLine("Process " + this.identnum + " is entering Ready queue");
            double smallnum = circuit.Readyqueue[0].runlength;
            while (smallnum != this.runlength) //waiting in line on ReadyQueue, checking to see if it is the smallest in the queue yet
            {
                
                smallnum = circuit.Readyqueue[0].runlength;
               
                for (int count =0; count < circuit.Readyqueue.Count; count++)
                {
                    if(smallnum > circuit.Readyqueue[count].runlength)
                    {
                        smallnum = circuit.Readyqueue[count].runlength;
                    }
                    

                }
                 
                Thread.Sleep(1);
                this.waittime++;
                
            }

            while (circuit.CPUspace[0] != null) //waiting for cpu to become available
            {
                Thread.Sleep(1);
                this.waittime++;
            }
            
            circuit.CPUspace[0] = this;
            circuit.Readyqueue.Remove(this); //transfering to cpu from ready queue
            this.currentqueue = "CPU";
            double sleeptime = expotool.ReallyRandom();
            
            int sleeptime2;
            
            
            
          
            if (sleeptime > (runlength-runtime))
            {
                sleeptime = (runlength - runtime);
                
            }
            sleeptime = Convert.ToInt32(Math.Abs((sleeptime % ((Int32.MaxValue / 2) - 1))));
            sleeptime2 = Convert.ToInt32(sleeptime);

            Thread.Sleep(sleeptime2); //simulated usage of cpu, time spent is PRG according to exponential distribution, converted from double to int and modulo'd to prevent overflow errors
            Console.WriteLine("Process " + this.identnum + " entering cpu queue for "+sleeptime);
            runtime += sleeptime;
            cpuuse += sleeptime;

        }
       
    }
}
 