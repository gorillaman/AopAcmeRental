using System;
using System.Linq;

namespace AcmeDomain.Entities
{
    public class Invoice
    {
        public Guid Id { get; set; }
        public Customer Customer { get; set; }
        public Vehicle Vehicle { get; set; }
        public decimal CostPerDay { get; set; }
        public decimal Discount { get; set; }
    }
}
