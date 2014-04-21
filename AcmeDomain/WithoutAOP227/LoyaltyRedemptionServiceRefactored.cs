using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcmeDomain.Entities;

namespace AcmeDomain.WithoutAOP227
{
    public class LoyaltyRedemptionServiceRefactored : ILoyaltyRedemptionService
    {
        readonly ILoyaltyDataService _dataService;
        readonly IExceptionHandler _exceptionHandler;
        readonly ITransactionManager _transactionManager;

        readonly ITransactionManager _transactionManager2;

        //ctor
        public LoyaltyRedemptionServiceRefactored(ILoyaltyDataService service, IExceptionHandler exceptionHandler, ITransactionManager transactionManager)
        {
            _dataService = service;
            _exceptionHandler = exceptionHandler;
            _transactionManager = transactionManager;
        }

       //ctor using combined TransactionManager
        public LoyaltyRedemptionServiceRefactored(ILoyaltyDataService service, ITransactionManager tran2)
        {
            _dataService = service;
            _transactionManager2 = tran2;
        }

        //original method
        public void Redeem(Invoice invoice, int numberOfDays)
        {
            //defensive programming
            if (invoice == null) throw new ArgumentNullException("invoice");

            if (numberOfDays <= 0)            
                throw new ArgumentException("", "numberOfDays");
                        
            //logging
            Console.WriteLine("Redeem: {0}", DateTime.Now);
            Console.WriteLine("Invoice: {0}", invoice.Id);

            _exceptionHandler.Wrapper(() =>
            {
                _transactionManager.Wrapper(() =>
                {
                    var pointsPerDay = 10;
                    if (invoice.Vehicle.Size >= Size.Luxury)                    
                        pointsPerDay = 15;                    
                    var points = numberOfDays * pointsPerDay;
                    _dataService.SubtractPoints(invoice.Id, points);
                    invoice.Discount = numberOfDays * invoice.CostPerDay;

                    //logging
                    Console.WriteLine("Redeem complete: {0}", DateTime.Now);
                });
            });

        }

        //Redeem method refactored without AOP using combined trans manager 2
        public void RedeemRefWithoutAOP(Invoice invoice, int numberOfDays)
        {
            //defensive programming
            if (invoice == null) throw new ArgumentNullException("invoice");

            if (numberOfDays <= 0)
                throw new ArgumentException("", "numberOfDays");

            //logging
            Console.WriteLine("Redeem: {0}", DateTime.Now);
            Console.WriteLine("Invoice: {0}", invoice.Id);

            //combined t-manager better but still complicated
            _transactionManager2.Wrapper(() =>
            {
                var pointsPerDay = 10;
                if (invoice.Vehicle.Size >= Size.Luxury)
                    pointsPerDay = 15;
                var points = numberOfDays * pointsPerDay;
                _dataService.SubtractPoints(invoice.Id, points);
                invoice.Discount = numberOfDays * invoice.CostPerDay;

                //logging
                Console.WriteLine("Redeem complete: {0}", DateTime.Now);
            });
           
        }

    }
}
