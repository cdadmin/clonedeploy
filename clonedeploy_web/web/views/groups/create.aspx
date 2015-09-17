<%@ Page Title="" Language="C#" MasterPageFile="~/views/groups/groups.master" AutoEventWireup="true" Inherits="views.groups.GroupCreate" CodeFile="create.aspx.cs" %>

<%@ MasterType VirtualPath="~/views/groups/groups.master" %>
<%@ Reference virtualPath="~/views/masters/Site.master" %>

<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#creategroup').addClass("nav-current");
        });
    </script>
    <div class="size-4 column">
        Group Name:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtGroupName" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Group Type:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlGroupType" runat="server" CssClass="ddlist">
            <asp:ListItem>standard</asp:ListItem>
            <asp:ListItem>smart</asp:ListItem>
        </asp:DropDownList>
    </div>
    <br class="clear"/>
   
     <div class="size-4 column">
        Image:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlGroupImage" runat="server" CssClass="ddlist" AutoPostBack="true" OnSelectedIndexChanged="ddlGroupImage_OnSelectedIndexChanged"/>
    </div>
    <br class="clear"/>
   
    <div class="size-4 column">
        Image Profile:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlImageProfile" runat="server" CssClass="ddlist"/>
    </div>

    <br class="clear"/>
    <div class="size-4 column">
        Group Description:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtGroupDesc" runat="server" CssClass="descbox" TextMode="MultiLine"></asp:TextBox>
    </div>
    <br class="clear"/>
  
    <div class="size-4 column">
        Sender Arguments:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtGroupSenderArgs" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>
     <div class="size-4 column">
        Receiver Arguments:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtGroupReceiveArgs" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>
   
    <div class="size-4 column">
        &nbsp;
    </div>
    <div class="size-5 column">
        <asp:LinkButton ID="Submit" runat="server" OnClick="Submit_Click" Text="Add Group" CssClass="submits"/>
    </div>
    <br class="clear"/>
   
</asp:Content>