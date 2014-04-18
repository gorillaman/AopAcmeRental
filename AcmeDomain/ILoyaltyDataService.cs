using System;
using System.Linq;

namespace AcmeDomain
{
    public interface ILoyaltyDataService
    {
        void AddPoints(Guid customerId, int points);
        void SubtractPoints(Guid customerId, int points);
    }
}
