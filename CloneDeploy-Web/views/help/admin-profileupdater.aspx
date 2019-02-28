<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#admin').addClass("nav-current");
            $('#admin-profileupdater').addClass("nav-current-sub");
        });
    </script>
    <h1>Admin->Image Profile Updater</h1>
     <p>
       When a new kernel is released, you may want to update all of your image profiles to use the new kernel, that can quickly be done here instead of needing to visit every image profile.
    </p>
    
</asp:Content>