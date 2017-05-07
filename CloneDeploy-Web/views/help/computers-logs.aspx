<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#computers').addClass("nav-current");
            $('#computers-logs').addClass("nav-current-sub");
        });
    </script>
    <h1>Computers->Logs</h1>
    <p>
        The logs page displays the imaging logs for both deploy and upload tasks. If a computer is registered with CloneDeploy and On Demand Mode
        was used, the logs will still show up in this location. The logs can also be exported for use in the Forums.
    </p>
</asp:Content>