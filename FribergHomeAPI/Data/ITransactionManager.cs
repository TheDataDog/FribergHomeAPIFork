using Microsoft.EntityFrameworkCore.Storage;

namespace FribergHomeAPI.Data
{
	public interface ITransactionManager
	{
		Task<ITransaction> BeginAsync();
	}
}
