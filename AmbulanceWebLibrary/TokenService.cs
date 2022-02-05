using AmbulanceWebLibrary.Models;
using System;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using System.ComponentModel;

namespace AmbulanceWebLibrary
{
    public class TokenService
    {
        string tokenHeader = "{ \"alg\": \"HS256\", \"typ\": \"JWT\"}";
        private static TokenService instance;
        string secret;

        public TokenService()
        {
            var hmac = new HMACSHA256();
            secret = Convert.ToBase64String(hmac.Key);
        }

        public static TokenService getInstance()
        {
            if (instance == null)
                instance = new TokenService();
            return instance;
        }

        public string GenerateToken(Worker worker)
        {
            string tokenHeaderEncode = ConvertToBase64String(tokenHeader);

            DateTime today = DateTime.Now;
            string tokenPayload = JsonConvert.SerializeObject(new { issueDate = today.ToString(),
                expireDate = today.AddDays(1).ToString(),
                worker = worker });
            string tokenPayloadEncode = ConvertToBase64String(tokenPayload);

            string tokenSignatureEncode = ConvertToBase64String_signature(String.Format("{0}.{1}", tokenHeaderEncode, tokenPayloadEncode));

            string token = String.Format("{0}.{1}.{2}", tokenHeaderEncode, tokenPayloadEncode, tokenSignatureEncode);

            return token;
        }

        public bool CheckToken(string token)
        {
            int position = token.LastIndexOf(".");
            string firstPart = token.Substring(0, position);
            string secondPart = token.Substring(position + 1);

            string tokenSignatureEncode = ConvertToBase64String_signature(firstPart);

            return String.Compare(tokenSignatureEncode, secondPart) == 0;
        }

        string ConvertToBase64String(string tokenPart)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(tokenPart))
            .Replace('+', '-')
            .Replace('/', '_')
            .Replace("=", "");
        }

        string ConvertToBase64String_signature(string tokenPart)
        {
            return Convert.ToBase64String(HashHMAC(Convert.FromBase64String(secret), Encoding.UTF8.GetBytes(tokenPart)))
            .Replace('+', '-')
            .Replace('/', '_')
            .Replace("=", "");
        }

        private static byte[] HashHMAC(byte[] key, byte[] message)
        {
            var hash = new HMACSHA256(key);
            return hash.ComputeHash(message);
        }

    }
}
