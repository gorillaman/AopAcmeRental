using System;
using System.Linq;
using System.Transactions;

namespace AcmeDomain
{

    public interface ITransactionManager
    {
        void Wrapper(Action method);
    }

    public class TransactionManager : ITransactionManager
    {
        public void Wrapper(Action method)
        {
            using (var scope = new TransactionScope())
            {
                var retries = 3;
                var succeeded = false;
                while (!succeeded)
                {
                    try
                    {
                        method();
                        scope.Complete();
                        succeeded = true;
                    }
                    catch (Exception)
                    {
                        if (retries >= 0)                        
                            retries--;                        
                        else
                            throw;                       
                    }
                }

            }
        }

    }
}
