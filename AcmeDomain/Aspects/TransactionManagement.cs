using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PostSharp.Aspects;
using System.Transactions;

namespace AcmeDomain.Aspects
{
    [Serializable]
    public class TransactionManagement : MethodInterceptionAspect
    {
        public override void OnInvoke(MethodInterceptionArgs args)
        {
            Console.WriteLine("Starting trans...");


            //start new transaction
            using (var scope = new TransactionScope())
            {
                //retry up to three times
                var retries = 3;
                var succeeded = false;

                while (!succeeded)
                {
                    try
                    {
                        args.Proceed();

                        //complete transaction
                        scope.Complete();
                        succeeded = true;
                    }
                    catch
                    {
                        //dont re-trhwo until the
                        //retry limit is reached
                        if (retries >= 0)
                            retries--;
                        else
                            throw;                        
                    }
                }
            }

            Console.WriteLine("Trans complete...");
        }

    }
}
