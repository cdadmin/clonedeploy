<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#users').addClass("nav-current");
            $('#users-resetpass').addClass("nav-current-sub");
        });
    </script>
    <h1>Users->Reset Password</h1>
    <p>This page is available to users that are not administrators.  It provides the ability to change password and update email and notification settings.</p>
</asp:Content>