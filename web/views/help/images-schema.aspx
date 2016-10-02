<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
     <script type="text/javascript">
         $(document).ready(function () {
             $('#images').addClass("nav-current");
             $('#images-schema').addClass("nav-current-sub");
         });
        </script>
    <h1>Images->Schema</h1>
   <p>Displays the Schema for the image.  When deploying an image, all of the partitioning decisions are based off the information found in this file.  This view is only for informational purposed, nothing can be changed here.  The physical file is located in the root of the image folder and should never be changed.</p>
</asp:Content>

