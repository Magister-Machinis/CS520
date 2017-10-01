using System;
using System.Security.Cryptography;

namespace SimulationCore
{
    public class Toolkit
    {
        public bool Eventgenerator(int freq) // using built in cryptographic random # generator to produce true returns 'freq' percentage of the time
        {
            RNGCryptoServiceProvider gibberish = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[4];
            gibberish.GetBytes(buffer);
            int randomnum = BitConverter.ToInt32(buffer, 0);
            if ((randomnum % 100) <= freq)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
 