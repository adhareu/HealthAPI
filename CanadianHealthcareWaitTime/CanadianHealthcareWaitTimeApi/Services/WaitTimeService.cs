using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CanadianHealthcareWaitTimeApi.Exceptions;
using CanadianHealthcareWaitTimeApi.Models;
using Microsoft.Extensions.Logging;

namespace CanadianHealthcareWaitTimeApi.Services
{
    public class WaitTimeService : IWaitTimeService
    {
        private readonly ILogger<WaitTimeService> _logger;
        private readonly Dictionary<string, int> _waitTimes;

        public WaitTimeService(ILogger<WaitTimeService> logger)
        {
            _logger = logger;
            // Hardcoded wait times for MVP (in minutes)
            _waitTimes = new Dictionary<string, int>
            {
                { "M5V", 45 }, // Toronto
                { "V6B", 60 }, // Vancouver
                { "H3B", 30 }  // Montreal
            };
        }

        public async Task<WaitTimeResponse> GetWaitTimeAsync(WaitTimeRequest request)
        {
            _logger.LogInformation("Processing wait time request for postal code: {PostalCode}, service: {ServiceType}",
                request.PostalCode, request.ServiceType);

            if (string.IsNullOrWhiteSpace(request.PostalCode))
                throw new WaitTimeApiException("Postal code is required.");

            if (!IsValidPostalCode(request.PostalCode))
                throw new WaitTimeApiException("Invalid Canadian postal code format.");

            var serviceType = string.IsNullOrWhiteSpace(request.ServiceType) ? "ER" : request.ServiceType;

            var postalPrefix = request.PostalCode.ToUpper().Substring(0, 3);
            int waitTime = _waitTimes.TryGetValue(postalPrefix, out int time) ? time : 50;

            var response = new WaitTimeResponse
            {
                PostalCode = request.PostalCode,
                ServiceType = serviceType,
                EstimatedWaitTimeMinutes = waitTime,
                Message = $"Estimated wait time for {serviceType} near {request.PostalCode}"
            };

            _logger.LogInformation("Wait time calculated: {WaitTime} minutes", waitTime);
            return await Task.FromResult(response);
        }

        private bool IsValidPostalCode(string postalCode)
        {
            // Canadian postal code format: A1A1A1 (no spaces)
            var regex = new Regex(@"^[A-Za-z]\d[A-Za-z]\d[A-Za-z]\d$");
            return regex.IsMatch(postalCode);
        }
    }
}
