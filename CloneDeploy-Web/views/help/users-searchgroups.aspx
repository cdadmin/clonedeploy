<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#users').addClass("nav-current");
            $('#users-searchgroups').addClass("nav-current-sub");
        });
    </script>
    <h1>Users->Search Groups</h1>
    <p>
        The search user groups displays the list of all current user groups. Multiple user groups can be deleted from this page.
        Deleting a user group does not delete the users within the group. The search bar implies a wildcard before and after the search
        string.
    </p>
    </p>
</asp:Content>