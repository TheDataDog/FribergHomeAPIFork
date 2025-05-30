using AutoMapper;
using FribergHomeAPI.Controllers;
using FribergHomeAPI.Data;
using FribergHomeAPI.Data.Repositories;
using FribergHomeAPI.Mappings;
using FribergHomeAPI.Models;
using FribergHomeAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.FribergHomeAPIFork.Fixtures;
using Test.FribergHomeAPIFork.Helpers;

namespace Test.FribergHomeAPIFork.Systems.Controllers
{
	public class TestAccountsController_Intergration
	{
		private readonly IMapper mapper;
		private readonly Mock<ITransactionManager> mockTransactionManager;
		private readonly Mock<ITransaction> mockTransaction;
		private readonly ServiceProvider serviceProvider;
		private AccountService accountService;

		public TestAccountsController_Intergration()
        {
            serviceProvider = ConfigServiceProvider.UseBothInMemoryDatabaseAndIdentity();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            mapper = config.CreateMapper();

			mockTransactionManager = new Mock<ITransactionManager>();
			mockTransaction = new Mock<ITransaction>();
		}

		[Fact]
		public async Task Register_OnSuccess_CreatesUserAgentApplicationAndAssignUserRole()
		{
			//Arrange
			using var scope = serviceProvider.CreateScope();
			var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApiUser>>();
			var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
			var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

			foreach(var role in new[] { "User", "Agent" })
			{
				if(!await roleManager.RoleExistsAsync(role))
				{
					await roleManager.CreateAsync(new IdentityRole(role));
				}
			}

			var agentRepository = new RealEstateAgentRepository(dbContext);
			var agencyRepository = new RealEstateAgencyRepository(dbContext);

			var agency = AccountsFixtures.CreateAgency();
			await agencyRepository.AddAsync(agency);

			mockTransaction.Setup(t => t.CommitAsync()).Returns(Task.CompletedTask);
			mockTransaction.Setup(t => t.RollbackAsync()).Returns(Task.CompletedTask);
			mockTransactionManager.Setup(tm => tm.BeginAsync()).ReturnsAsync(mockTransaction.Object);

			accountService = new AccountService(userManager, 
												agentRepository,
												dbContext, 
												new ConfigurationBuilder().Build(),
												agencyRepository,
												mapper,
												mockTransactionManager.Object);

			var accountsController = new AccountsController(mapper, accountService);
			var accountDTO = AccountsFixtures.CreateAccountDTO();

			//Act
			var result =  await accountsController.Register(accountDTO);

			//Assert
			Assert.IsType<CreatedResult>(result);

			var createdUser = await userManager.FindByEmailAsync(accountDTO.Email);
			Assert.NotNull(createdUser);

			var isInRole = await userManager.IsInRoleAsync(createdUser, "User");
			Assert.False(isInRole);

			var agent = dbContext.Agents.SingleOrDefault(a => a.Email.Equals(accountDTO.Email));
			Assert.NotNull(agent);

			var application = dbContext.Applications.SingleOrDefault(a => a.AgentId.Equals(agent.Id));
			Assert.NotNull(application);

			mockTransaction.Verify(t => t.CommitAsync(), Times.Once);


		}
    }
}
