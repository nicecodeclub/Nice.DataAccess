using System;
using System.Data;

namespace Nice.DataAccess.Transactions
{
    public class TransactionScope : IDisposable
    {
        private Transaction transaction = Transaction.Current;
        //private Transaction _expectedCurrent;
        //private CommittableTransaction _committableTransaction;
        //private DependentTransaction _dependentTransaction;
        public TransactionScope(string connStriKey = DataUtil.DefaultConnStringKey, IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            if (transaction == null)
            {
                DataHelper dataHelper = DataUtil.GetDataHelper(connStriKey);
                IDbConnection connection = dataHelper.GetConnection();
                connection.Open();
                IDbTransaction dbTransaction = connection.BeginTransaction(isolationLevel);
                Transaction.Current = new CommittableTransaction(dbTransaction);
            }
            else
            {
                Transaction.Current = transaction.DependentClone();
            }
        }
        private bool isCompleted;
        public bool IsCompleted
        {
            get { return isCompleted; }
        }
        public void Complete()
        {
            isCompleted = true;
        }
        public void Dispose()
        {
            Transaction current = Transaction.Current;
            Transaction.Current = transaction;
            if (!this.isCompleted)
            {
                current.Rollback();
            }
            CommittableTransaction committableTransaction = current as CommittableTransaction;
            if (committableTransaction != null)
            {
                if (this.isCompleted)
                {
                    committableTransaction.Commit();
                }
                committableTransaction.Dispose();
            }
        }
    }
}
