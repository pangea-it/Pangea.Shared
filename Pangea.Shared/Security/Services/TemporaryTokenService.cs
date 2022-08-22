using Pangea.Shared.Extensions.General;
using Pangea.Shared.Security.Models;
using System.ComponentModel.DataAnnotations;

namespace Pangea.Shared.Security.Services
{
    public class TemporaryTokenService
    {
        #region Class Members

        private readonly CryptingService _cryptingService;
        private readonly TokenSettings _tokenSettings;

        #endregion

        #region Constructors

        public TemporaryTokenService(CryptingService cryptingService, TokenSettings tokenSettings)
        {
            _cryptingService = cryptingService;
            _tokenSettings = tokenSettings;
        }

        #endregion

        #region Methods

        public string GenerateTemporaryToken<T>(T inputToken) where T : TemporaryTokenModel, new()
        {
            var expireDate = Convert.ToInt32((DateTime.Now.AddMinutes(_tokenSettings.ExpirationTime) - new DateTime(1970, 1, 1)).TotalSeconds);
            inputToken.Expires = expireDate;

            var padding = new[] { '=' };
            var token = _cryptingService.Encrypt(_tokenSettings.EncryptionKey!, inputToken.ToJson()).TrimEnd(padding).Replace('+', '-').Replace('/', '_');

            return token;
        }

        public T ParseTemporaryToken<T>(string inputToken, bool validateExpiration = true) where T : TemporaryTokenModel, new()
        {
            var incoming = inputToken.Replace('_', '/').Replace('-', '+');

            switch (inputToken.Length % 4)
            {
                case 2: incoming += "=="; break;
                case 3: incoming += "="; break;
            }

            var cypher = _cryptingService.Decrypt(_tokenSettings.EncryptionKey!, incoming).TrimAll();

            T token;
            try
            {
                token = cypher!.FromJson<T>();
            }
            catch (Exception ex)
            {
                throw new ValidationException("Invalid token", ex);
            }

            if (validateExpiration)
            {
                var unixTime = Convert.ToInt32((DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds);
                
                if (unixTime > token.Expires) throw new ValidationException("Token is outdated");
            }

            return token;
        }

        #endregion
    }
}
