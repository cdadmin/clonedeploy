<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#global').addClass("nav-current");
            $('#global-bootmenutemplates').addClass("nav-current-sub");
        });
    </script>
    <h1>Global->Boot Menu Templates</h1>
    <p>
        Boot Menu Templates and Boot Menu Entries are lumped together in this section. Boot Menu Templates and Entries are only used with the Linux Imaging Environment. The difference b/w the two is that Boot Menu Templates allow you create a completely customized boot menu
        from scratch. You would need to refer to ipxe, syslinux, or grub documentation in order to create a complete menu. Boot Menu Templates can only be assigned to Computers or Groups as a custom
        boot menu. They cannot be used as the Global Default Boot Menu. Boot Menu Entries are a way to add custom boot menu options to the existing Global Default Boot Menu. The entries are inserted every time
        you generate the Global Default Boot Menu. They do not get inserted when you create the entry, only after you generate the new boot files in Admin->Boot Menu
    </p>

    <h2>New / Edit Boot Entry</h2>
    <h3>Name</h3>
    <p>The name that will appear in the boot menu list</p>
    <h3>Description (Optional)</h3>
    <p>For your own use.</p>
    <h3>Type</h3>
    <p>The PXE Mode that the boot menu entry will be applied to. The entry will be inserted anytime there is matching PXE Mode selected in Admin->PXE. It works for both Proxy and Non Proxy Mode.</p>
    <h3>Order</h3>
    <p>The order you to display the menu options in. Lower numbers are listed first. They are always listed after the default CloneDeploy options. Needs to be a valid integer. Negatives are allowed. </p>
    <h3>Active</h3>
    <p>Only Active menu entries are added to the Global Default Boot Menu.</p>
    <h3>Default</h3>
    <p>Makes this menu entry the default boot option</p>
    <h3>Contents</h3>
    <p>The boot menu entry</p>
</asp:Content>