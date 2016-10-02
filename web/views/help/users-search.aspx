<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
     <script type="text/javascript">
         $(document).ready(function () {
             $('#users').addClass("nav-current");
             $('#users-search').addClass("nav-current-sub");
         });
        </script>
    <h1>Users->Search</h1>
   <p>The search screen is the default view when you select Users. It displays all of the current users. 
       The search bar implies a wildcard before and after the string you enter. Leave it blank for all users. 
       You can also use this view to delete multiple users as well as view additional options for an individual user.</p>
</asp:Content>

