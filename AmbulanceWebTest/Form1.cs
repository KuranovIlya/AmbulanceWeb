using AmbulanceWebLibrary;
using System.Windows.Forms;

namespace AmbulanceWebTest
{
    public partial class Form1 : Form
    {
        WebServer webServer;
        public Form1()
        {
            InitializeComponent();
            webServer = new WebServer();
            webServer.Start();
        }
    }
}
