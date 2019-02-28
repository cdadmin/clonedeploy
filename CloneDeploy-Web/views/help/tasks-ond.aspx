<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#tasks').addClass("nav-current");
            $('#tasks-activeunicast').addClass("nav-current-sub");
        });
    </script>
    <h1>Tasks->Unregistered On Demand</h1>
    <p>Displays all active tasks with an unregistered computer.  If an on demand task is started with a registered computer, it is not displayed here.  That is 
        displayed in active unicasts page. </p>
</asp:Content>