<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#global').addClass("nav-current");
            $('#global-munki').addClass("nav-current-sub");
        });
    </script>
    <h1>Global->Munki</h1>
    <p>
        Munki Templates are a collection of Catalogs, Managed Installs, Managed Uninstalls, Optional Installs, Managed Updates, and Included Manifests, that can be assigned to computers in order
        create a Manifest file for use with Munki. Before you can create Munki Templates you must first setup the Munki location in Admin->Munki. Munki Templates allow CloneDeploy to manage your Manifests
        for all of your Apple computers. Only Manifests are generated, you will continue your current workflow for creating Catalogs, Packages, etc. Once the template is created and assigned to a computer it
        can be automatically installed during the imaging process. Conditions, priority, and specific versions are all supported. Each Munki section, Catalogs, Managed Installs, etc, has two pages, Assigned and Available.
        The available page displays what was found on the Munki base directory, and assigned is currently what is part of the template. If nothing is displayed in the available pages you may need to double check
        your settings in Admin-Munki. In the search page there is an option for preview and one for apply. The preview button will show the resulting manifest for a single template, but remember a computer may have
        more than one template assigned, in which case you should use the preview munki template option in Computers. Finally, the apply button will apply that template to any computers that have been assigned that template.
        Any time changes are made to the template, or you set new computers to use the template, you must apply the template again.
    </p>
</asp:Content>