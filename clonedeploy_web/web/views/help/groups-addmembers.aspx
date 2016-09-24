<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
     <script type="text/javascript">
         $(document).ready(function () {
             $('#groups').addClass("nav-current");
             $('#groups-addmembers').addClass("nav-current-sub");
         });
        </script>
    <h1>Groups->Add Members</h1>
   
</asp:Content>

