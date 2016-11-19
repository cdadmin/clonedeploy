<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
     <script type="text/javascript">
         $(document).ready(function () {
             $('#tasks').addClass("nav-current");
             $('#tasks-startgroup').addClass("nav-current-sub");
         });
        </script>
    <h1>Tasks->Start Group Task</h1>
   <p>Allows a user to start a deploy task for all group members using either a multicast or unicast.  A multicast will use the assigned
       multicast image for the group.  A group unicast is the equivalent of start a computer task for each group member individually.  In this scenario
       the image assigned to each computer is used instead of the group's multicast image.
   </p>
</asp:Content>

