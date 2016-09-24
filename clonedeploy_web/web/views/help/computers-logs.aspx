<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
     <script type="text/javascript">
         $(document).ready(function () {
             $('#computers').addClass("nav-current");
             $('#computers-logs').addClass("nav-current-sub");
         });
        </script>
    <h1>Computers->Logs</h1>
   
</asp:Content>

