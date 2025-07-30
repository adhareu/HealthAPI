using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CanadianHealthcareWaitTimeApi.Services;

var host = new HostBuilder()
               .ConfigureFunctionsWebApplication()
               .ConfigureServices(services =>
               {
                   services.AddSingleton<IWaitTimeService, WaitTimeService>();
               })
               .Build();

await host.RunAsync();