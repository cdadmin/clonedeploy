<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#groups').addClass("nav-current");
            $('#groups-munki').addClass("nav-current-sub");
        });
    </script>
    <h1>Groups->Munki</h1>
    <p>
        Sets the Munki Templates for all computers in a group. See <a href="<%= ResolveUrl("~/views/help/computers-munki.aspx") %>">Computers->Munki Templates</a> for more info.
    </p>
</asp:Content>