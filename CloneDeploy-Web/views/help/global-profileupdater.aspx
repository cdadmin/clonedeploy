<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#global').addClass("nav-current");
            $('#global-profileupdater').addClass("nav-current-sub");
        });
    </script>
    <h1>Global->Image Profile Updater</h1>
    <p>
        The Image Profile Updater allows you quickly change the default kernel for your image profiles. This is useful if a new kernel is released and you want to update all of the image profiles with
        the new kernel. Remember that the image profile kernel is only used if a web task is created for that computer and you are PXE booting. If you are using On Demand Imaging with PXE boot you are using
        the kernel that was assigned to the Global Default Boot Menu.
    </p>
</asp:Content>