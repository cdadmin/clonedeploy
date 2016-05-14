<%@ Page Title="" Language="C#" MasterPageFile="~/views/global/munki/munki.master" AutoEventWireup="true" CodeFile="manifestcreate.aspx.cs" Inherits="views_global_munki_manifestcreate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
     <li>New Manifest Template</li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubHelp" Runat="Server">
      <a href="<%= ResolveUrl("~/views/help/index.html") %>" class="submits help" target="_blank"></a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ActionsRightSub" Runat="Server">
     <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_OnClick" Text="Add Manifest Template" CssClass="submits actions green" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="SubContent2" Runat="Server"><script type="text/javascript">
        $(document).ready(function() {
            $('#create').addClass("nav-current");
        });
    </script>
    
     <div class="size-4 column">
        Manifest Template Name:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtManifestName" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>
    
    <div class="size-4 column">
        Manifest Template Description:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtManifestDesc" runat="server" TextMode="MultiLine" CssClass="descbox"></asp:TextBox>
    </div>
    <br class="clear"/>
    
     <div class="size-4 column">
        Override Template As Manifest:
    </div>
    <div class="size-5 column">
        <asp:CheckBox ID="chkBoxManifest" runat="server"></asp:CheckBox>
    </div>
    <br class="clear"/>
</asp:Content>

