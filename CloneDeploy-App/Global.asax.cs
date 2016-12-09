using System.IO;
using System.Web;
using System.Web.Http;
using log4net;
using log4net.Config;

namespace CloneDeploy_App
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            var logPath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "private" +
                             Path.DirectorySeparatorChar + "logs" + Path.DirectorySeparatorChar + "CloneDeployApplication.log";
            GlobalContext.Properties["LogFile"] =logPath;
            XmlConfigurator.Configure();
        }
    }
}
