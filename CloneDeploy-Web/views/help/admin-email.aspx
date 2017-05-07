<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#admin').addClass("nav-current");
            $('#admin-email').addClass("nav-current-sub");
        });
    </script>
    <h1>Admin->Email</h1>
    <p>If you want user's to be able to receive email notifications, you must first setup the email server settings. These are standard SMTP settings.</p>
</asp:Content>