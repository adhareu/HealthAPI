using CanadianHealthcareWaitTimeApi.Exceptions;
using CanadianHealthcareWaitTimeApi.Models;
using CanadianHealthcareWaitTimeApi.Services;
using System.Net;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace CanadianHealthcareWaitTimeApi
{
    public class WaitTimeFunction
    {
        private readonly IWaitTimeService _waitTimeService;
        private readonly ILogger<WaitTimeFunction> _logger;

        public WaitTimeFunction(IWaitTimeService waitTimeService, ILogger<WaitTimeFunction> logger)
        {
            _waitTimeService = waitTimeService;
            _logger = logger;
        }

        [Function("GetWaitTime")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "waittime")] HttpRequestData req)
        {
            _logger.LogInformation("Received wait time request at {RequestTime}", DateTime.UtcNow);

            try
            {
                // Validate API key
                if (!req.Headers.TryGetValues("X-Api-Key", out var apiKeyValues) || !IsValidApiKey(apiKeyValues))
                {
                    _logger.LogWarning("Invalid or missing API key.");
                    var response = req.CreateResponse(HttpStatusCode.Unauthorized);
                    await response.WriteStringAsync("Invalid or missing API key.");
                    return response;
                }

                // Parse query parameters
                var query = HttpUtility.ParseQueryString(req.Url.Query);
                var request = new WaitTimeRequest
                {
                    PostalCode = query["postalCode"],
                    ServiceType = query["serviceType"]
                };

                // Process request
                var result = await _waitTimeService.GetWaitTimeAsync(request);

                // Create response
                var responseOk = req.CreateResponse(HttpStatusCode.OK);
                await responseOk.WriteAsJsonAsync(result);
                return responseOk;
            }
            catch (WaitTimeApiException ex)
            {
                _logger.LogError(ex, "API error: {Message}", ex.Message);
                var response = req.CreateResponse(ex.StatusCode);
                await response.WriteStringAsync(ex.Message);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred.");
                var response = req.CreateResponse(HttpStatusCode.InternalServerError);
                await response.WriteStringAsync("An unexpected error occurred.");
                return response;
            }
        }

        private bool IsValidApiKey(System.Collections.Generic.IEnumerable<string> apiKeyValues)
        {
            // Placeholder: Replace with secure key storage (e.g., Azure Key Vault)
            const string validApiKey = "your-secure-api-key-12345";
            return apiKeyValues != null && apiKeyValues.Any(key => key == validApiKey);
        }
    }
}
