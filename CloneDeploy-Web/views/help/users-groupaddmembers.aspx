<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#users').addClass("nav-current");
            $('#users-groupaddmembers').addClass("nav-current-sub");
        });
    </script>
    <h1>Users->Add Group Members</h1>
    <p>
        Allows you to select which users are part of the group. A user can only be a member of a single group. Users cannot
        be added to LDAP groups, they are always managed by the directory server.
    </p>
</asp:Content>