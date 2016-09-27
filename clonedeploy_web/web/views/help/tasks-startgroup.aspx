<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
     <script type="text/javascript">
         $(document).ready(function () {
             $('#tasks').addClass("nav-current");
             $('#tasks-startgroup').addClass("nav-current-sub");
         });
        </script>
    <h1>Tasks->Start Group Task</h1>
   <p>Allows a user to start a deploy task for all group members using either a multicast or unicast.</p>
</asp:Content>

