<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
     <script type="text/javascript">
         $(document).ready(function () {
             $('#computers').addClass("nav-current");
             $('#computers-bootmenu').addClass("nav-current-sub");
         });
        </script>
    <h1>Computers->Boot Menu</h1>
   <h3>Active</h3>
    <p>The active page displays the current boot menu file that will be displayed when this specific computer is PXE booted.  It could be
    the global default menu, an active task menu, or a custom menu.  Note that
    boot menu's are only used with the Linux Imaging Environment.  No changes can be made on this page, it is for informational / debug purposes
    only.</p>
    <h3>Custom</h3>
    <p>The custom page allows you to set a custom boot menu that only applies to this computer.  It will stand in place of the global default
    boot menu.  The menu is not used when an imaging task is created, it only applies when a task is not running for the computer. If using ProxyDHCP Mode, you have the option to edit all 3 menus depending on the computer's architecture.  Optionally, you can
        select to use a template that can be created in Global->Boot Menu Templates</p>
    
</asp:Content>

