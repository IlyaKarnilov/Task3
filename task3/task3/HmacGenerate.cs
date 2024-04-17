using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace task3
{
    public class HmacGenerate
    {
        public string GenerateHmacKey()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] key = new byte[32];
                rng.GetBytes(key);
                string hexKey = BitConverter.ToString(key).Replace("-", "").ToLower();
                return hexKey;
            }
        }

        public string CalculateHmac(string move, string key)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(move);
                byte[] hash = hmac.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}
