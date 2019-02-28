<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#computers').addClass("nav-current");
            $('#computers-newedit').addClass("nav-current-sub");
        });
    </script>
    <h1>Computers->New</h1>
    <h3>Name</h3>

    The name you enter here is what the machine will be renamed to after it images. Computer names must be unique. You cannot have multiple computes in CloneDeploy with the same name.
    <h3>MAC Address</h3>

    The computer’s mac address must be correct for CloneDeploy to work. This is how the computer identifies itself to the Server when pxe booting. Valid formats include – 00:00:00:00:00:00 – 00-00-00-00-00-00 – 000000000000
    
    <h3>Client Identifier</h3>
    A unique identifier used with the imaging environments.  This field is only populated when the computer is added from and imaging environment.  This is the recommended way to add computers in order to avoid issues when computers have the same mac address.  This should not be modified in most circumstances. 

    <h3>Image</h3>

    The current assigned image of the computer. Both uploading and deploying will use this image.
    <h3>Image Profile</h3>

    The current assigned image profile of the selected image. Profiles control the image process for the computer.
    <h3>Description (optional)</h3>

    The description field is for your own use.
    
    <h3>Cluster Group</h3>
    If CloneDeploy has clustering enabled, this field sets the group that this computer should use for pxe booting, imaging, and multicasting.  If not set, the default cluster group is used.  Cluster group precedence goes in order of the smallest scope.  Computer,Room,Building, then site.

    <h3>Sites, Buildings, Rooms</h3>

    This allows you to organize your computers in a physical manner and assign specific cluster groups to them,  allowing for distribution of images, tftp, etc according to physical location.
    
    <h3>Alternate Server Ip</h3>
    This defines which Ip a client computer should use to communicate with the CloneDeploy API for http traffic.  This does not effect pxe booting, smb, or multicasting.  A use case for this is when your network is completely separated by different vlans and you must use a separate nic for each vlan on your CloneDeploy server.  By default the server ip is the admin setting server ip which would not work for different vlans if they cannot route to each other.
    
    <h3>Custom Attributes</h3>

    Custom attributes allow you to set custom values that can be used with custom scripting. An example of this may be a product key. You can access the custom attribute values within a script with $cust_attr_x replacing x with the number of the attribute.
    <h3>Create Another</h3>

    This can be helpful when adding a lot of computers. When this box is checked all values will remain the same after you click Add Computer. If you are only incrementing a computer name number or mac address all other values will remain the same.
</asp:Content>