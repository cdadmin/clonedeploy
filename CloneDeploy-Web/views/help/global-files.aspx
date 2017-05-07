<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#global').addClass("nav-current");
            $('#global-files').addClass("nav-current-sub");
        });
    </script>
    <h1>Global->Files / Folders</h1>
    <p>
        Files and folders allow you to define any extra files you want to copy to the computer during the imaging process. This is just a database definition so CloneDeploy is aware of these files.
        You must copy the files to the resources folder located in your distribution point shared folder. When setting the path field, it is relative to the location of the resources folder.
        Example: Adding a file named myfile.txt to c:\program files\clonedeploy\cd_dp\resources\myfile.txt you would just enter myfile.txt in the path field. Files and folders are then assigned
        to an image profile.
    </p>
</asp:Content>