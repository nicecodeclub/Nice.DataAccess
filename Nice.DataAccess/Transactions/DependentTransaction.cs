namespace Nice.DataAccess.Transactions
{
    public class DependentTransaction : Transaction
    {
        private Transaction innerTransaction;
        public Transaction InnerTransaction
        {
            get { return innerTransaction; }
            private set { innerTransaction = value; }
        }
        internal DependentTransaction(Transaction transaction)
        {
            this.innerTransaction = transaction;
            this.DbTransaction = this.InnerTransaction.DbTransaction;
        }
    }
}
