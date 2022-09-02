using System.Data;

namespace IDXWeb.Repositories
{
    public abstract class RepositoryBase
    {
        protected IDbTransaction Transaction { get; }
        protected IDbConnection Connection { get { return Transaction.Connection; } }

        public RepositoryBase(IDbTransaction transaction)
        {
            Transaction = transaction;
        }
    }
}