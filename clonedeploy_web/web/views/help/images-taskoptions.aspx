<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
     <script type="text/javascript">
         $(document).ready(function () {
             $('#images').addClass("nav-current");
             $('#images-taskoptions').addClass("nav-current-sub");
         });
        </script>
    <h1>Images->Task Options</h1>
   <h3>Web Cancelable</h3>

<p>If a web task is cancelled from the web interface, the task will disappear, but will continue to run on the client itself.  
    Checking this box will stop the task on the client and perform the task completed action.  This would mainly be used for 
    debugging.  Sometimes an image process will appear to have frozen.  If Web Cancellable is enabled it should stop the client 
    and upload any part of the log it currently has.  This must be enabled before a task is started to work.</p>
<h3>Task Completed Action</h3>

<p>Specifies what should happen when a client task finishes or errors out.</p>
</asp:Content>

