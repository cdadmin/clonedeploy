<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Http" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="Helpers" %>
<%@ Import Namespace="Swashbuckle.Application" %>


<script runat="server">
 
    protected static string GetXmlCommentsPath()
    {
        return System.String.Format(@"{0}\bin\WebApiSwagger.XML", System.AppDomain.CurrentDomain.BaseDirectory);
    }
    
    void Application_Start(object sender, EventArgs e)
    {
        GlobalConfiguration.Configuration
          .EnableSwagger(c =>
          {
              c.SingleApiVersion("v1", "CloneDeploy Web API");

          })
          .EnableSwaggerUi();
        
        GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
        RouteTable.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "{controller}/{action}/{id}",
            defaults: new {id = System.Web.Http.RouteParameter.Optional}
            );
       
    }

    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    {
        HttpContext ctx = HttpContext.Current;
        StringBuilder sb = new StringBuilder();
        sb.Append(ctx.Request.Url.ToString() + System.Environment.NewLine);
        sb.Append("Source:" + System.Environment.NewLine + ctx.Server.GetLastError().Source.ToString());
        sb.Append("Message:" + System.Environment.NewLine + ctx.Server.GetLastError().Message.ToString());
        sb.Append("Stack Trace:" + System.Environment.NewLine + ctx.Server.GetLastError().StackTrace.ToString());
        Logger.Log(sb.ToString());
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
