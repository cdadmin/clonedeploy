<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#global').addClass("nav-current");
            $('#global-imageclass').addClass("nav-current-sub");
        });
    </script>
    <h1>Global->Image Classifications</h1>
    <p>
       Image classifications can be defined here and then assigned to images,groups, and computers.  For example, a classification could be called EFI.  Then when an image is created it could be assigned 
        the EFI classification.  Finally, only computers with the EFI classification would be allowed to use that image.
    </p>
</asp:Content>