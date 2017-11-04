<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#groups').addClass("nav-current");
            $('#groups-multicast').addClass("nav-current-sub");
        });
    </script>
    <h1>Groups->Multicast Options</h1>
    <p>
        Used to set the Image and Image Profile that are used when multicasting a group of computers. This image may or may not be the same as the image currently assigned to the computer.
        Setting this option does not change the image assigned to the computers in the group. Having a separate image set specifically for multicasting gives you more flexibility in how you use
        your groups.  Finally, an option for cluster group can be used to assign a group's multicast session to a specific multicast server.
    </p>
</asp:Content>