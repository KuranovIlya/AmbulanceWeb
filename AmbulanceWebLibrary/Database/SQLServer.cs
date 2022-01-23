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
            connectionString = "Server=localhost\\SQLEXPRESS_12;Database=AmbulanceTest1;User Id=sa;Password=kuranov;";
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
                    query = "SELECT WORK_TEAM_ID, WORK_TEAM_NAME FROM WORK_TEAM";

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
            } catch(Exception ex)
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
                    query = "";

                    using (var command = new SqlCommand(query, conn))
                    {
                        command.Parameters.Add(new SqlParameter("team_id", team_id));
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Worker worker = new Worker(reader.GetInt32(0), String.Format("{0} {1} {2}", reader.GetString(1), reader.GetString(2), reader.GetString(3)), new WorkTeam(reader.GetInt32(4), reader.GetString(5)));
                        }
                        reader.Close();
                    }
                }


                return calls;
            } catch(Exception ex)
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
            catch (Exception ex)
            {
                return worker;
            }
        }


    }
}
