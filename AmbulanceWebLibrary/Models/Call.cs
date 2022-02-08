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
        int amb_call_id;
        [DataMember]
        string amb_call_lev_name; 
        [DataMember]
        string amb_call_type_name; 
        [DataMember]
        string amb_call_datetime; 
        [DataMember]
        string amb_call_state_name; 
        [DataMember]
        string patient_fio;
        [DataMember]
        string amb_call_patient_sex;
        [DataMember]
        string amb_call_patient_born_date;
        [DataMember]
        int amb_ride_id;
        [DataMember]
        string call_address;
        [DataMember]
        string amb_call_phone_num;

        public Call(int amb_call_id, string amb_call_lev_name, string amb_call_type_name, string amb_call_datetime, string amb_call_state_name, string patient_fio, string amb_call_patient_sex, string amb_call_patient_born_date, int amb_ride_id, string call_address, string amb_call_phone_num)
        {
            this.amb_call_id = amb_call_id;
            this.amb_call_lev_name = amb_call_lev_name;
            this.amb_call_type_name = amb_call_type_name;
            this.amb_call_datetime = amb_call_datetime;
            this.amb_call_state_name = amb_call_state_name;
            this.patient_fio = patient_fio;
            this.amb_call_patient_sex = amb_call_patient_sex;
            this.amb_call_patient_born_date = amb_call_patient_born_date;
            this.amb_ride_id = amb_ride_id;
            this.call_address = call_address;
            this.amb_call_phone_num = amb_call_phone_num;
        }
    }
}
