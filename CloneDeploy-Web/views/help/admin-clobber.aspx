<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
     <script type="text/javascript">
         $(document).ready(function () {
             $('#admin').addClass("nav-current");
             $('#admin-clobber').addClass("nav-current-sub");
         });
        </script>
    <h1>Admin->Clobber</h1>
   <p>When Clobber Mode is enabled, any computer that is pxe booted, either registered or non registered, will be re-imaged with the image defined with Clobber Mode.  Be extremely careful with this option, 
       you could easily create a very bad day for yourself.  It should only be used in a controlled isolated environment.
   </p>
</asp:Content>

