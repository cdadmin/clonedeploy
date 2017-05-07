<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#groups').addClass("nav-current");
            $('#groups-newedit').addClass("nav-current-sub");
        });
    </script>
    <h1>Groups->New / Edit</h1>
    <h3>Name</h3>

    <p>The name of the Group.</p>
    <h3>Type</h3>

    <p>There are two types of groups. Standard and Smart. You could think of it as static and dynamic. Standard groups are manually assigned computers where as Smart groups have computers dynamically added to them based on a certain criteria. This option cannot be changed once set.</p>
    <h3>Description (Optional)</h3>

    <p>A description of the group for your own use.</p>
</asp:Content>