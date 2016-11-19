<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
     <script type="text/javascript">
         $(document).ready(function () {
             $('#tasks').addClass("nav-current");
             $('#tasks-startcomputer').addClass("nav-current-sub");
         });
        </script>
    <h1>Tasks->Start Computer Task</h1>
   <p>Allows a user to start computer tasks such as deploy, upload or permanent deploy.  A permanent deploy is a task that never closes.  Every time that computer is pxe booted it will be re-imaged.
       Task can be started by clicking the correct button for that computer or multiple computers can be selected and started using the actions drop down.
   </p>
</asp:Content>

