<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#images').addClass("nav-current");
            $('#images-history').addClass("nav-current-sub");
        });
    </script>
    <h1>Images->History</h1>
    <p>
      Displays a list of events that happened involving this image.
    </p>
</asp:Content>