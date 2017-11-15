﻿/* 
 * https://msdn.microsoft.com/en-us/library/hb7xxkfx(v=vs.110).aspx for pinging entropy gen 
 * https://stackoverflow.com/questions/9386672/finding-the-number-of-places-after-the-decimal-point-of-a-double for handling pesky double decimals not converting to int
 */
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Threading;

namespace GenericTools
{
    public class Toolkit
    {

        int Type;
        double seed;
        double multiplier;
        double modulo;
        double increment;

        bool debug;
        List<double> waitinglist;
        List<double> TimeList;
        List<double> Seedlist;
        DateTime LastTime;

        public Toolkit(int type = 0, bool Debug = false) // instance of generator is started with either in built crypto PRG or homebrewed entropy source, debug turns on console output
        {

            debug = Debug;
            Type = type; // 0= internal crypto library, 1 = linear distribution, 2 = exponential distribution, 3  = entropy generated list
            waitinglist = new List<double>();
            if (type > 0)
            {




                if (type == 1)
                {
                    Thread seedthread = new Thread(() => { seed = SeedGen(); });
                    seedthread.Start();
                    Thread.Sleep(1);
                    Thread multhread = new Thread(() => { multiplier = SeedGen(); });
                    multhread.Start();
                    Thread.Sleep(1);
                    Thread modthread = new Thread(() => { modulo = SeedGen(); });
                    modthread.Start();
                    Thread.Sleep(1);
                    Thread incthread = new Thread(() => { increment = SeedGen(); });
                    incthread.Start();
                    Thread.Sleep(1);
                    seedthread.Join();
                    multhread.Join();
                    modthread.Join();
                    incthread.Join();
                }

                if (type == 2)
                {
                    Thread seedthread = new Thread(() => { seed = SeedGen(); });
                    seedthread.Start();
                    Thread.Sleep(1);
                    Thread multhread = new Thread(() => { multiplier = SeedGen(); });
                    multhread.Start();
                    Thread.Sleep(1);
                    Thread modthread = new Thread(() => { modulo = SeedGen(); });
                    modthread.Start();
                    Thread.Sleep(1);
                    Thread incthread = new Thread(() => { increment = SeedGen(); });
                    incthread.Start();
                    Thread.Sleep(1);
                    TimeList = new List<double>();
                    TimeList.Add(0.5);
                    LastTime = DateTime.Now;
                    seedthread.Join();
                    multhread.Join();
                    modthread.Join();
                    incthread.Join();
                }
                if (type == 3)
                {
                    Seedlist = new List<double>();

                    Thread Genthread = new Thread(() => { ListGenerator(); });
                    Genthread.Start();
                }

            }


        }

        void ListGenerator()
        {
            while (true)
            {
                Thread.Sleep(1);
                if (Seedlist.Count < 300)
                {
                    Thread[] threadarray = new Thread[75];
                    for (int count = 0; count < 75; count++)
                    {
                        threadarray[count] = new Thread(() => { Seedlist.Add(SeedGen()); });
                        threadarray[count].Start();
                    }
                    for (int count = 0; count < 75; count++)
                    {
                        threadarray[count].Join();
                    }
                }
            }
        }
        double TimeAverage()
        {
            DateTime temp = DateTime.Now;
            double TimeDif = (LastTime.Ticks - temp.Ticks) % 511;
            LastTime = temp;
            TimeList.Add(TimeDif);
            double avg = 0;
            for (int count = 0; count < TimeList.Count - 1; count++)
            {
                avg += TimeList[count] / TimeList.Count;
            }
            while (TimeList.Count > 100)
            {
                TimeList.RemoveAt(0);
            }

            return avg;
        }

        public double getSeed()
        {
            return seed;
        }
        public double getMult()
        {
            return multiplier;
        }

        public double getInc()
        {
            return increment;
        }

        public double getMod()
        {
            return modulo;
        }

