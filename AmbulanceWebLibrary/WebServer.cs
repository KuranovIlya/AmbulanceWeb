using AmbulanceWebLibrary.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;

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
        string Authentication(string login, string password, int team_id);
    }

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class SimplexAmbulanceService : IService
    {
        SQLServer sql = new SQLServer();
        TokenService tokenService = new TokenService();

        public string Authentication(string login, string password, int team_id)
        {
            // Worker worker = sql.WorkerAuthentication("Юров", "", 5);
            Worker worker = sql.WorkerAuthentication(login, password, team_id);

            string token = tokenService.GenerateToken(worker);

            return token;
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
            catch (Exception exc)
            {
            }
            return true;
        }
    }
}
