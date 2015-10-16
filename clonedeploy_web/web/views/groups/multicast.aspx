<%@ Page Title="" Language="C#" MasterPageFile="~/views/groups/groups.master" AutoEventWireup="true" CodeFile="multicast.aspx.cs" Inherits="views_groups_multicast" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
      <li><a href="<%= ResolveUrl("~/views/groups/edit.aspx") %>?groupid=<%= Group.Id %>" ><%= Group.Name %></a></li>
    <li>Multicast Settings</li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubContent" Runat="Server">
     <script type="text/javascript">
        $(document).ready(function() {
            $('#multicast').addClass("nav-current");
        });
    </script>
    
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
        <asp:LinkButton ID="Submit" runat="server" OnClick="Submit_OnClick" Text="Update Group" CssClass="submits"/>
    </div>
    <br class="clear"/>
</asp:Content>

