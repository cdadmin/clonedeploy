<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#images').addClass("nav-current");
            $('#images-newedit').addClass("nav-current-sub");
        });
    </script>
    <h1>Images->New / Edit</h1>
    <h3>Name</h3>

    <p>The name of the Image</p>

    <h3>Client Imaging Environment</h3>
    <p>The imaging environment the image will be used with - This does not always correspond to the OS you are uploading.</p>

    <h3>Type</h3>
    <p>There are two types of Images, Block and File. This specifies if the image is captured at the block or file level. Block is the most tested and I would recommend sticking with that unless you run into issues. This cannot be changed after the image is created.</p>

    <h3>Description (Optional)</h3>

    <p>A description of the image for your own use.</p>
    
    <h3>Classification</h3>
    <p>Sets a classification for the image that can later be used to restrict computers to this image.</p>

    <h3>Enabled</h3>
    <p>Images can only be deployed if they are enabled</p>

    <h3>Protected</h3>

    <p>A protected image cannot be deleted or uploaded.</p>

    <h3>Visible On Demand</h3>

    <p>Specifies if you want the image to be usable in On Demand mode</p>
</asp:Content>