
using Microsoft.EntityFrameworkCore.Storage;

namespace FribergHomeAPI.Data
{
	public class EfCoreTransaction : ITransaction
	{
		private readonly IDbContextTransaction transaction;

		public EfCoreTransaction(IDbContextTransaction transaction)
        {
			this.transaction = transaction;
		}
        public Task CommitAsync() => transaction.CommitAsync();

		public ValueTask DisposeAsync() => transaction.DisposeAsync();

		public Task RollbackAsync() => transaction.RollbackAsync();
	}
}
