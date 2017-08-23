using System;
using Newtonsoft.Json;

namespace Services.Tests.Services.Gitlab
{
    public class GetTokenResponse 
    {
		[JsonProperty("access_token")]
		public string AccessToken { get; set; }

		[JsonProperty("token_type")]
		public string TokenType { get; set; }

		[JsonProperty("expires_in")]
		public long ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("id_token")]
        public string IdToken { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
    }

}
