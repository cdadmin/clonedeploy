<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
     <script type="text/javascript">
         $(document).ready(function () {
             $('#groups').addClass("nav-current");
             $('#groups-smart').addClass("nav-current-sub");
         });
        </script>
    <h1>Groups->Smart Criteria</h1>
   <p>Sets the current criteria used for adding computers to a smart group.  Currently there is only one option, where computer name contains.  
       I hope to add more flexibility to this eventually.  This is useful when your computer names follow a repetitive naming scheme such as lab123-pc1.  We would add lab123 as the criteria, 
       then anytime more computers are added with the name including lab123, they will automatically be added to the group.  The Test Query shows the matching computers for the criteria, but does 
       not get applied to the group until you click Update Criteria. This page is only visible for smart groups.</p>
</asp:Content>

