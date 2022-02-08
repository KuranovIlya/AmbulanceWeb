using System.Runtime.Serialization;

namespace AmbulanceWebLibrary.Models
{
    [DataContract]
    public class AuthResponse
    {
        [DataMember]
        string token;
        [DataMember]
        string worker;

        public AuthResponse(string token, string worker)
        {
            this.token = token;
            this.worker = worker;
        }
    }
}
