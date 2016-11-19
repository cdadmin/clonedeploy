<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
     <script type="text/javascript">
         $(document).ready(function () {
             $('#images').addClass("nav-current");
             $('#images-profiles').addClass("nav-current-sub");
         });
        </script>
    <h1>Images->Profiles</h1>
   <p>Image Profiles are one of the most important concepts in CloneDeploy.  They control how an image is uploaded or deployed.  Images can have multiple profiles to support multiple
       configurations.  A default profile is always generated when an image is created.  Profile options will appear different depending on the Imaging Environment assigned
       to the image.
   </p>
</asp:Content>

