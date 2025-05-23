using FribergHomeAPI.Data;
using FribergHomeAPI.Data.Repositories;
using FribergHomeAPI.DTOs;
using FribergHomeAPI.Models;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Test.FribergHomeAPIFork
{
	public class TestRegister
	{
		private readonly Mock<UserManager<ApiUser>> mockUserManager;
		private readonly Mock<IRealEstateAgentRepository> mockAgentRepository;
		private readonly Mock<IRealEstateAgencyRepository> mockAgencyRepository;
		private readonly Mock<ITransactionManager> mockTransactionManager;

		public TestRegister()
        {
			mockUserManager = GetMockUserManager();
			mockAgentRepository = new Mock<IRealEstateAgentRepository>();
			mockAgencyRepository = new Mock<IRealEstateAgencyRepository>();
			mockTransactionManager = new Mock<ITransactionManager>();            
        }

		private ApiUser CreateApiUser(AccountDTO dto)
		{
			return new ApiUser
			{
				FirstName = dto.FirstName,
				LastName = dto.LastName,
				UserName = dto.Email,
				NormalizedUserName = dto.Email.ToUpper(),
				NormalizedEmail = dto.Email.ToUpper(),
				Email = dto.Email
			};
		}
		private RealEstateAgent CreateAgent(AccountDTO dto, string userId)
		{
			return new RealEstateAgent
			{
				FirstName = dto.FirstName,
				LastName = dto.LastName,
				Email = dto.Email,
				PhoneNumber = dto.PhoneNumber,
				ImageUrl = dto.ImageUrl,
				ApiUserId = userId
			};
		}

		private Mock<UserManager<ApiUser>> GetMockUserManager()
		{
			var store = new Mock<IUserStore<ApiUser>>();
			return new Mock<UserManager<ApiUser>>(
				store.Object,
				null, null, null, null, null, null, null, null
			);
		}

		[Fact]
		public void Test1()
		{
			//Arrange
			var dto = new AccountDTO
			{
				FirstName = "Anna",
				LastName = "Svensson",
				Email = "anna@example.com",
				Password = "StrongP@ssword123",
				PhoneNumber = "0701234567",
				ImageUrl = "http://img.com/anna.jpg",
				AgencyId = 1
			};

			mockUserManager.Setup(um => um.CreateAsync(It.IsAny<ApiUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success );

			//Act

			//Assert

		}
	}
}