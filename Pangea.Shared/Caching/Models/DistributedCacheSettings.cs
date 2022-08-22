namespace Pangea.Shared.Caching.Models
{
    public class DistributedCacheSettings
    {
        public string? Endpoint { get; set; }

        public int Port { get; set; } = 6379;

        public bool AllowAdmin { get; set; } = true;

        public int ConnectTimeout { get; set; } = 60 * 1000;
    }
}
