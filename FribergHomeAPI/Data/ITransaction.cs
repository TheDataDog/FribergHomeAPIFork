namespace FribergHomeAPI.Data
{
	public interface ITransaction : IAsyncDisposable
	{
		Task CommitAsync();
		Task RollbackAsync();
	}
}
