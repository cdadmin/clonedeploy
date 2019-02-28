<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#admin').addClass("nav-current");
            $('#admin-profiletemplate').addClass("nav-current-sub");
        });
    </script>
    <h1>Admin->Image Profile Templates</h1>
    <p>
      Image profile templates allow you customize the default imaging profile options used when a new image is created.  There are 3 templates that can be customized according to image type.
        A Linux Block image, a Linux File image, and a WinPE image.  
    </p>
    
</asp:Content>