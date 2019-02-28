<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#admin').addClass("nav-current");
            $('#admin-cluster').addClass("nav-current-sub");
        });
    </script>
    <h1>Admin->Cluster</h1>
    <p>
        Clustering is used to combine multiple CloneDeploy servers with various roles to help with load balancing, distribution, and failover.
    </p>

    <h3>Server Roles->Server Operation Mode</h3>
    <p>When not clustering select single, otherwise select a cluster option.  Only a single server can be the primary.</p>


    <h3>Secondary Servers</h3>
    <p>Secondary servers are the additional servers that will be added to the cluster.  They will have their roles already set to Cluster Secondary.  Then create a service account on each secondary
        server and fill in the info on the Add Secondary Server Tab.
    </p>

    <h3>Cluster Groups</h3>
    <p>When clustering is enabled, there must be at least 1 cluster group.  The cluster group defines which roles and distribution points belong to that cluster.  Each cluster group must have at least one server and
        one distribution point.  After the group is created, it can be assigned to computers to control which computers connect to which servers and distribution points.
    </p>
</asp:Content>