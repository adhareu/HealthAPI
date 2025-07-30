using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CanadianHealthcareWaitTimeApi.Models;

namespace CanadianHealthcareWaitTimeApi.Services
{
    public interface IWaitTimeService
    {
        Task<WaitTimeResponse> GetWaitTimeAsync(WaitTimeRequest request);
    }
}
