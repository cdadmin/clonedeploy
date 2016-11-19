<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
     <script type="text/javascript">
         $(document).ready(function () {
             $('#admin').addClass("nav-current");
             $('#admin-multicast').addClass("nav-current-sub");
         });
        </script>
    <h1>Admin->multicast</h1>
   <p>These settings are globally applied to all multicasts.  Arguments can be specified to both the server and client to try and tweak the multicast.  Each multicast session also uses 2 ports.  Those 2
       ports may be anwywhere between the start and end port, be sure your firewall has this range opened.  Finally a multicast image can be decrompressed on the server before the image is sent, result in
       a larger data transfer, or on the client after the image is sent.
   </p>
</asp:Content>

