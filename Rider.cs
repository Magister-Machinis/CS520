using GenericTools;
namespace SimulationCore
{
    class Rider //generic holder for riders and whether they are on a buss or waiting
    {
        bool riding = false;
        Toolkit Tool = new Toolkit();
        int attentionspan = (Tool.ReallyRandom()) % 100; //how likely this user is to get on or off a buss

        public bool getState()
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
    }
}

 