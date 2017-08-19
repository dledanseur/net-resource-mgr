using System;
using Xunit;
using Moq;
using Tools.HttpHandler.Impl;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace Tools.HttpHandler.Tests
{
    public class RestClientImplTest
    {

        Mock<IHttpHandler> _httpHandlerMock = new Mock<IHttpHandler>();
        private RestClientImpl _restClient;

        public RestClientImplTest() {
            _restClient = new RestClientImpl(_httpHandlerMock.Object);
        }

        [Fact]
        public async Task TestGetWithJsonResult()
        {
            // given
            HttpResponseMessage msg = JsonMessageWithContent("{'key': 'value'}"
                                                            );
            _httpHandlerMock.Setup(r => r.Get("/test/api")).Returns(Task.FromResult(msg));

            // when
            RestResponse<Dictionary<string, string>> result = await _restClient.Get<Dictionary<string, string>>("/test/api");

			// then
			Dictionary<string, string> expectedResult = DictionaryWithKeysAndValues("key", "value");
			
            Assert.Equal(expectedResult, result.Body);
            Assert.NotNull(result.Headers);
            Assert.Equal(1, result.Headers.Count);
            Assert.Equal("testheader_value", result.Headers["testheader"][0]);

            _httpHandlerMock.VerifyAll();
        }

		[Fact]
		public async Task TestGetWithQueryParams()
		{
            // given
			HttpResponseMessage msg = JsonMessageWithContent("{'key': 'value'}");

			Dictionary<string, string> queryParams = DictionaryWithKeysAndValues("key", "value",
                "key2", "value2");

            // when
            _httpHandlerMock.Setup(r => r.Get(It.IsAny<String>())).Returns(Task.FromResult(msg));

            // then
            RestResponse<Dictionary<string, string>> result = await _restClient.Get<Dictionary<string, string>>("/test/api", queryParams);

            _httpHandlerMock.Verify(r => r.Get("/test/api?key=value&key2=value2"));
		}

		[Fact]
		public async Task TestPostWithJsonResult()
		{
            // given
			HttpResponseMessage msg = JsonMessageWithContent("{'keyRes': 'valRes'}");

            string calledUrl = null;
            HttpContent resultingContent = null;

            _httpHandlerMock.Setup(r => r.Post(It.IsAny<String>(), It.IsAny<HttpContent>()))
                            .Returns(Task.FromResult(msg))
                            .Callback<String, HttpContent>((url, httpContent) =>
                            {
                                calledUrl = url;
                                resultingContent = httpContent;
                            });


            Dictionary<string, string> content = DictionaryWithKeysAndValues("key", "value");

            // when
            RestResponse<Dictionary<string, string>> result = await _restClient.Post<Dictionary<string, string>>("/test/api",content);

            // then
            Dictionary<string, string> expectedResult = DictionaryWithKeysAndValues("keyRes", "valRes");
			

            Assert.Equal(expectedResult, result.Body);
            Assert.Equal("/test/api", calledUrl);
			Assert.NotNull(result.Headers);
            Assert.Equal(1,result.Headers.Count);
			Assert.Equal("testheader_value", result.Headers["testheader"][0]);
            Assert.Equal("{\"key\":\"value\"}", resultingContent.ReadAsStringAsync().Result);


		}

		
		private HttpResponseMessage JsonMessageWithContent(String content)
		{
			HttpResponseMessage msg = new HttpResponseMessage();

			msg.Content = new StringContent(content, Encoding.UTF8, "application/json");
            msg.Headers.Add("testheader", new[] { "testheader_value"});

			return msg;
		}

        private Dictionary<string,string> DictionaryWithKeysAndValues(params string[] keysAndValues) {
            Dictionary<string,string> dic = new Dictionary<string, string>();

            for (int i = 0; i < keysAndValues.Length; i+= 2) {
                dic.Add(keysAndValues[i],keysAndValues[i+1]);
            }

            return dic;
        }
    }

}
