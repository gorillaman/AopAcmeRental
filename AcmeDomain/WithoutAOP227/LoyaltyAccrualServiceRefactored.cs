using System;
using System.Linq;
using AcmeDomain.Entities;

namespace AcmeDomain.WithoutAOP227
{
    public class LoyaltyAccrualServiceRefactored : ILoyaltyAccrualService
    {
        readonly ILoyaltyDataService _dataService;

        readonly IExceptionHandler _exceptionHandler;
        readonly ITransactionManager _transactionManager;
        readonly ITransactionManager _tranMgr2;
        readonly ITransactionFacade _tranFacade;

        //ctor - gone wild
        public LoyaltyAccrualServiceRefactored(ILoyaltyDataService service, IExceptionHandler exceptionHandler, ITransactionManager transactionManager)
        {
            _dataService = service;

            _exceptionHandler = exceptionHandler;
            _transactionManager = transactionManager;
        }

        //ctor - 2 services combined, from listing 2.21
        public LoyaltyAccrualServiceRefactored(ILoyaltyDataService service, ITransactionManager tran2)
        {
            _dataService = service;
            _tranMgr2 = tran2;
        }

        //ctor - recieves facade t-manager
        public LoyaltyAccrualServiceRefactored(ILoyaltyDataService service, ITransactionFacade tranFacade)
        {
            _dataService = service;
            _tranFacade = tranFacade;
        }

        //original method
        public void Accrue(RentalAgreement agreement)
        {
            //defensive programming
            if (agreement == null)
            {
                throw new ArgumentNullException("agreement");
            }

            //logging
            Console.WriteLine("Accrue: {0}", DateTime.Now);
            Console.WriteLine("Customer: {0}", agreement.Customer.Id);
            Console.WriteLine("Vehicle: {0}", agreement.Vehicle.Id);

            //exception handling
            _exceptionHandler.Wrapper(() =>
            {
                _transactionManager.Wrapper(() => {
                    
                        var rentalTime = (agreement.EndDate.Subtract(agreement.StartDate));
                        var numberOfDays = (int)Math.Floor(rentalTime.TotalDays);
                        var pointsPerDay = 1;
                        if (agreement.Vehicle.Size >= Size.Luxury)
                            pointsPerDay = 2;
                        var points = numberOfDays * pointsPerDay;
                        _dataService.AddPoints(agreement.Customer.Id, points);

                        //logging
                        Console.WriteLine("Accrue complete: {0}", DateTime.Now);                   
                });

            });            
        }

        //refactored without AOP
        public void AccrueRefWithoutAOP(RentalAgreement agreement)
        {
            //defensive programming
            if (agreement == null)
            {
                throw new ArgumentNullException("agreement");
            }

            //logging
            Console.WriteLine("Accrue: {0}", DateTime.Now);
            Console.WriteLine("Customer: {0}", agreement.Customer.Id);
            Console.WriteLine("Vehicle: {0}", agreement.Vehicle.Id);

            //using a facade wrapper
            _tranFacade.Wrapper(() =>
            //uses combined TransactionManager that combines exceptions and transactions
            //_tranMgr2.Wrapper(() =>
            {
                var rentalTime = (agreement.EndDate.Subtract(agreement.StartDate));
                var numberOfDays = (int)Math.Floor(rentalTime.TotalDays);
                var pointsPerDay = 1;
                if (agreement.Vehicle.Size >= Size.Luxury)
                    pointsPerDay = 2;
                var points = numberOfDays * pointsPerDay;
                _dataService.AddPoints(agreement.Customer.Id, points);

                //logging
                Console.WriteLine("Accrue complete: {0}", DateTime.Now);
            });
           
        }
    }
}
