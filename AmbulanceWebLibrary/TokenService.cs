using AmbulanceWebLibrary.Models;
using System;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;

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
            try
            {
                int position = token.LastIndexOf(".");
                string firstPart = token.Substring(0, position);
                string secondPart = token.Substring(position + 1);

                string tokenSignatureEncode = ConvertToBase64String_signature(firstPart);

                return String.Compare(tokenSignatureEncode, secondPart) == 0;
            } catch
            {
                return false;
            }
            
        }

        public int GetTeamFromToken(string token)
        {
            try
            {
                int position = token.LastIndexOf(".");
                string firstPart = token.Substring(0, position);

                string payload = firstPart.Substring(token.IndexOf(".") + 1);
                payload = payload.Substring(0, payload.Length);
                payload = payload.Replace('-', '+').Replace('_', '/');
                payload += "=";
                byte[] data = Convert.FromBase64String(payload);
                string decodedString = Encoding.UTF8.GetString(data);

                var worker = JObject.Parse(decodedString);
                var a = worker["worker"]["workTeam"]["id"];


                return Convert.ToInt32(a);
            }
            catch
            {
                return -1;
            }
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
