<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
     <script type="text/javascript">
         $(document).ready(function () {
             $('#groups').addClass("nav-current");
             $('#groups-properties').addClass("nav-current-sub");
         });
        </script>
    <h1>Groups->Computer Properties</h1>
   <p>Allows you to set the properties of all the computers in your group to a specific value.  Any field that is checked when you click Update will update all computers 
       in that group to that value.  Anything that is not checked remains unchanged on the computers.  Checking the Set As Default For New Group Members box will automatically set 
       the values selected to any new computers added to the group.</p>
</asp:Content>

