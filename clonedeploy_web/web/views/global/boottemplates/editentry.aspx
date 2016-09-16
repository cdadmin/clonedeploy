<%@ Page Title="" Language="C#" MasterPageFile="~/views/global/boottemplates/boottemplates.master" AutoEventWireup="true" CodeFile="editentry.aspx.cs" Inherits="views_global_boottemplates_editentry" %>
<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
    <li><%= BootEntry.Name %></li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SubHelp" Runat="Server">
    <a href="<%= ResolveUrl("~/views/help/index.html") %>"   target="_blank">Help</a>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ActionsRightSub" Runat="Server">
  <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_OnClick" Text="Update Entry" CssClass="btn btn-default"  />
     <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
    <span class="caret"></span>
  </button>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
     <script type="text/javascript">
        $(document).ready(function() {
            $('#editentry').addClass("nav-current");
        });
    </script>
    
     <div class="size-4 column">
        Name:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtName" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>

   
    <div class="size-4 column">
        Description:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="descbox"></asp:TextBox>
    </div>
    <br class="clear"/>
    
    <div class="size-4 column">
        Type:
    </div>
    <div class="size-5 column">
        <asp:DropDownList runat="server" id="ddlType" CssClass="ddlist">
            <asp:ListItem>syslinux/pxelinux</asp:ListItem>
            <asp:ListItem>ipxe</asp:ListItem>
            <asp:ListItem>grub</asp:ListItem>
            </asp:DropDownList>
    </div>
    <br class="clear"/>
    
    <div class="size-4 column">
        Order:
    </div>
     <div class="size-5 column">
        <asp:TextBox ID="txtOrder" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>
    
    <div class="size-4 column">
        Active:
    </div>
    <div class="size-5 column">
        <asp:CheckBox runat="server" id="chkActive" CssClass=""/>
    </div>
    <br class="clear"/>
    
     <div class="size-4 column">
        Default:
    </div>
    <div class="size-5 column">
        <asp:CheckBox runat="server" id="chkDefault" CssClass=""/>
    </div>
    <br class="clear"/>

   <div class="size-4 column">
        Contents:
    </div>
    <br class="clear"/>
    

     <asp:TextBox ID="txtContents" runat="server" CssClass="sysprepcontent" TextMode="MultiLine"></asp:TextBox>

   
</asp:Content>