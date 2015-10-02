<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/dp/dp.master" AutoEventWireup="true" CodeFile="edit.aspx.cs" Inherits="views_admin_dp_edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
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
            <asp:ListItem>SMB+HTTP</asp:ListItem>
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
        Username:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtUsername" runat="server" CssClass="textbox"/>
    </div>

    <br class="clear"/>
    
     <div class="size-4 column">
        Password:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtPassword" runat="server" CssClass="textbox" TextMode="Password"/>
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
    <div class="size-5 column">
        <asp:TextBox ID="txtPhysicalPath" runat="server" CssClass="textbox"/>
    </div>

    <br class="clear"/>
     </div>

     <div class="size-4 column">
        Uses Backend Storage:
    </div>
    <div class="size-5 column">
        <asp:CheckBox ID="chkBackend" runat="server" OnCheckedChanged="chkBackend_OnCheckedChanged" AutoPostBack="True" />
    </div>

    <br class="clear"/>
      <br />
    
    <div id="BackendServer" runat="server" Visible="False">
      <div class="size-4 column">
        Backend Server Ip / Name:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtBackendServer" runat="server" CssClass="textbox"/>
    </div>

    <br class="clear"/>
    </div>

    <div class="size-4 column">
        &nbsp;
    </div>
    <div class="size-5 column">
        <asp:LinkButton ID="buttonUpdateDp" runat="server" OnClick="buttonUpdateDp_OnClick" Text="Update Distribution Point" CssClass="submits" ClientIDMode="Static"/>
    </div>
</asp:Content>

