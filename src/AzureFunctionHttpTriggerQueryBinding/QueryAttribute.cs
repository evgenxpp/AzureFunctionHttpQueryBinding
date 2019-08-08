namespace Microsoft.Azure.WebJobs.HttpTriggerQueryBinding
{
    using System;
    using Microsoft.Azure.WebJobs.Description;

    /// <summary>
    /// Query parameter attribute.
    /// </summary>
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class QueryAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryAttribute"/> class.
        /// </summary>
        /// <param name="name">The query parameter name.</param>
        public QueryAttribute(string name = null)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets query parameter name.
        /// </summary>
        public string Name { get; }
    }
}
