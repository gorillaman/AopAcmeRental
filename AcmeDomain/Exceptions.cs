using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcmeDomain
{
    public class Exceptions
    {
        public static bool Handle(Exception ex)
        {
            if (ex.GetType() == typeof(ArithmeticException))
                return false;
            if (ex.GetType() == typeof(TimeoutException))
                return true;

            return false;			 
	    }                                    
    }
}
