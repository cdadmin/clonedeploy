<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
     <script type="text/javascript">
         $(document).ready(function () {
             $('#admin').addClass("nav-current");
             $('#admin-client').addClass("nav-current-sub");
         });
        </script>
    <h1>Admin->Client</h1>
   <h3>Queue Size</h3>
    <p>The number of concurrent computers that can be imaged.  The rest will sit in a queue until one is finished.  Does not apply to multicast, on demand, and uploads.  This is globally applied to
        all distribution points, eventually this setting will be moved to a per distribution point method.
    </p>
    <h3>Global Computer Arguments</h3>
    <p>Any additional arguments that need passed to all computers can go here.  This only applies to the Linux Imaging Environment.  Arguments set here are passed in before the Kernel is loaded so the
        kernel can read them also.
    </p>
</asp:Content>

