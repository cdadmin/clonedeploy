<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#users').addClass("nav-current");
            $('#users-acl').addClass("nav-current-sub");
        });
    </script>
    <h1>Users->ACLs</h1>
    <p>
        Here you can set specific permissions for a user or user group. They are mostly self explanatory. You’ll notice the
        smart group permission does not have a box for read, delete, or search. They are inherited from Group permission.
        Administrators do not use ACL’s. If you are not using Group Based Computer Management or Image Management, then these
        permissions are globally applied for this user to all computer, groups, and images. Otherwise the permissions are applied
        only to the computers, groups, and images you have been assigned. The user will have no permissions for the computers, groups, and images
        they have not been assigned.
    </p>
</asp:Content>