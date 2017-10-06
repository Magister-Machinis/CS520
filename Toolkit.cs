using System;
using System.Security.Cryptography;

namespace GenericTools
{
    public class Toolkit
    {
        public Toolkit()
        {}
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
            return randomnum;
        }
    }
}
 