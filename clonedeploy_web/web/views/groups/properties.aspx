<%@ Page Title="" Language="C#" MasterPageFile="~/views/groups/groups.master" AutoEventWireup="true" CodeFile="properties.aspx.cs" Inherits="views_groups_properties" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
       <li><a href="<%= ResolveUrl("~/views/groups/edit.aspx") %>?groupid=<%= Group.Id %>" ><%= Group.Name %></a></li>
    <li>Computer Properties</li>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Help">
     <a href="<%= ResolveUrl("~/views/help/index.html")%>" class="submits actions" target="_blank">Help</a>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="SubPageActionsRight">
      <asp:LinkButton ID="btnSubmit" runat="server" Text="Update Properties" OnClick="btnSubmit_OnClick" CssClass="submits actions"/>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="SubContent" Runat="Server">
     <script type="text/javascript">
        $(document).ready(function() {
            $('#properties').addClass("nav-current");
        });
    </script>
    <div class="size-4 column">
        Set As Default For New Group Members:
    </div>
    <div class="size-5 column">
        <asp:CheckBox runat="server" ID="chkDefault" AutoPostBack="True" OnCheckedChanged="chkDefault_OnCheckedChanged"/>
    </div>
    <br class="clear" />
    <br />
    <div class="size-4 column">
        Image:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlComputerImage" runat="server" CssClass="ddlist" AutoPostBack="true" OnSelectedIndexChanged="ddlComputerImage_OnSelectedIndexChanged"/>
   </div>
    <div class="size-4 column">
        <asp:CheckBox runat="server" Id="chkImage"/>
    </div>
    <br class="clear"/>
   
    <div class="size-4 column">
        Image Profile:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlImageProfile" runat="server" CssClass="ddlist"/>
    </div>
    <div class="size-4 column">
        <asp:CheckBox runat="server" Id="chkProfile"/>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Computer Description:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtComputerDesc" runat="server" CssClass="descbox" TextMode="MultiLine"></asp:TextBox>
    </div>
    <div class="size-4 column">
        <asp:CheckBox runat="server" Id="chkDescription"/>
    </div>
    <br class="clear"/>
    
     <div class="size-4 column">
        Site:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlSite" runat="server" CssClass="ddlist"/>
    </div>
    <div class="size-4 column">
        <asp:CheckBox runat="server" Id="chkSite"/>
    </div>
    <br class="clear"/>
     <div class="size-4 column">
        Building:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlBuilding" runat="server" CssClass="ddlist"/>
    </div>
    <div class="size-4 column">
        <asp:CheckBox runat="server" Id="chkBuilding"/>
    </div>
    <br class="clear"/>
     <div class="size-4 column">
        Room:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlRoom" runat="server" CssClass="ddlist"/>
    </div>
    <div class="size-4 column">
        <asp:CheckBox runat="server" Id="chkRoom"/>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Custom Attribute 1:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtCustom1" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <div class="size-4 column">
        <asp:CheckBox runat="server" Id="chkCustom1"/>
    </div>
    <br class="clear"/>
    
    <div class="size-4 column">
        Custom Attribute 2:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtCustom2" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <div class="size-4 column">
        <asp:CheckBox runat="server" Id="chkCustom2"/>
    </div>
    <br class="clear"/>
    
    <div class="size-4 column">
        Custom Attribute 3:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtCustom3" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <div class="size-4 column">
        <asp:CheckBox runat="server" Id="chkCustom3"/>
    </div>
    <br class="clear"/>
    
    <div class="size-4 column">
         Custom Attribute 4:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtCustom4" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <div class="size-4 column">
        <asp:CheckBox runat="server" Id="chkCustom4"/>
    </div>
    <br class="clear"/>
    
    <div class="size-4 column">
        Custom Attribute 5:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtCustom5" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <div class="size-4 column">
        <asp:CheckBox runat="server" Id="chkCustom5"/>
    </div>
</asp:Content>

