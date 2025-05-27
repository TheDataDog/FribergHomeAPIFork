using FluentAssertions;
using FribergHomeAPI.Constants;
using FribergHomeAPI.Data;
using FribergHomeAPI.Data.Repositories;
using FribergHomeAPI.DTOs;
using FribergHomeAPI.Models;
using FribergHomeAPI.Results;
using FribergHomeAPI.Services;
using Microsoft.AspNetCore.Identity;
using Moq;
using Test.FribergHomeAPIFork.Fixtures;

namespace Test.FribergHomeAPIFork.Systems.Services
{
    public class TestAccountService
    {
        private readonly Mock<UserManager<ApiUser>> mockUserManager;
        private readonly Mock<IRealEstateAgentRepository> mockAgentRepository;
        private readonly Mock<IRealEstateAgencyRepository> mockAgencyRepository;
        private readonly Mock<ITransactionManager> mockTransactionManager;
		private readonly Mock<ITransaction> mockTransaction;

		public TestAccountService()
        {
            mockUserManager = AccountsFixtures.GetMockUserManager();
            mockAgentRepository = new Mock<IRealEstateAgentRepository>();
            mockAgencyRepository = new Mock<IRealEstateAgencyRepository>();
            mockTransactionManager = new Mock<ITransactionManager>();
            mockTransaction = new Mock<ITransaction>();
        }

        [Fact]
        public async void RegisterAsync_OnSuccess_CheckAll()
        {
            //Arrange
            var accountDTO = AccountsFixtures.CreateAccountDTO();
            var apiUser = AccountsFixtures.CreateApiUser(accountDTO);
            var agent = AccountsFixtures.CreateAgent(accountDTO, apiUser.Id);

            mockUserManager.Setup(um => um.CreateAsync(It.IsAny<ApiUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            mockUserManager.Setup(um => um.AddToRoleAsync(It.IsAny<ApiUser>(), ApiRoles.User)).ReturnsAsync(IdentityResult.Success);
            mockAgentRepository.Setup(ar => ar.AddAsync(It.IsAny<RealEstateAgent>())).ReturnsAsync(agent);
            mockAgencyRepository.Setup(ar => ar.AddApplication(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);
            mockTransactionManager.Setup(tm => tm.BeginAsync()).ReturnsAsync(mockTransaction.Object);
            mockTransaction.Setup(t => t.CommitAsync()).Returns(Task.CompletedTask);
            mockTransaction.Setup(t => t.RollbackAsync()).Returns(Task.CompletedTask);

            var accountService = new AccountService(
                                        mockUserManager.Object, 
                                        mockAgentRepository.Object,
                                        null,
                                        null,
                                        mockAgencyRepository.Object, 
                                        null,
                                        mockTransactionManager.Object);

            //Act
            var result = await accountService.RegisterAsync(accountDTO);

            //Assert
            Assert.NotNull(result);
            result.Should().BeOfType<ServiceResult<RealEstateAgent>>();
            result.Success.Should().BeTrue();
            result.Errors.Should().BeEmpty();
            mockTransactionManager.Verify(tm => tm.BeginAsync(), Times.Once);
            mockTransaction.Verify(t => t.CommitAsync(), Times.Once);
            mockTransaction.Verify(t => t.RollbackAsync(), Times.Never);
            mockUserManager.Verify(um => um.CreateAsync(It.Is<ApiUser>(u => u.Email == accountDTO.Email), It.Is<string>(s => s.ToString() ==accountDTO.Password)), Times.Once());
            mockUserManager.Verify(um => um.AddToRoleAsync(It.IsAny<ApiUser>(), ApiRoles.User), Times.Once);
            mockAgentRepository.Verify(ar => ar.AddAsync(It.Is<RealEstateAgent>(a => a.Email == accountDTO.Email)), Times.Once);
            mockAgencyRepository.Verify(ar => ar.AddApplication(agent.Id, accountDTO.AgencyId), Times.Once);


		}
    }
}