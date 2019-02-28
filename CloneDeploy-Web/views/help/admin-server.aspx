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
      <h3>Server Identifier</h3>
    <p>The name of the server displayed in the top right corner.  Only there to remind you which server you are connected to.</p>
    <h3>API Port</h3>
    <p>The port the CloneDeploy Web Application is running on. Defaults to 80 or 443 when using SSL. This setting doesn't change the port in you Web Server for you, you must do that first.</p>
    <h3>Manual Override API Url</h3>
    <p>The Web Service value is automatically generated based on the Server Ip and Web Server Port values. If you want to manually set the value you must first check this box.</p>
    <h3>Base Url</h3>
    <p>Instructs the client computers where the client API is.</p>
    <h3>Ipxe Use SSL</h3>
    <p>When ssl is enabled, the ipxe boot menus are not updated to use SSL because of compatibility issues.  If you want to enable SSL on ipxe also, check this box.</p>
      <h3>Tftp Server IP</h3>
    <p>This setting is only used when using CloneDeploy Proxy DHCP with clustering.  Each server identifies it's tftp server ip and the proxy will monitor the status of each tftp server.</p>
    <h3>Tftp Path</h3>
    <p>The base path for your tftp folder.  This is needed so CloneDeploy knows where to create the pxe boot files.</p>
    <h3>Computer View</h3>
    <p>
        By default, anytime you search for computers in the WebUI, the initial view will display all computers before you change the search criteria. If you have too many computers, this can take a long
        time to load the page. Changing this value to search will not load any computers until you enter a search criteria.
    </p>
    <h3>Monitor Secondary Servers</h3>
    <p>If CloneDeploy is in clustered mode, the primary can check on the secondaries and send an email alert out if one is not responding.  This option can be enabled or disabled here.</p>

    <h3>Monitor Interval</h3>
    <p>The interval time in minutes that the secondary server check should be performed</p>

    <h3>Task Timeout</h3>
    <p>The time in minutes before a task should automatically be cancelled if no updates are received.  This keeps the queue from stacking up in case a computer fails.</p>
</asp:Content>