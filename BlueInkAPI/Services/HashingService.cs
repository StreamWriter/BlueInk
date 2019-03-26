using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlueInk.API.Services
{
    public class HashingService
    {
        public (string hashedPassword, string salt) Hash(string password)
        {
            var salt = GetSalt();

            var hashedPass = Hash(password, Encoding.UTF8.GetString(salt));

            return (hashedPass, Encoding.UTF8.GetString(salt));
        }

        public string Hash(string password, string salt)
        {
            var saltBytes = Encoding.UTF8.GetBytes(salt);

            var valueBytes = KeyDerivation.Pbkdf2(
                               password: password,
                               salt: saltBytes,
                               prf: KeyDerivationPrf.HMACSHA512,
                               iterationCount: 10000,
                               numBytesRequested: 32);

            var hashedPass = Convert.ToBase64String(valueBytes);

            return hashedPass;
        }

        private static byte[] GetSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
    }
}
