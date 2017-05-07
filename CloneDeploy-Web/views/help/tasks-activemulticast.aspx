<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#tasks').addClass("nav-current");
            $('#tasks-activemulticast').addClass("nav-current-sub");
        });
    </script>
    <h1>Tasks->Active Multicasts</h1>
    <p>Displays all active multicast tasks that have been started by the current user. Administrators see tasks created from all users. Multicast tasks can also be cancelled from this page.</p>
</asp:Content>