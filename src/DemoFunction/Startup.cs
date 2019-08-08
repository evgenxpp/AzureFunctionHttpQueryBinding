using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Azure.WebJobs.HttpTriggerQueryBinding;

[assembly: WebJobsStartup(typeof(DemoFunction.Startup))]

namespace DemoFunction
{
    /// <summary>
    /// Application startup.
    /// </summary>
    public sealed class Startup : IWebJobsStartup
    {
        /// <inheritdoc/>
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddExtension<QueryConfigProvider>();
        }
    }
}
