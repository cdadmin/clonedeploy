<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#users').addClass("nav-current");
            $('#users-neweditgroups').addClass("nav-current-sub");
        });
    </script>
    <h1>Users->New / Edit Groups</h1>
    <h3>Group Name</h3>
    <p>The display name for the group.</p>
    <h3>Group Role</h3>
    <p>The role that will be assigned to each user in the group. This value cannot be changed once submitted.</p>
    <h3>Use LDAP Group</h3>
    <p>
        If the group will validate users based on an LDAP group membership, this box must be checked. This value cannot
        be changed once submitted.
    </p>
    <h3>LDAP Group Name</h3>
    <p>The name of the LDAP group to use for membership validation.</p>
    <p>
        When using LDAP user groups, users do not need to be manually added to CloneDeploy, they will be automatically added
        with a successful login. This allows you to manage users based on LDAP groups, without the need to manually add and remove
        users from the CloneDeploy WebUI. Before you can use LDAP groups you must first setup LDAP authentication in Admin->Security.
    </p>

</asp:Content>