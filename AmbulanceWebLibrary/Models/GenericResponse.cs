using System.Runtime.Serialization;

namespace AmbulanceWebLibrary.Models
{
    [DataContract]
    public class GenericResponse
    {
        [DataMember]
        int responseCode;
        [DataMember]
        string responseMessage;
        [DataMember]
        string serializedObject;

        public GenericResponse(int responseCode, string responseMessage, string serializedObject)
        {
            this.responseCode = responseCode;
            this.responseMessage = responseMessage;
            this.serializedObject = serializedObject;
        }
    }
}
