using Newtonsoft.Json;

namespace Pangea.Shared.Attributes.Authorization.Extensions
{
    public static class HttpClientExtensions
    {
        public async static Task<T> Get<T>(this HttpResponseMessage httpResponse, CancellationToken cancellationToken) where T : new()
        {
            var stringResponse = await httpResponse.Content.ReadAsStringAsync(cancellationToken);

            if (stringResponse is null)
            {
                return new T();
            }

            T result = JsonConvert.DeserializeObject<T>(stringResponse)!;

            return result;
        }
    }
}
