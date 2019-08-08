namespace Microsoft.Azure.WebJobs.HttpTriggerQueryBinding
{
    using System.Threading.Tasks;
    using Microsoft.Azure.WebJobs.Host.Bindings;

    /// <summary>
    /// Query parameter binding provider.
    /// </summary>
    internal class QueryBindingProvider : IBindingProvider
    {
        /// <inheritdoc/>
        public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            return Task.FromResult<IBinding>(
                new QueryBinding(context.Parameter));
        }
    }
}
