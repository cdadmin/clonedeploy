<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#computers').addClass("nav-current");
            $('#computers-import').addClass("nav-current-sub");
        });
    </script>
    <h1>Computers->Import</h1>
    <p>The import page allows you to import a list of computers from a csv. The expected header is:</p>
    <p>
        <b>Name,Mac,Description,CustomAttribute1,CustomAttribute2,CustomAttribute3,CustomAttribute4,CustomAttribute5</b>
    </p>
</asp:Content>