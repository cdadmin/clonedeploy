<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#admin').addClass("nav-current");
            $('#admin-alternateips').addClass("nav-current-sub");
        });
    </script>
    <h1>Admin->Server->Alternate Server Ips</h1>
    <p>
      Alternate Server Ips are used when you need to image multiple segmented networks with a single CloneDepoy server.  The additional ip's that refer to the server a specified here.  The CloneDeploy server would need
        an interface belonging to each network, which obviously bridges those networks.  In terms of security, probably not the best idea to bridge two networks that were meant to be separate, but I'll leave that to you.
    </p>
</asp:Content>