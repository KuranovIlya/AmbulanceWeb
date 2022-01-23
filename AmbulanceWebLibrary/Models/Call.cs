using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace AmbulanceWebLibrary.Models
{
    [DataContract]
    public class Call
    {
        [DataMember]
        int id;
    }
}
