<%@ Page Title="" Language="C#" MasterPageFile="~/views/users/acls/acls.master" AutoEventWireup="true" CodeFile="groupmanagement.aspx.cs" Inherits="views_users_acls_groupmanagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
     <script type="text/javascript">
        $(document).ready(function() {
            $('#group').addClass("nav-current");
        });
    </script>
     <div class="size-4 column">
        Enable Group Based Computer Management
    </div>
    
    <div class="size-10 column">
        <asp:CheckBox runat="server" Id="CheckBox1"/>
    </div>
</asp:Content>

