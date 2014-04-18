using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcmeDomain.Entities;

namespace AcmeDomain
{
    public interface ILoyaltyRedemptionService
    {
        void Redeem(Invoice invoice, int numberOfDays);
    }      
}
