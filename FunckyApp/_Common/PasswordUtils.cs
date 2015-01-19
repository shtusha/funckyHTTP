using System;
using System.Security.Cryptography;
using System.Text;

namespace FunckyApp
{
    public static class PasswordUtils
    {

        public static string HashPassword(this string password, string salt)
        {
            using (var hasher = new SHA256Managed())
            {
                var hashedBytes = hasher.ComputeHash(Encoding.ASCII.GetBytes(password + salt));
                return Convert.ToBase64String(hashedBytes);
            }
        }


        private static readonly RandomNumberGenerator _generator = RandomNumberGenerator.Create();

        public static string GenerateHashSalt()
        {
            var bytes = new byte[12]; //just a number
            _generator.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}
