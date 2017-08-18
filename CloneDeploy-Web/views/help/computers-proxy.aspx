<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#computers').addClass("nav-current");
            $('#computers-proxy').addClass("nav-current-sub");
        });
    </script>
    <h1>Computers->Proxy DHCP Reservation</h1>
    <p>
        A Proxy DHCP Reservation allows you to set specific computer to pxe boot in a specific way without effecting any other computers.
        You must have the CloneDeploy Proxy DHCP Server setup properly to look for reservations. This is useful because once the Proxy DHCP Server
        is configured, you never need to change any DHCP options to control how computer's will pxe boot. It can all be configured within the WebUI.
        Typically when using Proxy DHCP mode, a mode is set for each boot type(bios, efi32, efi64). For example all 3 modes may be configured with ipxe, but
        what if a specific model of computer doesn't work with ipxe? This setting allows you to tell this computer to boot using syslinux or grub, leaving all
        other computer's to your default ipxe setting. This setting also allows you to assign an imaging environment to specific computers. For example,
        if the global default imaging environment is set to the Linux Imaging Environment, you could assign the WinPE Imaging Environment to any computer
        model that may be incompatible with Linux.
    </p>
    <p>* The WinPE Boot File options do not display until the WinPE Imaging Environment has been enabled.</p>
    <p>
        * The TFTP server should most likely always be set to <b>[server-ip]</b>
    </p>
    <p>* The Proxy Reservations have no effect on the macOS imaging environment.</p>
</asp:Content>