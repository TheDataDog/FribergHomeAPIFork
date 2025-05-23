using FribergHomeAPI.TestDemo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.FribergHomeAPIFork.Fixtures
{
	public class DogsFixtures
	{
		public static List<Dog> GetDogs() => new()
		{
			new Dog()
			{
				id = 1,
				Name = "Bamse",
				Breed = "Golden"
			},

			new Dog()
			{
				id = 2,
				Name = "Bullen",
				Breed = "Schäfer"
			}
		};
	}
}
