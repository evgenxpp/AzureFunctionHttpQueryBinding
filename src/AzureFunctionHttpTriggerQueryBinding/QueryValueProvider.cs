namespace Microsoft.Azure.WebJobs.HttpTriggerQueryBinding
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.WebJobs.Host.Bindings;

    /// <summary>
    /// Query parameter value provider.
    /// </summary>
    internal class QueryValueProvider : IValueProvider
    {
        private readonly IEnumerable<string> stringValues;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryValueProvider"/> class.
        /// </summary>
        /// <param name="parameterType">The parameter type.</param>
        /// <param name="parameterName">The parameter name.</param>
        /// <param name="queryCollection">The http query collection.</param>
        public QueryValueProvider(
            Type parameterType,
            IEnumerable<string> stringValues)
        {
            this.Type = parameterType ?? throw new ArgumentNullException(nameof(parameterType));
            this.stringValues = stringValues ?? new List<string>();
        }

        /// <inheritdoc/>
        public Type Type { get; }

        /// <inheritdoc/>
        public Task<object> GetValueAsync()
        {
            if (this.stringValues.Any())
            {
                var elementType = GetParameterElementType();

                if (typeof(IConvertible).IsAssignableFrom(elementType)
                    && this.TryConvertStringValuesToObject(elementType, out var convertedObject))
                {
                    return Task.FromResult(convertedObject);
                }
            }

            return Task.FromResult(
                this.Type.IsValueType
                    ? Activator.CreateInstance(this.Type)
                    : null);
        }

        /// <inheritdoc/>
        public string ToInvokeString() => null;

        private Type GetParameterElementType()
        {
            if (this.Type.IsArray)
            {
                return this.Type.GetElementType();
            }
            else if (typeof(IEnumerable).IsAssignableFrom(this.Type) && this.Type.IsGenericType)
            {
                return this.Type.GetGenericArguments().First();
            }

            return this.Type;
        }

        private bool TryConvertStringValuesToObject(Type elementType, out object convertedObject)
        {
            var converter = TypeDescriptor.GetConverter(elementType);

            try
            {
                if (this.Type.IsArray)
                {
                    var array = Array.CreateInstance(elementType, this.stringValues.Count());

                    foreach ((var index, var value) in this.stringValues.Select((value, index) => (index, value)))
                    {
                        array.SetValue(converter.ConvertFromInvariantString(value), index);
                    }

                    convertedObject = array;
                }
                else if (typeof(IEnumerable).IsAssignableFrom(this.Type) && this.Type.IsGenericType)
                {
                    var list = Activator.CreateInstance(
                        typeof(List<>).MakeGenericType(elementType)) as IList;

                    foreach (var value in this.stringValues)
                    {
                        list.Add(converter.ConvertFromInvariantString(value));
                    }

                    convertedObject = list;
                }
                else
                {
                    convertedObject = converter.ConvertFromInvariantString(this.stringValues.First());
                }

                return true;
            }
            catch
            {
                convertedObject = null;
                return false;
            }
        }
    }
}
