<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
     <script type="text/javascript">
         $(document).ready(function () {
             $('#users').addClass("nav-current");
             $('#users-groupremovemembers').addClass("nav-current-sub");
         });
        </script>
    <h1>Users->Remove Group Members</h1>
   <p>Allows you to remove existing group members from the group.  If a user is removed, that user will have the current permission assigned to the
       group when the user was removed.
   </p>
</asp:Content>

