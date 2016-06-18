<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/dp/dp.master" AutoEventWireup="true" CodeFile="create.aspx.cs" Inherits="views_admin_dp_create" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
    <li>New</li>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="SubHelp">
     <a href="<%= ResolveUrl("~/views/help/index.html")%>"  target="_blank">Help</a>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ActionsRightSub">
     <asp:LinkButton ID="buttonAddDp" runat="server" OnClick="buttonAddDp_OnClick" Text="Add Distribution Point"  />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
     <script type="text/javascript">
        $(document).ready(function() {
            $('#create').addClass("nav-current");
        });
    </script>
     <div class="size-4 column">
        Display Name:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtDisplayName" runat="server" CssClass="textbox" ClientIDMode="Static"></asp:TextBox>
      
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Server Ip / Name:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtServer" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Protocol:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlProtocol" runat="server" CssClass="ddlist">
        <asp:ListItem>SMB</asp:ListItem>
            </asp:DropDownList>
    </div>
    <br class="clear"/>
   
    <div class="size-4 column">
        Share Name:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtShareName" runat="server" CssClass="textbox"/>
    </div>
    <br class="clear" />
     <div class="size-4 column">
        Domain / Workgroup:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtDomain" runat="server" CssClass="textbox"/>
    </div>

    <br class="clear"/>
    
     <div class="size-4 column">
        Read/Write Username:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtRwUsername" runat="server" CssClass="textbox"/>
    </div>

    <br class="clear"/>
    
     <div class="size-4 column">
        Read/Write Password:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtRwPassword" runat="server" CssClass="textbox" TextMode="Password"/>
    </div>

    <br class="clear"/>
    
     <div class="size-4 column">
        Read Only Username:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtRoUsername" runat="server" CssClass="textbox"/>
    </div>

    <br class="clear"/>
    
     <div class="size-4 column">
        Read Only Password:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtRoPassword" runat="server" CssClass="textbox" TextMode="Password"/>
    </div>


    <br class="clear"/>
     <div class="size-4 column">
        Primary Distribution Point:
    </div>
    <div class="size-5 column">
        <asp:CheckBox ID="chkPrimary" runat="server" OnCheckedChanged="chkPrimary_OnCheckedChanged" AutoPostBack="True" />
    </div>

    <br class="clear"/>
    <br />
    
    <div id="PhysicalPath" runat="server" Visible="False">
     <div class="size-4 column">
        Physical Path:
    </div>
    <div class="size-1 column">
        <asp:TextBox ID="txtPhysicalPath" runat="server" CssClass="textbox"/>
    </div>

    <br class="clear"/>
     </div>

    
    
    <div id="BackendServer" runat="server" Visible="False">
      <div class="size-4 column">
        Backend Server Ip / Name:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtBackendServer" runat="server" CssClass="textbox"/>
    </div>

  
       
    </div>
</asp:Content>

