using System;
using System.Text;
namespace Tools.Random.Impl
{
    public class RandomObjects : IRandomObjects
    {
        private IRawRandom _raw_random;

        public RandomObjects() : this(new SecureRawRandom()) {}

        public RandomObjects(IRawRandom rnd) 
        {
            this._raw_random = rnd;
        }

        public string RandomHexString(int length)
        {
            byte[] rawData = _raw_random.GetBytes(length);

            return ByteArrayToString(rawData);


        }

		private static string ByteArrayToString(byte[] ba)
		{
			StringBuilder hex = new StringBuilder(ba.Length * 2);

            foreach (byte b in ba)
            {
                hex.AppendFormat("{0:x2}", b);
            }
			return hex.ToString();
		}
    }
}
