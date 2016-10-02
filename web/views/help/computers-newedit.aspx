<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
     <script type="text/javascript">
         $(document).ready(function () {
             $('#computers').addClass("nav-current");
             $('#computers-newedit').addClass("nav-current-sub");
         });
        </script>
    <h1>Computers->New</h1>
    <h3>Name</h3>

The name you enter here is what the machine will be renamed to after it images.  Computer names must be unique.  You cannot have multiple computes in CloneDeploy with the same name.
<h3>MAC Address</h3>

The computer’s mac address must be correct for CloneDeploy to work.  This is how the computer identifies itself to the Server when pxe booting.  Valid formats include – 00:00:00:00:00:00 – 00-00-00-00-00-00 – 000000000000
<h3>Image</h3>

The current assigned image of the computer. Both uploading and deploying will use this image.
<h3>Image Profile</h3>

The current assigned image profile of the selected image.  Profiles control the image process for the computer.
<h3>Description (optional)</h3>

The description field is for your own use.
<h3>Sites, Buildings, Rooms</h3>

This is new feature of CloneDeploy and is still under development.  This allows you to organize your computer in a physical manner.  It is also a key component of distribution points.  This allows you to setup multiple SMB shares for your images and point your computer to the appropriate one based on it’s physical location.
<h3>Custom Attributes</h3>

Custom attributes allow you to set custom values that can be used with custom scripting.  An example of this may be a product key.  You can access the custom attribute values within a script with $cust_attr_x replacing x with the number of the attribute.
<h3>Create Another</h3>

This can be helpful when adding a lot of computers. When this box is checked all values will remain the same after you click Add Computer. If you are only incrementing a computer name number or mac address all other values will remain the same.
</asp:Content>

