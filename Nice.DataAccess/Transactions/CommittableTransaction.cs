using System.Data;

namespace Nice.DataAccess.Transactions
{
    public class CommittableTransaction : Transaction
    {
        public CommittableTransaction(IDbTransaction dbTransaction)
        {
            DbTransaction = dbTransaction;
        }
        public void Commit()
        {
            DbTransaction.Commit();
        }
    }
}
