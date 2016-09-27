<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#groups').addClass("nav-current");
            $('#groups-search').addClass("nav-current-sub");
        });
    </script>
    <h1>Groups->Search</h1>
    <p>The search screen is the default view when you select Groups. It displays all of the groups that a user has permission to view. The search bar implies 
        a wildcard before and after the string you enter. Leave it blank for all groups. You can also use this view to delete multiple groups as well as view additional options for an individual group.</p>
</asp:Content>

