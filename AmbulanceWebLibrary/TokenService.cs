using AmbulanceWebLibrary.Models;
using System;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace AmbulanceWebLibrary
{
    public class TokenService
    {
        string tokenHeader = "{ \"alg\": \"HS256\", \"typ\": \"JWT\"}";
        const string AllowedChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz#@$^*()";
        public string key;

        public TokenService()
        {
            Random rand = new Random();

            key = "";
            for (int j = 1; j <= 64; j++)
            {
                // Выбор случайного числа от 0 до 25
                // для выбора буквы из массива букв.
                int letter_num = rand.Next(0, AllowedChars.Length - 1);

                // Добавить письмо.
                key += AllowedChars[letter_num];
            }
        }

        public string GenerateToken(Worker worker)
        {
            byte[] toEncodeAsBytes = System.Text.Encoding.ASCII.GetBytes(tokenHeader);
            string tokenHeaderEncode = Convert.ToBase64String(toEncodeAsBytes);

            DateTime today = DateTime.Now;
            string tokenPayload = JsonConvert.SerializeObject(new { issueDate = today.ToString(),
                expireDate = today.AddDays(1).ToString(),
                worker = worker });
            toEncodeAsBytes = System.Text.Encoding.ASCII.GetBytes(tokenPayload);
            string tokenPayloadEncode = Convert.ToBase64String(toEncodeAsBytes);



            string tokenSignature = GetTokenSignature(String.Format("{0}.{1}", tokenHeaderEncode, tokenPayloadEncode));

            string token = String.Format("{0}.{1}.{2}", tokenHeaderEncode, tokenPayloadEncode, tokenSignature);

            return token;
        }

        public bool CheckToken(string token)
        {
            return true;
        }

        public string GetTokenSignature(string token)
        {
            var sBuilder = new StringBuilder();
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] data = sha256Hash.ComputeHash(Encoding.ASCII.GetBytes(token));
                
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
            }
            return sBuilder.ToString();
        }
    }
}
