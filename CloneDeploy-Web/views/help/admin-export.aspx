<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#admin').addClass("nav-current");
            $('#admin-export').addClass("nav-current-sub");
        });
    </script>
    <h1>Admin->Export</h1>
    <p>Exports CSV file of all computers, groups, and images. Not really that useful to be honest, most information is not exported.</p>
</asp:Content>