        public bool Eventgenerator(double freq) // using built in cryptographic random # generator to produce true returns 'freq' percentage of the time
        {
            double randomnum = ReallyRandom();

            if ((randomnum % 100) <= freq)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public double ReallyRandom(bool bypass = false)
        {
            if (bypass == false)
            {
                double waitingnum = this.ReallyRandom(true);
                this.waitinglist.Add(waitingnum);
                while (waitinglist[0] != waitingnum) // access control so multiple threads can't get the same value
                {
                    Thread.Sleep(1);
                }
            }
            double randomnum = 1;

            if (Type == 0 | bypass == true)
            {
                RNGCryptoServiceProvider gibberish = new RNGCryptoServiceProvider();
                byte[] buffer = new byte[8];
                gibberish.GetBytes(buffer);
                randomnum = Math.Abs(BitConverter.ToInt64(buffer, 0));
            }
            else if (Type == 1)
            {
                randomnum = LinearDis();
            }
            else if (Type == 2)
            {
                randomnum = ExpoDis();
            }
            else if (Type == 3)
            {
                randomnum = EntroDis();
            }

            if (bypass == false)
            {
                waitinglist.RemoveAt(0);
            }
            return Math.Abs(randomnum);
        }

        double LinearDis()
        {


            seed = (((seed * multiplier) + increment) % modulo);

            return seed;
        }

        double ExpoDis()
        {

            seed = (-(1 / TimeAverage()) * Math.Log(LinearDis() / modulo)) * 100; //some typecasting bullshittery is needed to  get the Log() function to play nice with double typecasting
            if (double.IsNaN(seed) | double.IsInfinity(seed)) //if values get too small for double, reinitialize from crypto library and carry on
            {
                seed = this.ReallyRandom(true);
            }
            double precision = 1;
            while ((decimal)seed * (decimal)Math.Pow(10, precision) != Math.Round((decimal)seed * (decimal)Math.Pow(10, precision))) precision++;
            if (precision != 1)
            {
                seed *= (precision * 10);
            }
            return seed;

        }

        double EntroDis()
        {
            while (Seedlist.Count == 0)
            {
                Thread.Sleep(1);
            }
            double temp = Seedlist[0];
            Seedlist.RemoveAt(0);
            return temp;
        }

        public double SeedGen() //generates a random double number
        {
            double initialnum = 1;
            Ping Sender = new Ping();
            List<Thread> Pinglist = new List<Thread>();
            if (debug == true)
            {
                Console.WriteLine("Generating Seed value");
            }
            int count = 0;
            while (initialnum < 4093082899) // multiply until passing a sufficiently large number
            {

                Thread Pinger = new Thread(() =>
                {
                    IPAddress IP1 = ValidIP();
                    if (debug == true)
                    {
                        Console.WriteLine("Selected " + IP1);
                    }
                    try
                    {
                        PingReply reply = Sender.Send(IP1);
                        if (debug == true)
                        {
                            Console.WriteLine("Response time is " + reply.RoundtripTime);
                        }
                        if (reply.RoundtripTime != 0)
                        {
                            initialnum = initialnum * reply.RoundtripTime;

                        }
                        if (debug == true)
                        {
                            Console.WriteLine("Value is " + initialnum);
                        }
                    }
                    catch
                    {
                        if (debug == true)
                        {
                            Console.WriteLine("ping failed, trying another");
                        }
                    }
                });
                Pinglist.Add(Pinger);
                Pinger.Start();


                if (count % 200 == 0)
                {
                    if (debug == true)
                    {
                        Console.WriteLine("Waiting on pings to finish up");
                    }
                    for (int counter = 0; counter < Pinglist.Count - 1; counter++)
                    {
                        Pinglist[counter].Join();
                    }
                    Pinglist.Clear();

                }
                count++;

            }
            for (int counter = 0; counter < Pinglist.Count - 1; counter++)
            {
                Pinglist[counter].Join();
            }
            initialnum = initialnum % 4093082899; //large prime number close to the limit of an double
            if (debug == true)
            {
                Console.WriteLine("seed is " + initialnum);
            }
            return initialnum;

        }

        IPAddress ValidIP() // returns a valid non-reserved IP address
        {
            double oct1 = (ReallyRandom(true) % 255);
            double oct2 = (ReallyRandom(true) % 255);
            double oct3 = (ReallyRandom(true) % 255);
            double oct4 = (ReallyRandom(true) % 255);
            string IPstring;
            IPAddress newaddr;

            while (oct1 == 10 | oct1 == 127)
            {
                oct1 = (ReallyRandom(true) % 255);
            }
            while ((oct1 == 172 & (oct2 < 32 & oct2 > 15)) | (oct1 == 192 & oct2 == 168))
            {
                oct1 = (ReallyRandom(true) % 255);
                oct2 = (ReallyRandom(true) % 255);
            }
            IPstring = oct1 + "." + oct2 + "." + oct3 + "." + oct4;
            IPAddress.TryParse(IPstring, out newaddr);

            return newaddr;
        }
    }
}
