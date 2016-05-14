<%@ Page Title="" Language="C#" MasterPageFile="~/views/global/munki/munki.master" AutoEventWireup="true" CodeFile="general.aspx.cs" Inherits="views_global_munki_general" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
    <li><a href="<%= ResolveUrl("~/views/global/munki/general.aspx") %>?manifestid=<%= ManifestTemplate.Id %>&cat=sub2"><%= ManifestTemplate.Name %></a></li>
    <li>General</li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubHelp" Runat="Server">
       <a href="<%= ResolveUrl("~/views/help/index.html") %>" class="submits help" target="_blank"></a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ActionsRightSub" Runat="Server">
     <asp:LinkButton ID="buttonUpdateGeneral" runat="server" OnClick="buttonUpdateGeneral_OnClick" Text="Update Manifest Template" CssClass="submits actions green" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="SubContent2" Runat="Server">
     <script type="text/javascript">
        $(document).ready(function() {
            $('#general').addClass("nav-current");
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
        <asp:TextBox ID="txtManifestDesc" runat="server" CssClass="descbox" TextMode="MultiLine"></asp:TextBox>
    </div>
    <br class="clear" />
     <div class="size-4 column">
        Override Template As Manifest:
    </div>
    <div class="size-5 column">
        <asp:CheckBox ID="chkBoxManifest" runat="server"></asp:CheckBox>
    </div>
    <br class="clear"/>
   
</asp:Content>

