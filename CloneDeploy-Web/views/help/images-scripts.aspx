<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#images').addClass("nav-current");
            $('#images-scripts').addClass("nav-current-sub");
        });
    </script>
    <h1>Images->Scripts</h1>
    <p>This view allows you to set any custom scripts you want to run during the imaging process. They are only applied to deploy or multicast tasks. Any script that has a check in either the Pre or Post box will run. Pre scripts will run before the image is applied and post scripts will run after the image has applied. You can also assign a priority to run the scripts in a specific order. Lower numbers execute first. Scripts are added to CloneDeploy in the Global Properties -> Scripts view. Additional info can be found in the Global Properties documentation.</p>
</asp:Content>