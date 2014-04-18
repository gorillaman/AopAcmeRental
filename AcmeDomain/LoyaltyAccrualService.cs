using System;
using System.Linq;
using AcmeDomain.Entities;
using System.Transactions;

namespace AcmeDomain
{
    public class LoyaltyAccrualService : ILoyaltyAccrualService
    {
        readonly ILoyaltyDataService _loyaltyDataService;

        public LoyaltyAccrualService(ILoyaltyDataService service)
        {
            _loyaltyDataService = service;
        }

        public void Accrue(RentalAgreement agreement)
        {   
            //defensive programming
            if (agreement == null) throw new ArgumentNullException("agreement");

            //logging
            Console.WriteLine("Accrue: {0}", DateTime.Now);
            Console.WriteLine("Customer: {0}", agreement.Customer.Id);
            Console.WriteLine("Customer: {0}", agreement.Vehicle.Id);

            try
            {            
                using (var scope = new TransactionScope())
                {
                    var retries = 3;
                    var succeeded = false;

                    while (!succeeded)
                    {
                        try
                        {
                            var rentalTimeSpan = (agreement.EndDate.Subtract(agreement.StartDate));
                            var numberOfDays = (int)Math.Floor(rentalTimeSpan.TotalDays);
                            var pointsPerDay = 1;
                            if (agreement.Vehicle.Size >= Size.Luxury)
                                pointsPerDay = 2;
                            var points = numberOfDays * pointsPerDay;
                            _loyaltyDataService.AddPoints(agreement.Customer.Id, points);

                            scope.Complete();

                            succeeded = true;
                            //logging
                            Console.WriteLine("Accrue complete: {0}", DateTime.Now);
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
                if (!Exceptions.Handle(ex))
                    throw;
                
            }
           
        }


    }
     

}
