<%@ Application Language="C#" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="CloneDeploy_Common" %>
<%@ Import Namespace="log4net" %>
<%@ Import Namespace="log4net.Config" %>

<script runat="server">

    private void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown
    }

    private void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs
    }

/**/

    private void Application_Start(object sender, EventArgs e)
    {
       
        var logPath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "private" +
                      Path.DirectorySeparatorChar + "logs" + Path.DirectorySeparatorChar + "CloneDeployFE.log";
        GlobalContext.Properties["LogFile"] = logPath;
        XmlConfigurator.Configure();
    }

    private void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.
    }

    private void Session_Start(object sender, EventArgs e)
    {
        ApplicationServers.Configure();
        // Code that runs when a new session is started
    }

</script>