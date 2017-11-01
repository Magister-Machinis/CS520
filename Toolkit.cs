/* 
 * https://msdn.microsoft.com/en-us/library/hb7xxkfx(v=vs.110).aspx for pinging entropy gen 
 */
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Cryptography;

namespace GenericTools
{
    public class Toolkit
    {
        bool Entropy;
        bool Type;
        int seed;
        int multiplier;
        int modulo;
        int increment;
        int arrival;
        public Toolkit(bool entropy = false, bool linearorexpo = false) // instance of generator is started with either in built crypto PRG or homebrewed entropy source
        {
            Entropy = entropy;
            Type = linearorexpo; // if false than homebrewed algorithm uses linear distribution, else exponential distribution
            if (Entropy == true)
            {
                seed = SeedGen();
                multiplier = SeedGen();
                modulo = SeedGen();
                increment = SeedGen();
                arrival = SeedGen();
            }
        }

        public bool Eventgenerator(int freq) // using built in cryptographic random # generator to produce true returns 'freq' percentage of the time
        {
            int randomnum;
            if (Entropy == false)
            {
                randomnum = ReallyRandom();
            }
            else if(Type == false)
            {
                randomnum = LinearDis();
            }
            else
            {
                randomnum = ExpoDis();
            }

            if ((randomnum % 100) <= freq)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int ReallyRandom()
        {
            RNGCryptoServiceProvider gibberish = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[4];
            gibberish.GetBytes(buffer);
            int randomnum = BitConverter.ToInt32(buffer, 0);
            return Math.Abs(randomnum);
        }

        int LinearDis()
        {

            seed = Convert.ToInt32(((seed * multiplier + increment) % modulo)% 2147483647);
            return seed;
        }

        int ExpoDis()
        {
            seed = -(1 / arrival) * Convert.ToInt32(Math.Log((Convert.ToDouble(LinearDis() / modulo)))% 2147483647);
            return seed;
            
        }
        
        int SeedGen() //generates a random 
        {
            long initialnum = 1 ;
            Ping Sender = new Ping();
            while (initialnum < 2147483647) // multiply until passing a sufficiently large number
            {
                IPAddress IP1 = ValidIP();
                PingReply reply = Sender.Send(IP1);
                initialnum = initialnum * reply.RoundtripTime;
            }
            initialnum = initialnum % 2147483647; //enforce an upper bound on the numbers to keep it sane and improve distribution a bit

            return Convert.ToInt32(initialnum);

        }

        IPAddress ValidIP() // returns a valid non-reserved IP address
        {
            int oct1 = (ReallyRandom() % 255);
            int oct2 = (ReallyRandom() % 255);
            int oct3 = (ReallyRandom() % 255);
            int oct4 = (ReallyRandom() % 255);
            string IPstring;
            IPAddress newaddr;
            
            while(oct1 == 10 | oct1==127)
            {
                oct1 = (ReallyRandom() % 255);
            }
            while((oct1 == 172 & (oct2 < 32 & oct2 > 15)) | (oct1 == 192 & oct2 == 168))
            {
                oct1 = (ReallyRandom() % 255);
                oct2 = (ReallyRandom() % 255);
            }
            IPstring = oct1 + "." + oct2 + "." + oct3 + "." + oct4;
            IPAddress.TryParse(IPstring, out newaddr);
            
            return newaddr;
        }
    }
}
 