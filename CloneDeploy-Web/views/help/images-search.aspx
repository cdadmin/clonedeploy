<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#images').addClass("nav-current");
            $('#images-search').addClass("nav-current-sub");
        });
    </script>
    <h1>Images->Search</h1>
    <p>
        The search screen is the default view when you select Images. It displays all of the current images. The search bar implies a wildcard before and after the string
        you enter. Leave it blank for all images. You can also use this view to delete multiple images as well as view additional options for an individual image and approve multiple images.
    </p>
</asp:Content>