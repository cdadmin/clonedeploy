<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#admin').addClass("nav-current");
            $('#admin-dp').addClass("nav-current-sub");
        });
    </script>
    <h1>Admin->Distribution Points</h1>
    <p>
        Distribution points are simply SMB shares that client computers will connect to for uploading and deploying images. They are still under development and do not yet work completely as intended if
        multiple distribution points are used. If using multiple distribution points assigned to computers via a Site, Building, or Room, you should note that the image files do not get replicated among the
        distribution points. You must do that with your own method. Also, uploads will always go to the primary distribution point.
    </p>
    <h3>Display Name</h3>
    <p>The name of the distribution point, only used in the WebUI.</p>
    <h3>Server Ip / Name</h3>
    <p>The ip or fqdn of the share your client computers will connect to.</p>
    <h3>Protocol</h3>
    <p>The protocol that it used to communicate with the distribution point. SMB is currently the only option.</p>
    <h3>Share Name</h3>
    <p>The name of the shared folder on the OS. cd_share by default</p>
    <h3>Domain / Workgroup</h3>
    <p>Specifies the domain or workgroup that the users belong to that will connect to the share</p>
    <h3>Read / Write Username</h3>
    <p>The username that is used to connect to the share when uploading an image. This user must already exist on the OS.</p>
    <h3>Read / Write Password</h3>
    <p>The password for the Read / Write user. Special characters can sometimes be an issue here.</p>
    <h3>Read Only Username</h3>
    <p>The username that is used to connect to the share when deploying an image. This user must already exist on the OS.</p>
    <h3>Read Only Password</h3>
    <p>The password for the Read Only user. Special characters can sometimes be an issue here.</p>
    <h3>Primary Distribution Point</h3>
    <p>There must always be 1 primary distribution point, uploads always go to the primary distribution point. Check this box if it's primary.</p>
    <h3>Physical Path</h3>
    <p>The local path to the share.  Only used when Location is set to Local.</p>
      <h3>Queue Size</h3>
    <p>The number of simultaneous imaging sessions that can be used with this Distribution Point.</p>
</asp:Content>