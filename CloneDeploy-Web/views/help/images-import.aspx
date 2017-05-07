<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#images').addClass("nav-current");
            $('#images-import').addClass("nav-current-sub");
        });
    </script>
    <h1>Images->Import</h1>
    <p>Import allows you to import a list of images from a CSV file. The expected header is:</p>
    <p>
        <b>Name,Description,Type,Environment</b>
    </p>
</asp:Content>