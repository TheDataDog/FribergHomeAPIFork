using Microsoft.Extensions.Options;
using System.Net;

namespace FribergHomeAPI.TestDemo
{
	public class DogService : IDogService
	{
		private readonly HttpClient httpClient;
		private readonly ApiServiceConfig apiServiceConfig;

		public DogService(HttpClient httpClient, IOptions<ApiServiceConfig> config)
		{
			this.httpClient = httpClient;
			apiServiceConfig = config.Value;
		}
		public async Task<List<Dog>?> GetAllDogs()
		{
			var response = await httpClient.GetAsync(apiServiceConfig.Url);
			if (response.StatusCode == HttpStatusCode.NotFound)
			{
				return new List<Dog>();
			}

			if(response.StatusCode == HttpStatusCode.Unauthorized)
			{
				return null;
			}

			var dogs = await response.Content.ReadFromJsonAsync<List<Dog>>();
			return dogs;
		}
	}
}
