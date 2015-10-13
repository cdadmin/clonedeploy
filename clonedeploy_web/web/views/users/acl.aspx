<%@ Page Title="" Language="C#" MasterPageFile="~/views/users/User.master" AutoEventWireup="true" CodeFile="acl.aspx.cs" Inherits="views_users_acl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubContent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#acl').addClass("nav-current");
        });
    </script>
    <div class="size-4 column">&nbsp;</div>
    <div class="size-10 column">Create</div>
    <div class="size-10 column">Read</div>
    <div class="size-10 column">Update</div>
    <div class="size-10 column">Delete</div>
    <div class="clear"></div>
    
    <div class="size-4 column">
        Computers
    </div>
    
    <div class="size-10 column">
        <asp:CheckBox runat="server" Id="chkComputerCreate"/>
    </div>
     <div class="size-10 column">
        <asp:CheckBox runat="server" Id="chkComputerRead"/>
    </div>
     <div class="size-10 column">
        <asp:CheckBox runat="server" Id="chkComputerUpdate"/>
    </div>
     <div class="size-10 column">
        <asp:CheckBox runat="server" Id="chkComputerDelete"/>
    </div>
    <br class="clear"/>
    <br />
     <div class="size-4 column">
        Groups
    </div>
    
    <div class="size-10 column">
        <asp:CheckBox runat="server" Id="chkGroupCreate"/>
    </div>
     <div class="size-10 column">
        <asp:CheckBox runat="server" Id="chkGroupRead"/>
    </div>
     <div class="size-10 column">
        <asp:CheckBox runat="server" Id="chkGroupUpdate"/>
    </div>
     <div class="size-10 column">
        <asp:CheckBox runat="server" Id="chkGroupDelete"/>
    </div>
    
    <br class="clear"/>
    <br />
     <div class="size-4 column">
        Images
    </div>
    
    <div class="size-10 column">
        <asp:CheckBox runat="server" Id="chkImageCreate"/>
    </div>
     <div class="size-10 column">
        <asp:CheckBox runat="server" Id="chkImageRead"/>
    </div>
     <div class="size-10 column">
        <asp:CheckBox runat="server" Id="chkImageUpdate"/>
    </div>
     <div class="size-10 column">
        <asp:CheckBox runat="server" Id="chkImageDelete"/>
    </div>

</asp:Content>

