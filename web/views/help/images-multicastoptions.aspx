<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
     <script type="text/javascript">
         $(document).ready(function () {
             $('#images').addClass("nav-current");
             $('#images-deployoptions').addClass("nav-current-sub");
         });
        </script>
    <h1>Images->Multicast Options</h1>
   <p>Allows you to set custom arguments on the server and client to control multicast behavior.  More information is available under the Multicast Arguments documentation.</p>
</asp:Content>

