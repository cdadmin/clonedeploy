<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#images').addClass("nav-current");
            $('#images-sysprep').addClass("nav-current-sub");
        });
    </script>
    <h1>Images->Sysprep</h1>
    <p>This view allows you to set any sysprep file modifications. They are only applied to deploy or multicast tasks. You can also assign a priority to run the sysprep modifications in a specific order. Lower numbers execute first. Sysprep modifications are added to CloneDeploy in the Global Properties -> Sysprep view. Additional info can be found in the Global Properties documentation.</p>
</asp:Content>