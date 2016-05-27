<%@ Page Title="" Language="C#" MasterPageFile="~/views/global/filesandfolders/filesandfolders.master" AutoEventWireup="true" CodeFile="edit.aspx.cs" Inherits="views_global_filesandfolders_edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
     <li><%= FileFolder.Name %></li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubHelp" Runat="Server">
     <a href="<%= ResolveUrl("~/views/help/index.html") %>"   target="_blank">Help</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ActionsRightSub" Runat="Server">
      <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_OnClick" Text="Update File / Folder "  />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="SubContent2" Runat="Server">
      <script type="text/javascript">
        $(document).ready(function() {
            $('#edit').addClass("nav-current");
        });
    </script>
    
     <div class="size-4 column">
        Display Name:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtName" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>

   
    <div class="size-4 column">
        Path:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtPath" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>
    
    <div class="size-4 column">
        Type:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlType" runat="server" CssClass="ddlist" >
            <asp:ListItem>File</asp:ListItem>
             <asp:ListItem>Folder</asp:ListItem>
        </asp:DropDownList>
    </div>
    <br class="clear"/>
    
  
 
</asp:Content>

