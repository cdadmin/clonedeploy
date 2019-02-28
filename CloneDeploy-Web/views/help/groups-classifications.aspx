<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#groups').addClass("nav-current");
            $('#groups-classifications').addClass("nav-current-sub");
        });
    </script>
    <h1>Groups->Image Classifications</h1>
    <p>
        Sets the Image Classifications for all computers in a group.  This does not take affect unless the image classifications box is checked in Computer Properties.  See <a href="<%= ResolveUrl("~/views/help/computers-classifications.aspx") %>">Computers->Image Classifications</a> for more info.
    </p>
</asp:Content>