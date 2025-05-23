namespace FribergHomeAPI.TestDemo
{
	public interface IDogService
	{
		Task<List<Dog>?> GetAllDogs();
	}
}
