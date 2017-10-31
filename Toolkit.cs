using System;
using System.Security.Cryptography;

namespace GenericTools
{
    public class Toolkit
    {
        bool Entropy;
        bool Type;
        int seed1;
        int seed2;
        public Toolkit(bool entropy = false, bool linearorexpo = false) // instance of generator is started with either in built crypto PRG or homebrewed entropy source
        {
            Entropy = entropy;
            Type = linearorexpo;
        }

        public bool Eventgenerator(int freq) // using built in cryptographic random # generator to produce true returns 'freq' percentage of the time
        {
            int randomnum = ReallyRandom();
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
    }
}
 