<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#computers').addClass("nav-current");
            $('#computers-group').addClass("nav-current-sub");
        });
    </script>
    <h1>Computers->Group Membership</h1>
    <p>Displays a list of all groups that the computer belongs to. No changes can be made here. It is for informational purposes only.</p>
</asp:Content>