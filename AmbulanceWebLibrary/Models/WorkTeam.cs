using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AmbulanceWebLibrary
{
    [DataContract]
    public class WorkTeam
    {
        [DataMember]
        int id;
        [DataMember]
        string name;

        public WorkTeam(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
