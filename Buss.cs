/*
 * information sources:
 * msdn.microsoft.com
 * tutorialspoint.com
 * 
 * Hosted at:
 * https://github.com/Magister-Machinis/CS520 (amongst my other efforts)
 * 
 * John Fromholtz
 * CS520 Fall 2017
 */


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
 