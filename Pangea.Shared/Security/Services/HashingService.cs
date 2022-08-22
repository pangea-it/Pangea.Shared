using Pangea.Shared.Security.Enums;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace Pangea.Shared.Security.Services
{
    public class HashingService
    {
        public string? ComputeHash(string source, HashAlgorithmsEnum algorithm = HashAlgorithmsEnum.SHA256)
        {
            if (string.IsNullOrEmpty(source)) throw new ValidationException("Hashing source is empty.");

            StringBuilder hexBytes = new();
            byte[] sourceBytes = Encoding.UTF8.GetBytes(source);

            using (HashAlgorithm alg = algorithm switch
            {
                HashAlgorithmsEnum.SHA256 => SHA256.Create(),
                HashAlgorithmsEnum.SHA384 => SHA384.Create(),
                _ => SHA512.Create(),
            })
            {
                byte[] hashValue = alg.ComputeHash(sourceBytes);
                hashValue.Select(hv => hv.ToString("X2"))
                    .ToList()
                    .ForEach(hv => 
                    {
                        hexBytes.Append(hv);
                    });
            }

            return hexBytes.ToString();
        }

        public string? HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ValidationException("Password is empty.");

            return BCrypt.Net.BCrypt.EnhancedHashPassword(password, BCrypt.Net.HashType.SHA384);
        }

        public bool? VerifyPasswordHash(string password, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ValidationException("Password is empty.");
            if (string.IsNullOrWhiteSpace(passwordHash)) throw new ValidationException("PasswordHash is empty.");

            return BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash, BCrypt.Net.HashType.SHA384);
        }
    }
}