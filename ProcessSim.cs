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

namespace SimulationCore
{
    class Circuit
    {
        List<ProcessSim> Readyqueue;
        List<ProcessSim> IOqueue;
        List<ProcessSim> IOspace;
        List<ProcessSim> CPUspace;

        public Circuit()
        {
            Readyqueue = new List<ProcessSim>();
            IOqueue = new List<ProcessSim>();
            IOspace = new List<ProcessSim>();
            CPUspace = new List<ProcessSim>();
        }

    }
    class ProcessSim
    {
        public string currentqueue;
        public bool incrit;
        public long identnum;
        public long runlength;
        public ProcessSim(Toolkit tool, long runlenght)
        {
            incrit = false;
            currentqueue = "start";
            identnum = tool.ReallyRandom();
            runlength = runlenght;
        }

        public void Driver(Circuit circuit, Toolkit tool, Controller controller)
        {
            long startdelay = tool.ReallyRandom() % 60;
            while (controller.getState() == false)
            {
                Thread.Sleep(1);
            }
            for(int count = 0; count < startdelay; count++) // randomizing starts a bit to prevent possible collisions
            {
                Thread.Sleep(1);
            }
        }
    }
}
 