using System;
using System.Linq;
using AcmeDomain.Entities;
using System.Transactions;

namespace AcmeDomain
{
    
    public class LoyaltyRedemptionService : ILoyaltyRedemptionService
    {
        readonly ILoyaltyDataService _loyaltyDataService;

        public LoyaltyRedemptionService(ILoyaltyDataService service)
        {//ctor
            _loyaltyDataService = service;
        }

        public void Redeem(Invoice invoice, int numberOfDays)
        {   
            //defensive programming
            if (invoice == null) throw new ArgumentNullException("invoice");

            if (numberOfDays <= 0)
                throw new ArgumentException("", "numberOfDays");

            //logging
            Console.WriteLine("Redeem: {0}", DateTime.Now);
            Console.WriteLine("Invoice: {0}", invoice.Id);

            try
            {           
                using ( var scope = new TransactionScope()) 
                {
                    var retries = 3;
                    var succeeded = false;

                    while (!succeeded)
                    {
                        try {
                
                                var pointsPerDay = 10;
                                if (invoice.Vehicle.Size >= Size.Luxury)
                                    pointsPerDay = 15;
                                var points = numberOfDays * pointsPerDay;
                                _loyaltyDataService.SubtractPoints(invoice.Customer.Id, points);
                                invoice.Discount = numberOfDays * invoice.CostPerDay;

                                scope.Complete();

                                succeeded = true;

                                //logging
                                Console.WriteLine("Redeem complete: {0}", DateTime.Now);
                            }
                        catch
                        {
                            if (retries >= 0)
                                retries--;
                            else
                                throw;
                        }            
                    }
                }

            }
            catch (Exception ex)
            {
                if(!Exceptions.Handle(ex))
                throw;
            }


           
        }


    }
    
}
