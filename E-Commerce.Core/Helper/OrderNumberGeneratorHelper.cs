using System.Security.Cryptography;
using System.Text;

namespace E_Commerce.Core.Helper
{
    public static class OrderNumberGeneratorHelper
    {
        public static string GenerateOrderNumber(Guid userId)
        {
            string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");

            string userHash = HashUserId(userId);

            string random = GenerateRandomNumber(4); 

            return $"{timestamp}-{userHash}-{random}";
        }

        private static string HashUserId(Guid userId)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(userId.ToString()));
                return BitConverter.ToString(hash).Replace("-", "").Substring(0, 6); 
            }
        }

        private static string GenerateRandomNumber(int length)
        {
            Random random = new Random();
            string randomNumber = string.Empty;

            for (int i = 0; i < length; i++)
            {
                randomNumber += random.Next(0, 10); 
            }

            return randomNumber;
        }
    }
}
