namespace Microsoft.Azure.WebJobs.HttpTriggerQueryBinding
{
    using Microsoft.Azure.WebJobs.Host.Config;

    /// <summary>
    /// Query parameter configuration provider.
    /// </summary>
    public sealed class QueryConfigProvider : IExtensionConfigProvider
    {
        /// <inheritdoc/>
        public void Initialize(ExtensionConfigContext context)
        {
            context.AddBindingRule<QueryAttribute>()
                .Bind(new QueryBindingProvider());
        }
    }
}
