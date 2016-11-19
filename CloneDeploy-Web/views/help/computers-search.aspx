<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
     <script type="text/javascript">
         $(document).ready(function () {
             $('#computers').addClass("nav-current");
             $('#computers-search').addClass("nav-current-sub");
         });
        </script>
    <h1>Computers->Search</h1>
    <p>The Computers Search Page allows you to manage the computers that the current user has permissions for.  The search bar will locate 
        computers by name or mac address.  A wildcard is implied before and after the search string.  Multiple computers can be selected 
        and deleted from this screen.
        </p>
    <p>
        Search results can be filtered by Site, Building, Room, Group, Image, and result limit.
    </p>
</asp:Content>

