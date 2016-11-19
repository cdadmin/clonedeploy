<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
     <script type="text/javascript">
         $(document).ready(function () {
             $('#computers').addClass("nav-current");
             $('#computers-munki').addClass("nav-current-sub");
         });
        </script>
    <h1>Computers->Munki</h1>
   <p>This page lets you set any Munki Templates that have already been defined in Global->Munki Templates.  All Munki Templates are additive.  The resulting manifest file that is created will
       be a combination of any Templates that are assigned here as well as any templates that are assigned to any groups the computer may be a member of.  It is important to note that the manifest files
       are not updated until you Apply the template in Global->Munki Templates.  Every time you change the assigned templates you must re-apply them.  Finally, there is action called View Effective Manifest.
       This action will show you the Manifest this computer will use after you apply the templates, based on any templates you have currently assigned to that computer or groups the computer is in.  The Effective Manifest
       may or may not yet be active.
   </p>
</asp:Content>

