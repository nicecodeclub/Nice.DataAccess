using System;
using System.Data;

namespace Nice.DataAccess.Transactions
{
    public class Transaction
    {
        [ThreadStatic]
        private static Transaction current;

        public static Transaction Current
        {
            get { return current; }
            set { current = value; }
        }

        private IDbTransaction dbTransaction;
        public IDbTransaction DbTransaction
        {
            get { return dbTransaction; }
            protected set { dbTransaction = value; }
        }
        public DependentTransaction DependentClone()
        {
            return new DependentTransaction(this);
        }
       
        public void Rollback()
        {
            this.dbTransaction.Rollback();
        }
        public void Dispose()
        {
            if (dbTransaction.Connection != null)
            {
                dbTransaction.Connection.Close();
                dbTransaction.Connection.Dispose();
            }
            this.dbTransaction.Dispose();
        }
    }
}
