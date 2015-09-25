<%@ Page Title="" Language="C#" MasterPageFile="~/views/groups/groups.master" AutoEventWireup="true" Inherits="views.groups.GroupEdit" CodeFile="edit.aspx.cs" %>



<asp:Content ID="Breadcrumb" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
   <li><a href="<%= ResolveUrl("~/views/groups/edit.aspx") %>?groupid=<%= Group.Id %>" ><%= Group.Name %></a></li>
    </asp:Content>
<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">

    <script type="text/javascript">
        $(document).ready(function() {
            $('#general').addClass("nav-current");
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
    <br class="clear"/>
    <div class="size-4 column">
        &nbsp;
    </div>
    <div class="size-5 column">
        <asp:LinkButton ID="btnSubmit" runat="server" Text="Update Group" OnClick="btnSubmit_Click" CssClass="submits"/>
    </div>
    <br class="clear"/>
  

</asp:Content>