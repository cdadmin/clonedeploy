<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#admin').addClass("nav-current");
            $('#admin-logs').addClass("nav-current-sub");
        });
    </script>
    <h1>Admin->Logs</h1>
    <p>The logs page allows you to view the exception log, multicast log, and the imaging log for computers that were not registred with CloneDeploy</p>
</asp:Content>