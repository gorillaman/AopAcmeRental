using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcmeDomain.Entities
{
    public class RentalAgreement
    {
        public Guid Id { get; set; }
        public Customer Customer { get; set; }
        public Vehicle Vehicle { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
