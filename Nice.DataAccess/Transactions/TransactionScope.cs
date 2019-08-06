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
        public TransactionScope(IsolationLevel isolationLevel = IsolationLevel.Unspecified) :
            this(DataUtil.DefaultConnStringKey, isolationLevel)
        { }

        public TransactionScope(string connStriKey, IsolationLevel isolationLevel = IsolationLevel.Unspecified)
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


        public static bool Excute(Action action)
        {
            return Excute(action, DataUtil.DefaultConnStringKey);
        }

        public static bool Excute(Action action, string connStriKey)
        {
            bool result = false;
            using (TransactionScope transactionScope = new TransactionScope(connStriKey))
            {
                try
                {
                    action();
                    transactionScope.Complete();
                    result = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            };
            return result;
        }
    }
}
