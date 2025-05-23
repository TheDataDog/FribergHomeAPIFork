
namespace FribergHomeAPI.Data
{
	public class EfCoreTransactionManager : ITransactionManager
	{
		private readonly ApplicationDbContext dbContext;

		public EfCoreTransactionManager(ApplicationDbContext dbContext)
        {
			this.dbContext = dbContext;
		}
        public async Task<ITransaction> BeginAsync()
		{
			var transaction = await dbContext.Database.BeginTransactionAsync();
			return new EfCoreTransaction(transaction);
		}
	}
}
