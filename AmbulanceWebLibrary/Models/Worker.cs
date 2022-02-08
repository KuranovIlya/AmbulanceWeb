using System.Runtime.Serialization;

namespace AmbulanceWebLibrary.Models
{
    [DataContract]
    public class Worker
    {
        [DataMember]
        int id;

        [DataMember]
        string name;

        [DataMember]
        WorkTeam workTeam;

        public Worker(int id, string name, WorkTeam workTeam)
        {
            this.id = id;
            this.name = name;
            this.workTeam = workTeam;
        }
    }
}
