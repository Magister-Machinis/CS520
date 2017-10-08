using GenericTools;
using System.Threading;
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

        public bool getState() //probably not needed, got good form nonetheless
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
            int count = 0; // counter for looping through route, will be using a modulo call to keep it bound within route size
            while (true)
            {
                if (count > route.Length)
                {
                    count = count % route.Length;
                }

                if(Tool.Eventgenerator(chance) == true) //at each stop, there is a 'chance' percent chance of adding a new waiting passenger to that stop
                {
                    Rider newguy = new Rider();
                    route[count].stop.addWaiter(newguy);
                }
                Thread.Sleep(Tool.ReallyRandom()*100);
                count++;
            }
        }
    }
}

 