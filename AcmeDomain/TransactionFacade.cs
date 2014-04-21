using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcmeDomain
{
    public interface ITransactionFacade
    {
        void Wrapper(Action action);
    }

    public class TransactionFacade :ITransactionFacade
    {
        readonly IExceptionHandler _exceptionHandler;
        readonly ITransactionManager _transactionManager;

        public TransactionFacade(IExceptionHandler exceptionHandler, ITransactionManager transactionManager)
        {//ctor
            _exceptionHandler = exceptionHandler;
            _transactionManager = transactionManager;
        }

        public void Wrapper(Action action)
        {
            _exceptionHandler.Wrapper(() =>
            {
                _transactionManager.Wrapper(() =>
                {
                    action();
                });
            });
        }
    }
}
