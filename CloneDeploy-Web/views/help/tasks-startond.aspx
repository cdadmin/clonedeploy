<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#tasks').addClass("nav-current");
            $('#tasks-startond').addClass("nav-current-sub");
        });
    </script>
    <h1>Tasks->Start On Demand Multicast</h1>
    An On Demand multicast is a way to quickly start a multicast without needing to put computers into a group. To use this feature you first start the task
    in the WebUI, then boot each client computer into On Demand Mode and select multicast. Finally select the corresponding session number to join. After you
    have connected all the clients simply press Enter on any client to start the multicast. Alternatively, you can specify a client count in the WebUI in which case
    the multicast will automatically start after that number of clients have connected. The client count field is optional.
</asp:Content>