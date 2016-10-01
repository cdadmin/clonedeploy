<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
     <script type="text/javascript">
         $(document).ready(function () {
             $('#images').addClass("nav-current");
             $('#images-pxeoptions').addClass("nav-current-sub");
         });
        </script>
    <h1>Images->PXE Options</h1>
   <p>Specifies the kernel and boot image that are used when pxe booting computers that have this image profile assigned and an active
       task has been created.
   </p>
</asp:Content>

