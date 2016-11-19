<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
     <script type="text/javascript">
         $(document).ready(function () {
             $('#users').addClass("nav-current");
             $('#users-groupmgmt').addClass("nav-current-sub");
         });
        </script>
    <h1>Users->Group Based Computer Management</h1>
   <p>If you want to limit a user to only have control over specific groups and computers this is where you do it.  
       Anytime you have checked at least one group, this feature is enabled, otherwise it is disabled.  
      By default the ACLs for groups and computers apply to all groups and computers.  This setting would disable the global
       permission and assign those permissions only to the selected groups and group members.
   </p>
</asp:Content>

