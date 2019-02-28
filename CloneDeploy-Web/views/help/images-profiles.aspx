<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#images').addClass("nav-current");
            $('#images-profiles').addClass("nav-current-sub");
        });
    </script>
    <h1>Images->Profiles</h1>
    <p>
        Image Profiles are one of the most important concepts in CloneDeploy. They control how an image is uploaded or deployed. Images can have multiple profiles to support multiple
        configurations. A default profile is always generated when an image is created. Profile options will appear different depending on the Imaging Environment assigned
        to the image.
    </p>
    <h3>Profile Name</h3>

    <p>A name to identify the profile.</p>

    <h3>Profile Description</h3>

    <p>A description for the profile, for your own use.</p>

    <h3>Model Match</h3>

    <p>Model match is used to automatically deploy an image to any machines that match the model defined here.  This setting should be used with caution as it is very easy to image a machine even without starting any tasks.  You must also be careful to not overwite a computer that you want to upload an image from.  To use this setting enter the model name in this field.  The model string can be found in the upload and deploy logs.</p>
    <h3>Model Match Criteria</h3>

    <p>Specifies how the Model Match should be evaluated, such as a partial match or an exact match.</p>
</asp:Content>