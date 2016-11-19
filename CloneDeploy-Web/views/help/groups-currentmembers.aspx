<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
     <script type="text/javascript">
         $(document).ready(function () {
             $('#groups').addClass("nav-current");
             $('#groups-current').addClass("nav-current-sub");
         });
        </script>
    <h1>Groups->Current Members</h1>
   <p>Shows a list of all computers that are currently a member of the smart group.  Computers cannot be manually removed or added from a smart group, it is always based on the criteria.  This page
       is only visible for smart groups.
   </p>
</asp:Content>

