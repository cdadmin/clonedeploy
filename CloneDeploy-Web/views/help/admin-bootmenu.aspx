<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#admin').addClass("nav-current");
            $('#admin-bootmenu').addClass("nav-current-sub");
        });
    </script>
    <h1>Admin->Boot Menu</h1>
    <p>
        This is where you manage the Global Default Boot Menu. This the menu that client computers will see when pxe booted. It only applies to the Linux Imaging Environment. Also remember this menu
        is loaded only when an active task has not been created for a computer. Additionally you can set boot menu passwords here, or directly edit the full text of the boot menu.
    </p>
</asp:Content>