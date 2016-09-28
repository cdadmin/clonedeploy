<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
     <script type="text/javascript">
         $(document).ready(function () {
             $('#admin').addClass("nav-current");
             $('#admin-dp').addClass("nav-current-sub");
         });
        </script>
    <h1>Admin->Distribution Points</h1>
   <p>Distribution points are simply SMB shares that client computers will connect to for uploading and deploying images.  They are still under development and do not yet work completely as intended if
       multiple distribution points are used.  If using multiple distribution points assigned to computers via a Site, Building, or Room, you should note that the image files do not get replicated among the 
       distribution points.  You must do that with your own method.  Also, uploads will always go to the primary distribution point.
   </p>
</asp:Content>

