namespace Pangea.Shared.Security.Models
{
    public class TemporaryTokenModel
    {
        public long Id { get; set; }

        /// <summary>
        /// Expiration in minutes
        /// </summary>
        public int Expires { get; set; }
    }
}
