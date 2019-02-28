<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#global').addClass("nav-current");
            $('#global-scripts').addClass("nav-current-sub");
        });
    </script>
    <h1>Global->Imaging Scripts</h1>
    <p>
        Imaging scripts are custom scripts that can be run during the imaging process. After the script is defined it can then
        be assigned in the imaging profile. When using the Linux imaging environment the scripts will be executed as bash
        scripts. If using the WinPE Imaging Environment the script will be executed as a Powershell script. Any variables within
        the core scripts can be used here as well as custom attribute variables.
    </p>

  
</asp:Content>