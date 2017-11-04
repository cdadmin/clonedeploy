<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#computers').addClass("nav-current");
            $('#computers-history').addClass("nav-current-sub");
        });
    </script>
    <h1>Computers->History</h1>
    <p>
      Displays a list of events that happened involving this computer.
    </p>
</asp:Content>