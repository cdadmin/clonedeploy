<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
     <script type="text/javascript">
         $(document).ready(function () {
             $('#tasks').addClass("nav-current");
             $('#tasks-startcomputer').addClass("nav-current-sub");
         });
        </script>
    <h1>Tasks->Start Computer Task</h1>
   
</asp:Content>

