namespace Microsoft.Azure.WebJobs.HttpTriggerQueryBinding
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Internal;
    using Microsoft.Azure.WebJobs.Host.Bindings;
    using Microsoft.Azure.WebJobs.Host.Protocols;

    /// <summary>
    /// Query parameter binding.
    /// </summary>
    internal class QueryBinding : IBinding
    {
        private readonly ParameterInfo queryParameterInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryBinding"/> class.
        /// </summary>
        /// <param name="queryParameterInfo">The query parameter info.</param>
        public QueryBinding(ParameterInfo queryParameterInfo)
        {
            this.queryParameterInfo = queryParameterInfo ?? throw new ArgumentNullException(nameof(queryParameterInfo));
        }

        /// <inheritdoc/>
        public bool FromAttribute => true;

        /// <inheritdoc/>
        public Task<IValueProvider> BindAsync(object value, ValueBindingContext context)
        {
            return Task.FromResult<IValueProvider>(
                new QueryValueProvider(
                    this.queryParameterInfo.ParameterType,
                    value as IEnumerable<string>));
        }

        /// <inheritdoc/>
        public async Task<IValueProvider> BindAsync(BindingContext context)
        {
            var request = GetHttpReuqest(context.BindingData);
            return await this.BindAsync(
                this.GetStringValuesFromQuery(request?.Query),
                context.ValueContext);
        }

        /// <inheritdoc/>
        public ParameterDescriptor ToParameterDescriptor() => new ParameterDescriptor
        {
            Name = this.queryParameterInfo.Name,
        };

        private static DefaultHttpRequest GetHttpReuqest(IReadOnlyDictionary<string, object> bindingData)
        {
            const string requestBindingDataName = "$request";

            if (bindingData.TryGetValue(requestBindingDataName, out var requestObject)
                && requestObject is DefaultHttpRequest request)
            {
                return request;
            }

            return null;
        }

        private IEnumerable<string> GetStringValuesFromQuery(IQueryCollection queryCollection)
        {
            if (queryCollection != null)
            {
                var queryAttribute = this.queryParameterInfo.GetCustomAttribute<QueryAttribute>();
                var queryParameterName = queryAttribute == null || string.IsNullOrWhiteSpace(queryAttribute.Name)
                    ? this.queryParameterInfo.Name
                    : queryAttribute.Name;

                if (queryCollection.TryGetValue(queryParameterName, out var stringValues))
                {
                    return stringValues;
                }
            }

            return null;
        }
    }
}
