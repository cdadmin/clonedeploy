<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#users').addClass("nav-current");
            $('#users-newedit').addClass("nav-current-sub");
        });
    </script>
    <h1>Users->New / Edit</h1>
    <p>A User is anyone that needs access to either the Web Interface or Client Boot Image</p>

    <h3>
        Name
    </h3>

    <p>
        The name the user will login with. If using LDAP authentication it must match the value returned from the Authentication Attribute
        assigned in Admin->Security
    </p>


    <h3>Role</h3>

    <p>
        There are two types of Roles. Administrators and Users. Administrators have complete control over the system and cannot be
        controlled by ACLs. Users have as much power as you want to give them using ACL’s. By default a new user only has search
        permissions, and cannot really do anything until more permissions are applied.
    </p>
    <h3>Use LDAP Authentication</h3>
    <p>If the user will be authenticated via LDAP, this box must be checked. This value cannot be changed once submitted. Before you can use LDAP groups you must first setup LDAP authentication in Admin->Security.</p>

    <h3>Email (Optional)</h3>

    <p>If Email notifications are enabled in Admin->Email, you can set the user’s email here</p>


    <h3>Security Token</h3>

    <p>
        This is authorization key that is returned when logging in to the client boot image. It is used to Authorize calls to the
        client web service. If you feel that a user’s token is ever compromised you can generate a new one.
    </p>


    <h3>Notifications</h3>

    <p>The last 4 check boxes set the type of email notifications you want that user to receive.</p>
</asp:Content>