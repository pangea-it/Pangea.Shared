using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using System.Globalization;

namespace Pangea.Shared.Middlewares
{
    public class QueryArrayValueProvider : QueryStringValueProvider
    {
        #region Class Members

        private readonly string _key;
        private readonly string _separator;
        private readonly IQueryCollection _values;

        #endregion

        #region Constructors

        public QueryArrayValueProvider(IQueryCollection values, string separator)
            : this(string.Empty, values, separator) { }

        public QueryArrayValueProvider(string key, IQueryCollection values, string separator)
            : base(BindingSource.Query, values, CultureInfo.InvariantCulture)
        {
            _key = key;
            _values = values;
            _separator = separator;
        }

        #endregion

        #region Methods

        public override ValueProviderResult GetValue(string key)
        {
            var result = base.GetValue(key);

            if (_key != null && _key != key)
            {
                return result;
            }

            if (result != ValueProviderResult.None && result.Values.Any(x => x.IndexOf(_separator, StringComparison.OrdinalIgnoreCase) > 0))
            {
                var splitValues = new StringValues(result.Values.SelectMany(x => x.Split(new[] { _separator }, StringSplitOptions.None)).ToArray());

                return new ValueProviderResult(splitValues, result.Culture);
            }

            return result;
        }

        #endregion
    }
}
