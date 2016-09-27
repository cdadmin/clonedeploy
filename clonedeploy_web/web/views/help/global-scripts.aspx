<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
     <script type="text/javascript">
         $(document).ready(function () {
             $('#global').addClass("nav-current");
             $('#global-scripts').addClass("nav-current-sub");
         });
        </script>
    <h1>Global->Imaging Scripts</h1>
   <p>Imaging scripts are custom scripts that can be run during the imaging process.  After the script is defined it can then
       be assigned in the imaging profile.  When using the Linux or macOS imaging environment the scripts will be executed as bash
       scripts.  If using the WinPE Imaging Environment the script will be executed as a Powershell script.  Any variables within 
       the core scripts can be used here as well as custom attribute variables.</p>

<p>The CloneDeploy Core Client Scripts can also be edited here.  When modifying the Core Client Scripts you are modifying the 
    original files located at the CloneDeploy web directory\private\clientscripts.  You should probably make a copy of these files before 
    you start editing them.  Only administrators have access to this page.</p>
</asp:Content>

