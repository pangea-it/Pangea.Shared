namespace Pangea.Shared.Security.Models
{
    public class TokenSettings
    {
        public string? EncryptionKey { get; set; }

        /// <summary>
        /// Expiration time in minutes
        /// </summary>
        public int ExpirationTime { get; set; }
    }
}
