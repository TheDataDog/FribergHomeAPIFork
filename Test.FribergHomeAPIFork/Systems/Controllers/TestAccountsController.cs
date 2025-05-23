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

namespace Test.FribergHomeAPIFork.Systems.Controllers
{
	public class TestAccountsController
	{
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
	}
}
