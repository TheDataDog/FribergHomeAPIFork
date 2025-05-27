using FluentAssertions;
using FribergHomeAPI.TestDemo;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.FribergHomeAPIFork.Fixtures;
using Test.FribergHomeAPIFork.Helpers;

namespace Test.FribergHomeAPIFork.Systems.Services
{
	public class TestDogService
	{
		[Fact]
		public async Task GetAllDogs_OnInvoked_HttpGet()
		{
			//Arrange
			var url = "https://mywebsite.com/api/v1/dogs";
			var response = DogsFixtures.GetDogs();
			var mockHandler = MockHttpHandler<Dog>.SetupGetRequest(response);
			var httpClient = new HttpClient(mockHandler.Object);
			var config = Options.Create(new ApiServiceConfig()
			{
				Url = url,
			});			
			var fanService = new DogService(httpClient, config);

			//Act
			await fanService.GetAllDogs();

			//Assert
			mockHandler.Protected().Verify("SendAsync", Times.Once(),
											ItExpr.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Get && r.RequestUri.ToString() == url),
											ItExpr.IsAny<CancellationToken>());
		}

		[Fact]
		public async Task GetAllDogs_OnInvoked_GetListOfDogs()
		{
			//Arrange
			var url = "https://mywebsite.com/api/v1/dogs";
			var response = DogsFixtures.GetDogs();
			var mockHandler = MockHttpHandler<Dog>.SetupGetRequest(response);
			var httpClient = new HttpClient(mockHandler.Object);
			var config = Options.Create(new ApiServiceConfig()
			{
				Url = url,
			});
			var fanService = new DogService(httpClient, config);

			//Act
			var result = await fanService.GetAllDogs();

			//Assert
			result.Should().BeOfType<List<Dog>>();
		}

		[Fact]
		public async Task GetAllDogs_OnInvoked_ReturnEmptyList()
		{
			//Arrange
			var url = "https://mywebsite.com/api/v1/dogs";
			var mockHandler = MockHttpHandler<Dog>.SetupReturnNotFound();
			var httpClient = new HttpClient(mockHandler.Object);
			var config = Options.Create(new ApiServiceConfig()
			{
				Url = url,
			});
			var fanService = new DogService(httpClient, config);

			//Act
			var result = await fanService.GetAllDogs();

			//Assert
			result.Count.Should().Be(0);
		}
	}
}
