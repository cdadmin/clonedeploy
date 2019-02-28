<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#users').addClass("nav-current");
            $('#users-imagemgmt').addClass("nav-current-sub");
        });
    </script>
    <h1>Users->Image Management</h1>
    <p>
        If you want to limit a user to only have control over specific images this is where you do it.
        By default the ACLs for images apply to all images. This setting would disable the global
        permission and assign those permissions only to the selected images.
    </p>
</asp:Content>