using System;
using System.Linq;
using PostSharp.Aspects;
using AcmeDomain.Entities;

namespace AcmeDomain.Aspects
{
    [Serializable]
    public class LoggingAspect : OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            //listing 2.24
            Console.WriteLine("{0}: {1}", args.Method.Name, DateTime.Now);

            //listing 2.26
            foreach (var argument in args.Arguments)
            {
                if (argument.GetType() == typeof(RentalAgreement))
                {
                    Console.WriteLine("Customer: {0}",
                        ((RentalAgreement)argument).Customer.Id);
                    Console.WriteLine("Vehicle: {0}",
                        ((RentalAgreement)argument).Vehicle.Id);
                }
                if (argument.GetType() == typeof(Invoice))
                    Console.WriteLine("Invoice: {0}", ((Invoice)argument).Id);
            }
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            Console.WriteLine("{0} complete: {1}", args.Method.Name, DateTime.Now);
                //base.OnSuccess(args);
        }
    }
}
