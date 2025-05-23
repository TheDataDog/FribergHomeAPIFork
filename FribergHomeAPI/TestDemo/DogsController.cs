using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FribergHomeAPI.TestDemo
{
	[Route("api/[controller]")]
	[ApiController]
	public class DogsController : ControllerBase
	{
		private readonly IDogService dogService;

		public DogsController(IDogService dogService)
		{
			this.dogService = dogService;
		}

		[HttpGet(Name = "GetDogs")]
		public async Task<IActionResult> Get()
		{
			var dogs = await dogService.GetAllDogs();

			if (dogs.Any())
			{
				return Ok(dogs);
			}
			return NotFound();
		}
	}
}
