using System;
using System.Linq;

namespace Hemlock.Models.FakeDataClasses
{
    public class FakeTransactionLog : FakeDbSet<TransactionLog>
    {
        public override TransactionLog Find(params object[] keyValues)
        {
            //will return exception, convert to list
            return this.SingleOrDefault(
                transaction => transaction.ChangeDate == (DateTime)keyValues.Single());
        }

        private TransactionLog ToList(Func<object, bool> p)
        {
            throw new NotImplementedException();
        }
    }
}