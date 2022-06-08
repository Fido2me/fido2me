using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.ApplicationInsights.Extensibility.EventCounterCollector;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using AppInsights = Microsoft.ApplicationInsights.AspNetCore.Extensions;

namespace Fido2me
{
    public static class StartupConfigurationHelper
    {
        /// <summary>
        /// Helps to reduce the amount of collected data.
        /// See https://docs.microsoft.com/en-us/azure/azure-monitor/app/asp-net-core
        /// </summary>
        /// <param name="services"></param>
        /// <param name="aiKey"></param>
        public static void ConfigureApplicationInsights(IServiceCollection services, string aiKey)
        {
            var aiOptions = new AppInsights.ApplicationInsightsServiceOptions
            {
                // Disables adaptive sampling.
                EnableAdaptiveSampling = false,

                // Disables QuickPulse (Live Metrics stream).
                EnableQuickPulseMetricStream = false,

                AddAutoCollectedMetricExtractor = false,

                EnableActiveTelemetryConfigurationSetup = true,

                InstrumentationKey = aiKey,
                
            };


            services.AddApplicationInsightsTelemetry(aiOptions);
            services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>((module, o) =>
            {
                //module
            });

            services.ConfigureTelemetryModule<EventCounterCollectionModule>((module, o) =>
            {
                module.Counters.Clear();
                module.Counters.Add(new EventCounterCollectionRequest("System.Runtime", "gen-0-size"));
            });

            var performanceCounterService = services.FirstOrDefault<ServiceDescriptor>(t => t.ImplementationType == typeof(PerformanceCollectorModule));
            if (performanceCounterService != null)
            {
                services.Remove(performanceCounterService);
            }
        }

        public static void ConfigreSynchronousIO(IServiceCollection services, bool isAllowed)
        {
            // If using Kestrel:
            services.Configure<KestrelServerOptions>(options =>
            { options.AllowSynchronousIO = isAllowed; });

            // If using IIS:
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = isAllowed;
            });
        }
    }
}
