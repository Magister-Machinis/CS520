using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GenericTools;
namespace SimulationCore
{
    class Buss //generic holder class for the busses along with a stack of riders
    {

        BussRoute.RouteWrapper currentstop;
        List<Rider> passenger;
        int Bussnum;
        Toolkit Tools = new Toolkit();
        bool intransit;
        bool asleep;
        int Bursttime;
        int Arrivaltime;
        int Waittime;

        public Buss(int busnum, int bursttime, int arrivaltime)
        {
            passenger = new List<Rider>();
            Bussnum = busnum;
            intransit = false;
            asleep = false;
            Bursttime = bursttime;
            Arrivaltime = arrivaltime;
            Waittime = 0;

        }
        public int getWait()
        {
            return Waittime;
        }

        public bool getSleep()
        {
            return asleep;
        }

        public void ToggleSleep()
        {
            if(asleep == true)
            {
                asleep = false;
            }
            else
            {
                asleep = true;
            }
        }
        public int getBurst()
        {
            return Bursttime;
        }
        public int getArrival()
        {
            return Arrivaltime;
        }

        public bool getTravel()
        {
            return intransit;
        }

        public void toggleTransit()
        {
            if(intransit == false)
            {
                intransit = true;
            }
            else
            {
                intransit = false;
            }
        }
        public BussRoute.RouteWrapper getStop()
        {
            return currentstop;
        }

        public void setStop(BussRoute.RouteWrapper newstop)
        {
            currentstop = newstop;
        }
        public Rider getPassenger() //views top of rider queue
        {
            return passenger[0];
        }

        public Rider popPassenger(int adder)
        {
            Console.WriteLine("Removing passenger from buss " + this.GetNum());
            Rider toppass = passenger[0];
            passenger.RemoveAt(0);
            Thread.Sleep(2000); //takes 2 seconds for passenger to get kicked off
            return toppass;
        }

        public int GetNum()
        {
            return Bussnum;
        }

        public void stackPassenger(Rider newpass)
        {
            Console.WriteLine("adding new passenger from buss "+this.GetNum());
            passenger.Add(newpass);
            Thread.Sleep(2000); //takes 2 seconds for passenger to scramble aboard
        }

        public void MoveToNextStop()
        {
            Console.WriteLine("Buss number: " + this.GetNum() +" is moving to next stop");
            this.toggleTransit();
            this.currentstop.addBuss(this.currentstop.stop.popBuss()); //moving buss from loading queue to traveling queue

            for (int sleepcount = 1; sleepcount < 30; sleepcount++)
            {
                Thread.Sleep(10000);
            }

            while ((this.currentstop.getFrontbuss()).GetNum() != this.GetNum())//waits for this buss to be at the front of the queue and is simulating 5 minutes of 'travel' time, cant do it in one sleep command because it takes an int32 input for milliseconds
            {
                for (int sleepcount = 1; sleepcount < 30; sleepcount++) 
                {
                    Thread.Sleep(10000);
                }
            }
            currentstop.popBuss(); //removes buss from travel queue once it sees that its at the front
            currentstop = currentstop.GetNext();
            
               
            currentstop.stop.addBuss(this); //adds buss to end of new stops queue as it arrives
            this.toggleTransit();
        }

        public void BussDriver(double NumberofRounds, Controller controller) //function acts as the holder for each thread
        {
            while(controller.getState() == false) //waiting for the simulation to start
            {
                Thread.Sleep(1);
            }
            this.currentstop.stop.addBuss(this); // adds this item to the waiting queue once it arrives
            for (int count = 0; count < this.getArrival(); count++) //postpones start for selected arival time
            {
                Thread.Sleep(1);
            }
           
            this.ToggleSleep();
            while ((currentstop.stop.getBuss().GetNum()) != this.Bussnum) //checks to see if this item is the one at the front of the queue, sleeps a time if not
            {
                Thread.Sleep(1);
                Waittime++;
            }
            this.ToggleSleep();
            this.toggleTransit(); // marks item as active for the recorder
            for (int count = 0; count < this.getBurst(); count++) //simulates time spent using critical region
            {
                Thread.Sleep(1);
            }
            this.toggleTransit();
            
            this.currentstop.stop.popBuss(); // gets out of the way for the next setup
            /* code from previous project
            Console.WriteLine(this.GetNum() + " Buss has begun!");
            for (double roundcount = 0; roundcount < NumberofRounds; roundcount++) //main routine for this bus on the route
            {
                
                Console.WriteLine("Buss " + this.GetNum()+ " is begining round " + roundcount);
                Rider currentguy;
                bool flagval = false;
                while (flagval == false)
                {
                    if (currentstop.stop.getPassNum() > 0) //checking if there is anyone waiting at the stop, else assigning a flag value
                    {
                        currentguy = currentstop.stop.getFront();
                    }
                    else
                    {
                        currentguy = null;
                    }


                    if (currentguy != null)
                    {
                        if (Tools.Eventgenerator(currentguy.getattention()) == true) //buss loads passengers until no passengers left or one doesnt want to get on
                        {
                            Console.WriteLine("Buss " + this.GetNum() + " is loading a passenger");
                            currentstop.stop.popFront();
                            currentguy.toggleState();
                            this.stackPassenger(currentguy);
                        }
                        else
                        {
                            flagval = true;
                            Console.WriteLine(this.GetNum() + " buss is not wanted by passengers remaining");
                        }
                    }
                    else
                    {
                        flagval = true;
                        Console.WriteLine(this.GetNum() + " buss has no more passengers to pick up at this stop");
                    }
                }
                Console.WriteLine("Buss " + this.GetNum() + " is leaving the station");
                if (flagval == true) // if true then buss is done loading passengers at this stop
                {
                    this.MoveToNextStop();
                }
                Console.WriteLine("round " + roundcount + " has finished for buss " + this.GetNum()+" waiting in line at next stop now");

            }
            */
        }
    }
}
    
 