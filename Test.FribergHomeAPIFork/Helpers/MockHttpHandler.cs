using Azure;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.FribergHomeAPIFork.Helpers
{
	public class MockHttpHandler<T>
	{
		//Success
		internal static Mock<HttpMessageHandler> SetupGetRequest(List<T> response)
		{
			var mockResponse = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
			{
				Content = new StringContent(JsonConvert.SerializeObject(response))
			};

			mockResponse.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

			var mockHandler = new Mock<HttpMessageHandler>();
			mockHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
				ItExpr.IsAny<CancellationToken>()).ReturnsAsync(mockResponse);

			return mockHandler;
		}

		//NotFound
		internal static Mock<HttpMessageHandler> SetupReturnNotFound()
		{
			var mockResponse = new HttpResponseMessage(System.Net.HttpStatusCode.NotFound)
			{
				Content = new StringContent("")
			};

			mockResponse.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

			var mockHandler = new Mock<HttpMessageHandler>();
			mockHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
				ItExpr.IsAny<CancellationToken>()).ReturnsAsync(mockResponse);

			return mockHandler;
		}
	}
}
