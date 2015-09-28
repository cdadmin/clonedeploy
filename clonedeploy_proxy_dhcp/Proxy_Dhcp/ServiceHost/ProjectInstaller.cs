using System.ComponentModel;
using System.Configuration.Install;

namespace CloneDeploy_Proxy_Dhcp.ServiceHost
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }
    }

}