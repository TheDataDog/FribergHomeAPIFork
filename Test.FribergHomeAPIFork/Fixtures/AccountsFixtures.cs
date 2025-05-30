using FribergHomeAPI.DTOs;
using FribergHomeAPI.Models;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.FribergHomeAPIFork.Fixtures
{
	public class AccountsFixtures
	{
		public static AccountDTO CreateAccountDTO()
		{
			return new AccountDTO
			{
				FirstName = "Anna",
				LastName = "Svensson",
				Email = "anna@example.com",
				Password = "StrongPass123!",
				PhoneNumber = "0701234567",
				ImageUrl = "http://img.com/img.jpg",
				AgencyId = 1
			};
		}

		public static RealEstateAgent CreateAgent()
		{
			return new RealEstateAgent { Id = 1 };
		}

		public static ApiUser CreateApiUser(AccountDTO dto)
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
		public static RealEstateAgent CreateAgent(AccountDTO dto, string userId)
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

		public static RealEstateAgency CreateAgency()
		{
			return new RealEstateAgency
			{
				Id = 1,
				Name = "TestAgency",
				Presentation = "Presentation",
				LogoUrl = "http://img.com/img.jpg"
			};
		}

		public static Mock<UserManager<ApiUser>> GetMockUserManager()
		{
			var store = new Mock<IUserStore<ApiUser>>();
			return new Mock<UserManager<ApiUser>>(
				store.Object,
				null, null, null, null, null, null, null, null
			);
		}
	}
}
