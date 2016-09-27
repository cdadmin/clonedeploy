<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
     <script type="text/javascript">
         $(document).ready(function () {
             $('#tasks').addClass("nav-current");
             $('#tasks-all').addClass("nav-current-sub");
         });
        </script>
    <h1>Tasks->All</h1>
   <p> <p>Displays all active tasks that have been started by the current user.  Administrators see tasks created from all users.  Individual tasks can also be cancelled from this page.  Administrators have
       an extra action on this page called Cancel All Tasks.  This can be useful to clean things up.  It clears all multicast processes, database entries, and boot files for all tasks.
       </p>
</asp:Content>

