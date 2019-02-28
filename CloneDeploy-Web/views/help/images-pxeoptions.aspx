<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#images').addClass("nav-current");
            $('#images-pxeoptions').addClass("nav-current-sub");
        });
    </script>
    <h1>Images->PXE Options</h1>
    <p>
        Specifies the kernel and boot image that are used when pxe booting computers that have this image profile assigned and an active
        task has been created.  This is only used when an active web task for the computer with this profile is created. It does not apply to on demand or any other mode.  Kernel arguments can also be passed in from here.  This only applies to the Linux imaging environment and only if pxe booted.
    </p>
</asp:Content>