using System;
using System.Security.Cryptography;
using System.Text;

namespace HomenetBase
{
	public class Authentication
	{
        public static string BuildAuthenticationHeader(string userName, string password)
        {
            var passwordHashBase64 = Calculate_SHA256_base64(password);
            var header = userName + ":" + passwordHashBase64;
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(header));
        }

        public static string Calculate_SHA256_base64(string input)
        {
            using (SHA256 mySHA256 = SHA256.Create())
            {
                byte[] PasswordAsBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashValue = mySHA256.ComputeHash(PasswordAsBytes);
                string Base64 = Convert.ToBase64String(hashValue);
                return Base64;
            }
        }
	}
}
