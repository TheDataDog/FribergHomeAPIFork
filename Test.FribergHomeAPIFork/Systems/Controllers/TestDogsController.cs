using FluentAssertions;
using FribergHomeAPI.TestDemo;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.FribergHomeAPIFork.Fixtures;

namespace Test.FribergHomeAPIFork.Systems.Controllers
{
	public class TestDogsController
	{
		[Fact]
		public async Task Get_OnSuccess_ReturnStatusCode200()
		{
			//Arrange
			var mockDogService = new Mock<IDogService>();
			mockDogService.Setup(ds => ds.GetAllDogs()).ReturnsAsync(DogsFixtures.GetDogs);

			var dogsController = new DogsController(mockDogService.Object);

			//Act
			var result = (OkObjectResult) await dogsController.Get();

			//Assert
			result.StatusCode.Should().Be(200);
		}

		[Fact]
		public async Task Get_OnSuccess_InvokeService()
		{
			//Arrange
			var mockDogService = new Mock<IDogService>();
			mockDogService.Setup(ds => ds.GetAllDogs()).ReturnsAsync(DogsFixtures.GetDogs);

			var dogsController = new DogsController(mockDogService.Object);

			//Act
			var result = (OkObjectResult) await dogsController.Get();

			//Assert
			mockDogService.Verify(ds => ds.GetAllDogs(), Times.Once());

		}

		[Fact]
		public async Task Get_OnSucces_ReturnListOfDogs()
		{
			//Arrange
			var mockDogService = new Mock<IDogService>();
			mockDogService.Setup(ds => ds.GetAllDogs()).ReturnsAsync(DogsFixtures.GetDogs);

			var dogsController = new DogsController(mockDogService.Object);

			//Act
			var result = (OkObjectResult) await dogsController.Get();

			//Assert
			result.Should().BeOfType<OkObjectResult>();

			result.Value.Should().BeOfType<List<Dog>>();
		}

		[Fact]
		public async Task Get_OnNoDogs_ReturnNotFound()
		{
			//Arrange
			var mockDogService = new Mock<IDogService>();
			mockDogService.Setup(ds => ds.GetAllDogs()).ReturnsAsync(new List<Dog>());

			var dogsController = new DogsController(mockDogService.Object);

			//Act
			var result = (NotFoundResult) await dogsController.Get();

			//Assert
			result.Should().BeOfType<NotFoundResult>();

		}
	}
}
