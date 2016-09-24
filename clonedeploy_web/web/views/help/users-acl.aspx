<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
     <script type="text/javascript">
         $(document).ready(function () {
             $('#users').addClass("nav-current");
             $('#users-acl').addClass("nav-current-sub");
         });
        </script>
    <h1>Users->ACLs</h1>
   
</asp:Content>

