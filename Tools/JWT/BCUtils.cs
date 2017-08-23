using System;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Parameters;
namespace Tools.JWT
{
    public static class BCUtils
    {
		public static RSAParameters ToRSAParameters(RsaKeyParameters rsaKey)
		{
			RSAParameters rp = new RSAParameters();
			rp.Modulus = rsaKey.Modulus.ToByteArrayUnsigned();
			if (rsaKey.IsPrivate)
				rp.D = rsaKey.Exponent.ToByteArrayUnsigned();
			else
				rp.Exponent = rsaKey.Exponent.ToByteArrayUnsigned();
			return rp;
		}
    }
}
