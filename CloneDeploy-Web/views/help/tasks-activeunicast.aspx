<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#tasks').addClass("nav-current");
            $('#tasks-activeunicast').addClass("nav-current-sub");
        });
    </script>
    <h1>Tasks->Active Unicasts</h1>
    <p>Displays all active upload and deploy tasks that have been started by the current user. Administrators see tasks created from all users. Individual tasks can also be cancelled from this page.</p>
</asp:Content>