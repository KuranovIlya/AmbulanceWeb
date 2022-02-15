using AmbulanceWebLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AmbulanceWebLibrary
{
    class SQLServer : IDatabase
    {
        string connectionString;

        public SQLServer()
        {
            connectionString = "Server=localhost\\SQLEXPRESS;Database=AmbulanceTest1;User Id=sa;Password=kuranov;";
        }

        public SQLServer(string host, string user, string dbname, string password)
        {
            connectionString = String.Format("Server={0};Database={1};User Id={2};Password={3};", host, dbname, user, password);
        }

        public List<WorkTeam> GetAllTeams()
        {
            List<WorkTeam> workTeams = new List<WorkTeam>();

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query;
                    query = "SELECT WORK_TEAM_ID, WORK_TEAM_NAME FROM WORK_TEAM where WORK_TEAM_TYPE_ID = 3";

                    using (var command = new SqlCommand(query, conn))
                    {
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            workTeams.Add(new WorkTeam(reader.GetInt32(0), reader.GetString(1)));
                        }
                        reader.Close();
                    }
                }

                return workTeams;
            } catch
            {
                return workTeams;
            }
        }

        public List<Call> GetAmbCalls(int call_type, int team_id)
        {
            List<Call> calls = new List<Call>();

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    string query;
                    query = @"select AC.AMB_CALL_ID, ACL.AMB_CALL_LEV_NAME, ACT.AMB_CALL_TYPE_NAME, AC.AMB_CALL_DATETIME, ACS.AMB_CALL_STATE_NAME, 
                        dbo.GetFIO(AC.AMB_CALL_PATIENT_SURNAME, AC.AMB_CALL_PATIENT_NAME, AC.AMB_CALL_PATIENT_PATRONIMIC) as PATIENT_FIO, 
                        AC.AMB_CALL_PATIENT_SEX, AC.AMB_CALL_PATIENT_BORN_DATE, AR.AMB_RIDE_ID, dbo.SHORT_ADDRESS_NAME(A.ADDR_ID, 1, 2, 0, 0) AS CALL_ADDRESS, AC.AMB_CALL_PHONE_NUM 
                        from AMB_CALL AC 
                        inner join AMB_CALL_LEVEL ACL on AC.AMB_CALL_LEV_ID = ACL.AMB_CALL_LEV_ID 
                        inner join AMB_CALL_TYPE ACT on AC.AMB_CALL_TYPE_ID = ACT.AMB_CALL_TYPE_ID 
                        inner join AMB_CALL_STATE ACS on AC.AMB_CALL_STATE_ID = ACS.AMB_CALL_STATE_ID 
                        inner join ADDRESS A on AC.ADDR_ID = A.ADDR_ID 
                        left join AMB_RIDE AR on AR.AMB_CALL_ID = AC.AMB_CALL_ID 
                        where {0}";

                    switch(call_type)
                    {
                        case 1:
                            {
                                query = String.Format(query, "AC.AMB_CALL_STATE_ID = 1 and dateadd(dd,-2,getdate()) <= AC.AMB_CALL_DATETIME");
                                break;
                            }
                        case 2:
                            {
                                query = String.Format(query, "AR.WORK_TEAM_ID=@team_id and (AC.AMB_CALL_STATE_ID = 1 or AC.AMB_CALL_STATE_ID = 2) and dateadd(dd,-5,getdate()) <= AC.AMB_CALL_DATETIME");
                                break;
                            }
                        case 3:
                            {
                                query = String.Format(query, "AR.WORK_TEAM_ID=@team_id and AC.AMB_CALL_STATE_ID != 1 and AC.AMB_CALL_STATE_ID != 2 and dateadd(ww,-1,getdate()) <= AC.AMB_CALL_DATETIME");
                                break;
                            }
                    }

                    query = @"select AC.AMB_CALL_ID, ACL.AMB_CALL_LEV_NAME, ACT.AMB_CALL_TYPE_NAME, AC.AMB_CALL_DATETIME, ACS.AMB_CALL_STATE_NAME, 
                        dbo.GetFIO(AC.AMB_CALL_PATIENT_SURNAME, AC.AMB_CALL_PATIENT_NAME, AC.AMB_CALL_PATIENT_PATRONIMIC) as PATIENT_FIO, 
                        AC.AMB_CALL_PATIENT_SEX, AC.AMB_CALL_PATIENT_BORN_DATE, AR.AMB_RIDE_ID, dbo.SHORT_ADDRESS_NAME(A.ADDR_ID, 1, 2, 0, 0) AS CALL_ADDRESS, AC.AMB_CALL_PHONE_NUM 
                        from AMB_CALL AC 
                        inner join AMB_CALL_LEVEL ACL on AC.AMB_CALL_LEV_ID = ACL.AMB_CALL_LEV_ID 
                        inner join AMB_CALL_TYPE ACT on AC.AMB_CALL_TYPE_ID = ACT.AMB_CALL_TYPE_ID 
                        inner join AMB_CALL_STATE ACS on AC.AMB_CALL_STATE_ID = ACS.AMB_CALL_STATE_ID 
                        inner join ADDRESS A on AC.ADDR_ID = A.ADDR_ID 
                        left join AMB_RIDE AR on AR.AMB_CALL_ID = AC.AMB_CALL_ID";

                    using (var command = new SqlCommand(query, conn))
                    {
                        if (call_type == 3)
                        {
                            command.Parameters.Add(new SqlParameter("team_id", team_id));
                        }
                        
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            string amb_call_datetime;
                            if (!reader.IsDBNull(3))
                            {
                                amb_call_datetime = reader.GetDateTime(3).ToString();
                            }
                            else
                            {
                                amb_call_datetime = "";
                            }
                            string patient_fio;
                            if (!reader.IsDBNull(5))
                            {
                                patient_fio = reader.GetString(5);
                            }
                            else
                            {
                                patient_fio = "";
                            }
                            string amb_call_patient_sex;
                            if (!reader.IsDBNull(6))
                            {
                                amb_call_patient_sex = reader.GetString(6);
                            }
                            else
                            {
                                amb_call_patient_sex = "Неизв.";
                            }
                            string amb_call_patient_born_date;
                            if (!reader.IsDBNull(7))
                            {
                                amb_call_patient_born_date = reader.GetDateTime(7).ToString();
                            }
                            else
                            {
                                amb_call_patient_born_date = "";
                            }
                            int amb_ride_id;
                            if (!reader.IsDBNull(8))
                            {
                                amb_ride_id = reader.GetInt32(8);
                            }
                            else
                            {
                                amb_ride_id = -1;
                            }
                            string call_address;
                            if (!reader.IsDBNull(9))
                            {
                                call_address = reader.GetString(9);
                            }
                            else
                            {
                                call_address = "";
                            }
                            string amb_call_phone_num;
                            if (!reader.IsDBNull(10))
                            {
                                amb_call_phone_num = reader.GetString(10);
                            }
                            else
                            {
                                amb_call_phone_num = "";
                            }
                            calls.Add(new Call(reader.GetInt32(0), 
                                reader.GetString(1), 
                                reader.GetString(2),
                                amb_call_datetime, 
                                reader.GetString(4),
                                patient_fio,
                                amb_call_patient_sex,
                                amb_call_patient_born_date,
                                amb_ride_id,
                                call_address,
                                amb_call_phone_num));
                        }
                        reader.Close();
                    }
                }


                return calls;
            } catch
            {
                return null;
            } 


            
        }

        public Worker WorkerAuthentication(string login, string password, int team_id)
        {
            Worker worker = null;

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query;
                    query = "SELECT W.WORK_ID, W.WORK_SURNAME, W.WORK_NAME, W.WORK_PATRONIMIC, WT.WORK_TEAM_ID, WT.WORK_TEAM_NAME FROM SEC_USER SU INNER JOIN WORKER W ON SU.WORK_ID=W.WORK_ID INNER JOIN WORK_TEAM_MEMBER WTM ON WTM.WORK_ID=W.WORK_ID INNER JOIN WORK_TEAM WT ON WTM.WORK_TEAM_ID=WT.WORK_TEAM_ID WHERE SU.SEC_USER_LOGIN LIKE @login AND SU.SEC_USER_PASSWORD LIKE @password AND WT.WORK_TEAM_ID=@team_id";

                    using (var command = new SqlCommand(query, conn))
                    {
                        command.Parameters.Add(new SqlParameter("login", login));
                        command.Parameters.Add(new SqlParameter("password", password));
                        command.Parameters.Add(new SqlParameter("team_id", team_id));
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            worker = new Worker(reader.GetInt32(0), String.Format("{0} {1} {2}", reader.GetString(1), reader.GetString(2), reader.GetString(3)), new WorkTeam(reader.GetInt32(4), reader.GetString(5)));
                        }
                        reader.Close();
                    }
                }

                return worker;
            }
            catch 
            {
                return null;
            }
        }




    }
}
