using GenericTools;
using System.Threading;
using System;
namespace SimulationCore
{
    class Rider //generic holder for riders and whether they are on a buss or waiting
    {
        bool riding;
        int attentionspan;

        public Rider()
        {
            riding = false;
            Toolkit Tool = new Toolkit();
            attentionspan = ((Tool.ReallyRandom()) % 100); //how likely this user is to get on or off a buss
        }
        
        public int getattention()
        {
            return attentionspan;
        }

        public bool getState() //probably not needed, good form nonetheless
        {
            return riding;
        }

        public void toggleState()
        {
            if (riding == false)
            {
                riding = true;
            }
            else
            {
                riding = false;
            }
        }

        static public void Repopulate(int chance, BussRoute.RouteWrapper[] route) //background thread that randomly adds waiting passengers to the stops
        {
            Toolkit Tool = new Toolkit();
            while (true)
            {
                Console.WriteLine("Repopulation pass occuring at: " + DateTime.Now);
                for (int count = 0; count < route.Length; count++)
                {
                    if (count > route.Length)
                    {
                        count = count % route.Length;
                    }

                    if (Tool.Eventgenerator(chance) == true) //at each stop, there is a 'chance' percent chance of adding a new waiting passenger to that stop
                    {
                        Rider newguy = new Rider();
                        route[count].stop.addWaiter(newguy);
                    }
                }
                Thread.Sleep(Tool.ReallyRandom()%60000);
                
            }
        }
    }
}

 