using AmbulanceWebLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;

namespace AmbulanceWebLibrary
{
    [ServiceContract]
    public interface IService
    {
        //[WebInvoke(Method = "GET",
        //   RequestFormat = WebMessageFormat.Json,
        //   ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        [WebGet]
        List<WorkTeam> WorkTeams();

        [OperationContract]
        [WebGet]
        AuthResponse Authentication(string login, string password, int team_id);

        [OperationContract]
        [WebGet]
        bool TokenVerification(string token);

        [OperationContract]
        [WebGet]
        List<Call> AmbulanceCalls(string token, int call_type);
    }

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class SimplexAmbulanceService : IService
    {
        SQLServer sql = new SQLServer();

        public List<Call> AmbulanceCalls(string token, int call_type)
        {
            TokenService tokenService = TokenService.getInstance();
            List<Call> calls = new List<Call>();
            try
            {
                bool goodAuth = tokenService.CheckToken(token);
                //goodAuth = true;
                if (goodAuth)
                {
                    calls.AddRange(sql.GetAmbCalls(call_type, tokenService.GetTeamFromToken(token)));                    
                }
                return calls;
            } catch
            {
                return new List<Call>();
            }                 
        }

        public AuthResponse Authentication(string login, string password, int team_id)
        {
            Worker worker = sql.WorkerAuthentication(login, password ?? "", team_id);
            if (worker != null)
            {
                TokenService tokenService = TokenService.getInstance();
                string token = tokenService.GenerateToken(worker);
                
                string workerJson = JsonConvert.SerializeObject(worker);
                return new AuthResponse(token, workerJson);
            }
            return new AuthResponse(null, null);
        }

        public bool TokenVerification(string token)
        {
            TokenService tokenService = TokenService.getInstance();
            bool result = tokenService.CheckToken(token);
            return result;
        }

        public List<WorkTeam> WorkTeams()
        {
            List<WorkTeam> teams = sql.GetAllTeams();

            return teams;
        }

    }
    public class WebServer
    {
        Uri uri;
        ServiceHost host;
        TokenService tokenService = new TokenService();


        public bool Start()
        {
            try
            {
                //Инициализация сервиса и открытия порта для запросов
                uri = new Uri("http://localhost:8100/");
                host = new ServiceHost(typeof(SimplexAmbulanceService), uri);
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                host.Description.Behaviors.Add(smb);

                host.AddServiceEndpoint(typeof(IService), new BasicHttpBinding(), "Soap");
                ServiceEndpoint endpoint = host.AddServiceEndpoint(typeof(IService), new WebHttpBinding(), "Web");
                endpoint.Behaviors.Add(new WebHttpBehavior());

                host.Open();
            }
            catch
            {
            }
            return true;
        }
    }
}
