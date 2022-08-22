namespace Pangea.Shared.Caching.Models
{
    public sealed class CacheSettings
    {
        /// <summary>
        /// Duration in minutes
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Defines cache is local In-Memory cache or distributed
        /// </summary>
        public bool IsLocal { get; set; } = true;

        public DistributedCacheSettings? DistributedCacheSettings { get; set; }
    }
}
