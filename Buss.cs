namespace SimulationCore
{
    class Buss //generic holder class for the busses
    {
        int passenger;
        public int getPassenger()
        {
            return passenger;
        }

        public void addPassenger(int adder)
        {
            passenger = passenger + adder;
        }

        public void setPassener(int newnum)
        {
            passenger = newnum;
        }
    }
}
 