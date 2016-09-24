<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
     <script type="text/javascript">
         $(document).ready(function () {
             $('#tasks').addClass("nav-current");
             $('#tasks-activepermanent').addClass("nav-current-sub");
         });
        </script>
    <h1>Tasks->Active Permanent Tasks</h1>
   
</asp:Content>

