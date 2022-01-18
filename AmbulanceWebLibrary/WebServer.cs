using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace AmbulanceWebLibrary
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        [WebGet]
        //[WebInvoke(Method = "GET",
        //   RequestFormat = WebMessageFormat.Json,
        //   ResponseFormat = WebMessageFormat.Json)]
        string CheckConnection();
    }

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class SimplexAmbulanceService : IService
    {
        public string CheckConnection()
        {
            return "123";
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
