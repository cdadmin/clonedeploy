<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#admin').addClass("nav-current");
            $('#admin-pxe').addClass("nav-current-sub");
        });
    </script>
    <h1>Admin->PXE</h1>
    <p>
        The pxe settings control which bootloader is globally applied to computers when pxe booting. If you are booting a mix of legacy bios and EFI computers, it is recommended to use
        ProxyDHCP. When using ProxyDHCP, up to 3 different boot loaders can be utilized simultaneously to match the correct boot architecture. If not using ProxyDHCP, only one can be enabled at a time.
        This setting only applies to the WinPE and Linux Imaging Environments.
    </p>
</asp:Content>