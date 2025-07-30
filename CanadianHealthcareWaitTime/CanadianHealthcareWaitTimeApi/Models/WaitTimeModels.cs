using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanadianHealthcareWaitTimeApi.Models
{
    public class WaitTimeRequest
    {
        public string PostalCode { get; init; }
        public string ServiceType { get; init; }
    }

    public class WaitTimeResponse
    {
        public string PostalCode { get; init; }
        public string ServiceType { get; init; }
        public int EstimatedWaitTimeMinutes { get; init; }
        public string Message { get; init; }
    }
}
