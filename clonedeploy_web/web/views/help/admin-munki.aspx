<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
     <script type="text/javascript">
         $(document).ready(function () {
             $('#admin').addClass("nav-current");
             $('#admin-munki').addClass("nav-current-sub");
         });
        </script>
    <h1>Admin->Munki</h1>
   <p>Before Munki can be used with CloneDeploy, you must first tell CloneDeploy where the Munki repo can be found.  It can either be a local directory or an smb share.  The base path should either be
       the full path to the Munki repo directory locally, or a unc path.
   </p>
</asp:Content>

