using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CanadianHealthcareWaitTimeApi.Exceptions
{
    public class WaitTimeApiException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public WaitTimeApiException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
