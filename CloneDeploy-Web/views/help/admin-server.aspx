<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#admin').addClass("nav-current");
            $('#admin-server').addClass("nav-current-sub");
        });
    </script>
    <h1>Admin->Server</h1>
    <h3>Server Ip / FQDN</h3>
    <p>
        The IP Address of your CloneDeploy server or the fully qualified domain name if using SSL. This value is used in various locations in CloneDeploy and must be correct for client to server
        communication.
    </p>
    <h3>Web Server Port</h3>
    <p>The port the CloneDeploy Web Application is running on. Defaults to 80 or 443 when using SSL. This setting doesn't change the port in you Web Server for you, you must do that first.</p>
    <h3>Manual Override Web Service</h3>
    <p>The Web Service value is automatically generated based on the Server Ip and Web Server Port values. If you want to manually set the value you must first check this box.</p>
    <h3>Web Service</h3>
    <p>Instructs the client computers where the client web service is.</p>
    <h3>Tftp Path</h3>
    <p>The base path for your tftp folder</p>
    <h3>Computer View</h3>
    <p>
        By default, anytime you search for computers in the WebUI, the initial view will display all computers before you change the search criteria. If you have too many computers, this can take a long
        time to load the page. Changing this value to search will not load any computers until you enter a search criteria.
    </p>
</asp:Content>