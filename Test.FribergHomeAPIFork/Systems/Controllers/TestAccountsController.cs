using AutoMapper;
using FluentAssertions;
using FribergHomeAPI.Controllers;
using FribergHomeAPI.DTOs;
using FribergHomeAPI.Models;
using FribergHomeAPI.Results;
using FribergHomeAPI.Services;
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
	public class TestAccountsController
	{
		private readonly Mock<IMapper> mockMapper;
		private readonly Mock<IAccountService> mockAccountService;
		private readonly AccountsController accountsController;

		public TestAccountsController()
        {
			mockMapper = new Mock<IMapper>();
            mockAccountService = new Mock<IAccountService>();
			accountsController = new AccountsController(mockMapper.Object, mockAccountService.Object);
        }

        [Fact]
		public async Task Register_OnSuccess_ReturnStatusCode201()
		{
			//Arrange
			var dto = new AccountDTO
			{
				FirstName = "Anna",
				LastName = "Svensson",
				Email = "anna@example.com",
				Password = "StrongPass123!",
				PhoneNumber = "0701234567",
				ImageUrl = "http://img.com/img.jpg",
				AgencyId = 1
			};

			var agent = new RealEstateAgent {Id = 1};

			var mockMapper = new Mock<IMapper>();
			mockMapper.Setup(m => m.Map<AgentCreatedDTO>(It.IsAny<RealEstateAgent>())).Returns(new AgentCreatedDTO { Id = 1 });

			var mockAccountService = new Mock<IAccountService>();
			mockAccountService.Setup(s => s.RegisterAsync(dto)).ReturnsAsync(ServiceResult<RealEstateAgent>.SuccessResult(agent));

			var accountsController = new AccountsController(mockMapper.Object, mockAccountService.Object);

			//Act

			var result = (CreatedResult) await accountsController.Register(dto); //casta till ett CreatedResult annars returners IActionResult

			//Assert
			result.StatusCode.Should().Be(201);

		}

		[Fact]
		public async Task Register_OnSucces_InvokeService()
		{
			//Arrange
			var accountDTO = AccountsFixtures.CreateAccountDTO();
			var agent = AccountsFixtures.CreateAgent();

			mockAccountService.Setup(s => s.RegisterAsync(accountDTO)).ReturnsAsync(ServiceResult<RealEstateAgent>.SuccessResult(agent));
			mockMapper.Setup(m => m.Map<AgentCreatedDTO>(It.IsAny<RealEstateAgent>())).Returns(new AgentCreatedDTO {Id = 1});

			//Act

			await accountsController.Register(accountDTO);


			//Assert
			mockAccountService.Verify(s => s.RegisterAsync(accountDTO), Times.Once);
		}

		[Fact]
		public async Task Register_OnNotSucces_ReturnsBadRequest()
		{
			//Arrange
			var accountDTO = AccountsFixtures.CreateAccountDTO();
			mockAccountService.Setup(s => s.RegisterAsync(accountDTO)).ReturnsAsync(ServiceResult<RealEstateAgent>.Failure("Failure"));

			//Act
			var result = (BadRequestObjectResult) await accountsController.Register(accountDTO);

			//Assert
			result.StatusCode.Should().Be(400);

		}

		[Fact]
		public async Task Register_OnSuccess_ReturnsStringUriAndObjectValue()
		{
			//Arrange
			var accountDTO = AccountsFixtures.CreateAccountDTO();
			var agent = AccountsFixtures.CreateAgent();
			var expectedDTO = new AgentCreatedDTO { Id = 1 };

			mockAccountService.Setup(s => s.RegisterAsync(accountDTO)).ReturnsAsync(ServiceResult<RealEstateAgent>.SuccessResult(agent));
			mockMapper.Setup(m => m.Map<AgentCreatedDTO>(It.IsAny<RealEstateAgent>())).Returns(expectedDTO);

			//Act
			var result = (CreatedResult)await accountsController.Register(accountDTO);

			//Assert
			result.Should().NotBeNull();
			result.Location.Should().Be("/api/RealEstateAgents/1");
			result.Value.Should().BeEquivalentTo(expectedDTO);
		}
	}
}
