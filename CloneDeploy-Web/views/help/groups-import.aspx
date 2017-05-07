<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#groups').addClass("nav-current");
            $('#groups-import').addClass("nav-current-sub");
        });
    </script>
    <h1>Groups->Import</h1>
    <p>Import allows you to import a list of groups from a CSV file. It does not include group members. The expected header is:</p>
    <p>
        <b>Name,Description,Type,SmartCriteria</b>
    </p>
</asp:Content>