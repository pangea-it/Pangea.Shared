using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Pangea.Shared.Middlewares
{
    public class QueryArrayValueProviderFactory : IValueProviderFactory
    {
        #region Class Members

        private readonly string _Separator;
        private readonly string _Key;

        #endregion

        #region Constructors

        public QueryArrayValueProviderFactory(string separator)
            : this(string.Empty, separator) { }

        public QueryArrayValueProviderFactory(string key, string separator)
        {
            _Key = key;
            _Separator = separator;
        }

        #endregion

        #region Methods

        public Task CreateValueProviderAsync(ValueProviderFactoryContext context)
        {
            context.ValueProviders.Insert(0, new QueryArrayValueProvider(_Key, context.ActionContext.HttpContext.Request.Query, _Separator));
            
            return Task.CompletedTask;
        }

        #endregion
    }
}
