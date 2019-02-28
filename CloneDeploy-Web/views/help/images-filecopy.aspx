<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#images').addClass("nav-current");
            $('#images-filecopy').addClass("nav-current-sub");
        });
    </script>
    <h1>Images->File Copy</h1>
    <p>Allows you to copy additional files or folders to a specific partition after the image is applied. They are only applied to deploy or multicast tasks. Like scripts and sysprep they can be assigned a priority to run. Lower numbers run first. The Destination Partition is just the number of the partition you want to send to. You can find the correct number by viewing the image schema. If you are copying to an LVM partition you would use volumegroupname-logicalvolumename for the destination. The Destination Path is the folder on that partition you want to send the files to. The path is relative to the root of the partition. For example if wanted to push files to c:\myfiles\folder1 you would enter myfiles/folder1 or myfiles\folder1 in the destination path, slashes will be automatically corrected. If you wanted to save files directly to c: you would just enter / or \. Be careful folder names are case sensitive. Finally if you are copying a folder, you can select the Folder Copy Mode. Selecting Folder will place that folder and all of it’s contents in the destination path. Selecting Folder Contents will just copy the contents of that folder without creating the directory itself. Files / Folders are added to CloneDeploy in Global Properties -> Files / Folders. More information can be found in the Global Properties documentation.</p>
</asp:Content>