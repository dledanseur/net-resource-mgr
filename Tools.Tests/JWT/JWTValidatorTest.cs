using System;
using Tools.JWT;
using System.Collections.Generic;
using Xunit;
using System.IO;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;
using System.Security.Cryptography;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Parameters;
using Xunit.Abstractions;
using System.Diagnostics;
using Xunit.Sdk;


namespace Tools.Tests.JWT
{
    public class JWTValidatorTest
    {
        private JWTValidator _jwt_validator;

        private const string PUBLIC_KEY = "TestResources/RSAPublicKey.pem";
        private const string PRIVATE_KEY = "TestResources/RSAPrivateKey.pem";

		private readonly ITestOutputHelper _output;


		private RSAParameters _private_key;

        public JWTValidatorTest(ITestOutputHelper output)
        {
            this._output = output;

            String publicKeyContent = File.ReadAllText("TestResources/RSAPublicKey.pem");

            this._jwt_validator = new JWTValidator(publicKeyContent);

			AsymmetricCipherKeyPair keyPair;

            using (StreamReader sr = new StreamReader(PRIVATE_KEY))
			{
                PemReader pr = new PemReader(sr);
				keyPair = (AsymmetricCipherKeyPair)pr.ReadObject();
			}

            this._private_key = BCUtils.ToRSAParameters((RsaPrivateCrtKeyParameters)keyPair.Private);
        }

        [Fact]
        public void TestValidToken()
        {
            // given
            Dictionary<string, object> payload = new Dictionary<string, object>() {
                {"sub","1234"},
                {"name", "jdoe"},
                {"exp", FutureTimestamp()},
                {"aud", "client_id"},
                {"nonce", "1234"}
            };

            string jwt = GetJwt(payload);

            // when
            Dictionary<string,object> res = _jwt_validator.Validate(jwt, "client_id", "1234");

            // then
            Assert.Equal("1234", res["sub"]);
            Assert.Equal("jdoe", res["name"]);

        }



        [Fact]
		public void TestTokenWithWrongSignature()
		{
			// given
			Dictionary<string, object> payload = new Dictionary<string, object>() {
				{"sub","1234"},
				{"name", "jdoe"},
				{"exp", FutureTimestamp()},
				{"aud", "client_id"},
				{"nonce", "1234"}
			};


			string jwt = GetJwt(payload);

            string changedJWTStr = AlterStringBeforeLastChar(jwt);

            // when (and then)
            Assert.Throws<DecodingException>(() => _jwt_validator.Validate(changedJWTStr, "client_id", "1234"));

		}


        [Fact]
        public void TestExpiredToken() {
			// given
			Dictionary<string, object> payload = new Dictionary<string, object>() {
				{"sub","1234"},
				{"name", "jdoe"},
                {"exp", PastTimestamp()},
				{"aud", "client_id"},
				{"nonce", "1234"}
			};


			string jwt = GetJwt(payload);

			// when (and then)
            Assert.Throws<TokenExpiredException>(() => _jwt_validator.Validate(jwt, "client_id", "1234"));
        }

		[Fact]
		public void TestTokenWithWrongAudience()
		{
			// given
			Dictionary<string, object> payload = new Dictionary<string, object>() {
				{"sub","1234"},
				{"name", "jdoe"},
				{"exp", FutureTimestamp()},
				{"nonce", "1234"},
                {"aud", "wrong_client_id"}
			};


			string jwt = GetJwt(payload);

			// when (and then)
            Assert.Throws<WrongAudienceException>(() => _jwt_validator.Validate(jwt,"client_id", "1234"));
		}

        [Fact]
        public void TestTokenWithWrongNonce() {
			// given
			Dictionary<string, object> payload = new Dictionary<string, object>() {
				{"sub","1234"},
				{"name", "jdoe"},
				{"exp", FutureTimestamp()},
				{"aud", "client_id"},
				{"nonce", "1234"}
			};


			string jwt = GetJwt(payload);

			// when (and then)
            Assert.Throws<WrongNonceException>(() => _jwt_validator.Validate(jwt, "client_id", "5678"));
        }

		private long FutureTimestamp()
		{
			return ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds() + 300;
		}

		private long PastTimestamp()
		{
			return ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds() - 300;
		}

        private string GetJwt(Dictionary<string,object> payload) {
			using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
			{
				rsa.ImportParameters(this._private_key);
                string token = Jose.JWT.Encode(payload, rsa, Jose.JwsAlgorithm.RS256);

                return token;
			}
        }

		private string AlterStringBeforeLastChar(string jwt)
		{
			char[] changedJWT = jwt.ToCharArray();
			changedJWT[changedJWT.Length - 2] = 'A';

			return new string(changedJWT);
		}
    }
}
