using AmbulanceWebLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
