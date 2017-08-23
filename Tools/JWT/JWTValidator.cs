using System;
using System.IO;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Crypto.Parameters;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using Jose;

namespace Tools.JWT
{
    public class JWTValidator : IJWTValidator
    {
        public RSAParameters _public_key;

        public JWTValidator(string secret) {
			RsaKeyParameters keyPair;

            MemoryStream secretStream = new MemoryStream(Encoding.ASCII.GetBytes(secret));

            using (StreamReader sr = new StreamReader(secretStream))
			{
				PemReader pr = new PemReader(sr);
                keyPair = (RsaKeyParameters)pr.ReadObject();
			}

            this._public_key = BCUtils.ToRSAParameters(keyPair);
            //this._public_key = ToRSAParameters((RsaKeyParameters)keyPair.Public);

        }

        public Dictionary<string,object> Validate(string jwt, string audience, string nonce) {

			using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
			{
                rsa.ImportParameters(this._public_key);

                try
                {
                    
                    string json = Jose.JWT.Decode(jwt, rsa, Jose.JwsAlgorithm.RS256);

                    Dictionary<string,object> res = JsonConvert.DeserializeObject<Dictionary<string,object>>(json);

                    AsssertExpiry(res);

                    AssertAudience(audience, res);

                    AssertNonce(nonce, res);

                    return res;

                }
                catch (JoseException e) {
                    throw new DecodingException("Wrong JWT value",e);
                }

			}
        }

        private void AssertNonce(string nonce, Dictionary<string, object> res)
        {
			string non = (string)res["nonce"];
			if (non != nonce)
			{
                throw new WrongNonceException("Wrong nonce");
			}
        }

        private void AsssertExpiry(Dictionary<string, object> res)
        {
			long time = (long)res["exp"];

			DateTime foo = DateTime.Now;
			long unixTime = ((DateTimeOffset)foo).ToUnixTimeSeconds();

			if (time < unixTime)
			{
                throw new TokenExpiredException("Token expired");
			}
			
        }

        private void AssertAudience(string audience, Dictionary<string, object> res)
        {
			string aud = (string)res["aud"];
			if (aud != audience)
			{
                throw new WrongAudienceException("Wrong audience");
			}
			
        }



    }
}
