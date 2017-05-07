<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#admin').addClass("nav-current");
            $('#admin-security').addClass("nav-current-sub");
        });
    </script>
    <h1>Admin->Security</h1>
    <h3>Require Image Approval</h3>
    <p>Determines if you want to require newly uploaded images to be approved before they can be deployed. Administrators can always deploy even with an image being approved.</p>
    <h3>Force SSL</h3>
    <p>Will automatically redirect all WebUI requests to https. You must first setup SSL in your web application and very https is working before you enable this.</p>
    <h3>On Demand Mode</h3>
    <p>Determines if On Demand Imaging is available.</p>
    <h3>Debug Requires Login</h3>
    <p>Determines if the client console requires a login. If set to no a universal token must be generated. Only applies to the Linux Imaging Environment.</p>
    <h3>On Demand Requires Login</h3>
    <p>Determines if On Demand Mode requires a login. If set to no a universal token must be generated. Only applies to the Linux Imaging Environment.</p>
    <h3>Add Computer Requires Login</h3>
    <p>Determines if adding a client computer from the pxe boot menu requires a login. If set to no a universal token must be generated. Only applies to the Linux Imaging Environment.</p>
    <h3>Web Tasks Require Login</h3>
    <p>Determines if after starting an imaging task from the webui do the client computers still need to login in. If set to no a universal token must be generated. Only applies to the Linux Imaging Environment.</p>
    <h3>Clobber Mode Requires Login</h3>
    <p>Determines if clobber mode requires a login. If set to no a universal token must be generated. Only applies to the Linux Imaging Environment.</p>
    <h3>LDAP Authentication</h3>
    <p>
        If you plan on authenticating users via LDAP or LDAP groups, you must first setup LDAP here. This currently only works when CloneDeploy is installed on Windows and has only been tested when
        authenticating against Active Directory. The settings are standard that you would use almost anywhere for LDAP auth.
    </p>
</asp:Content>