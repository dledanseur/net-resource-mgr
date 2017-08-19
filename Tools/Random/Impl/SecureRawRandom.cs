using System;
using System.Security.Cryptography;
namespace Tools.Random.Impl
{
    
    public class SecureRawRandom: IRawRandom
    {

        private RandomNumberGenerator rng = RandomNumberGenerator.Create();

        public byte[] GetBytes(int length) {
            byte[] b = new byte[length];

            rng.GetBytes(b);

            return b;
        }
    }
}
