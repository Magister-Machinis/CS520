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


namespace SimulationCore
{
    class Controller //generic holder for inter-process flag values and such
    {
        bool StartStop;

        public Controller()
        {
            StartStop = false;
        }
                
        public bool getState()
        {
            return StartStop;
        }

        public void toggleState()
        {
            if (StartStop == false)
            {
                StartStop = true;
            }
            else
            {
                StartStop = false;
            }
        }
    }
}
 