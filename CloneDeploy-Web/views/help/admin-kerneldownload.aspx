<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#admin').addClass("nav-current");
            $('#admin-kerneldownloads').addClass("nav-current-sub");
        });
    </script>
    <h1>Admin->Kernel Downloads</h1>
   <p>
       The kernel downloads provide a simple interface to install new kernels when they are released.
    </p>
</asp:Content>