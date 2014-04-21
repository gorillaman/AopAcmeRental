using System;
using System.Linq;
using PostSharp.Aspects;

namespace AcmeDomain.Aspects
{
    [Serializable]
    public class ExceptionAspect : OnExceptionAspect
    {
        public override void OnException(MethodExecutionArgs args)
        {
            if (Exceptions.Handle(args.Exception))
            {
                args.FlowBehavior = FlowBehavior.Continue;
            }
        }
    }
}
