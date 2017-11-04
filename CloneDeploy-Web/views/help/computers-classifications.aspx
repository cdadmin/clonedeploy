<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#computers').addClass("nav-current");
            $('#computers-classifications').addClass("nav-current-sub");
        });
    </script>
    <h1>Computers->Image Classifications</h1>
    <p>
      Image classifications define which images a computer is compatible with.  If using classifications, you will only be able to assign images that match the classifications to the computer.
    </p>
</asp:Content>