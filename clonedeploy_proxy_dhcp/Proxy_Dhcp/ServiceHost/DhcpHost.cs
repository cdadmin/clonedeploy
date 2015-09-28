using System.ServiceProcess;
using CloneDeploy_Proxy_Dhcp.Server;

namespace CloneDeploy_Proxy_Dhcp.ServiceHost
{
    public partial class DhcpHost : ServiceBase
    {
        private readonly DiscoveryServer _mServer;
        private readonly ProxyServer _mProxy;
        public bool DiscoverListen { get; set; }
        public bool ProxyListen { get; set; }
        public DhcpHost(DiscoveryServer server, ProxyServer proxy)
        {
            InitializeComponent();
            _mServer = server;
            _mProxy = proxy;
        }

        public void ManualStart(string[] args)
        {
            OnStart(args);
        }

        public void ManualStop()
        {
            OnStop();
        }

        protected override void OnStart(string[] args)
        {
            if (DiscoverListen)
                _mServer.Start();
            if (ProxyListen)
                _mProxy.Start();
        }

        protected override void OnStop()
        {
            if (DiscoverListen)
                _mServer.Stop();
            if (ProxyListen)
                _mProxy.Stop();
        }
    }
}
