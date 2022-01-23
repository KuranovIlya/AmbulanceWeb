using AmbulanceWebLibrary.Models;
using System.Collections.Generic;

namespace AmbulanceWebLibrary
{
    interface IDatabase
    {
        List<WorkTeam> GetAllTeams();

        List<Call> GetAmbCalls(int call_type, int team_id);

        Worker WorkerAuthentication(string login, string password, int team_id);

    }
}
