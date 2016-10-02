<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
     <script type="text/javascript">
         $(document).ready(function () {
             $('#global').addClass("nav-current");
             $('#global-sites').addClass("nav-current-sub");
         });
        </script>
    <h1>Global->Sites</h1>
   <p>Sites are an optional way to represent a physical grouping of computers.  Those computers can be then be assigned their own distribution point for downloading images.  </p>
</asp:Content>

