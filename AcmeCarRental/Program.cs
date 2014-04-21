using System;
using System.Linq;
using AcmeDomain;
using AcmeDomain.Entities;
using without =  AcmeDomain.WithoutAOP227;

namespace AcmeCarRental
{
    class Program
    {
        static void Main(string[] args)
        {
            SimulateAddingPoints();

            Console.WriteLine();
            Console.WriteLine("***");
            Console.WriteLine();

            SimulateRemovingPoints();

            Console.WriteLine();
            Console.WriteLine();

            Console.ReadKey();
        }
        
        static void SimulateAddingPoints()
        {
            var dataService = new FakeLoyaltyDataService();
            //var tran2 = new TransactionManager2();

            //requires creating dependcies
            IExceptionHandler exceptionHandler = new ExceptionHandler();
            ITransactionManager tran = new TransactionManager();
            var facade = new TransactionFacade(exceptionHandler, tran);

            //original
            //var service = new LoyaltyAccrualService(dataService);

            //refactored combined tran-manager without AOP
            //var service = new without.LoyaltyAccrualServiceRefactored(dataService, tran2);

            //refactored without AOP, using facade
            var service = new without.LoyaltyAccrualServiceRefactored(dataService, facade);

            var rentalAgreement = new RentalAgreement
            {
                Customer = new Customer { Id = Guid.NewGuid(), 
                                          Name = "Matthew D. Groves", 
                                          DateOfBirth = new DateTime(1980,2, 10), 
                                          DriversLicense = "RR123456" },
                Vehicle = new Vehicle { Id = Guid.NewGuid(),
                                        Make = "Honda", 
                                        Model = "Odyssey", 
                                        Size = Size.Compact, 
                                        Vin = "1HABC123" },
                StartDate = DateTime.Now.AddDays(-3),
                EndDate = DateTime.Now
            };

            //original accrue method
            //service.Accrue(rentalAgreement);

            service.AccrueRefWithoutAOP(rentalAgreement);
        }

        static void SimulateRemovingPoints()
        {
            var dataService = new FakeLoyaltyDataService();
            var service = new LoyaltyRedemptionService(dataService);

            var invoice = new Invoice
            {
                Customer = new Customer
                {
                    Id = Guid.NewGuid(),
                    Name = "Jacob Watson",
                    DateOfBirth = new DateTime(1977, 4, 15),
                    DriversLicense = "RR009911"
                },
                Vehicle = new Vehicle
                {
                    Id = Guid.NewGuid(),
                    Make = "Cadillac",
                    Model = "Sedan",
                    Size = Size.Luxury,
                    Vin = "2BDI"
                },
                CostPerDay = 29.95m,
                Id = Guid.NewGuid()
            };

            service.Redeem(invoice, 3);
        }
        
    }
}
