<%@ Page Title="" Language="C#" MasterPageFile="~/views/images/profiles/profiles.master" AutoEventWireup="true" CodeFile="create.aspx.cs" Inherits="views_images_profiles_create" %>


<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
    <li>New</li>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="SubHelp">
    <a href="<%= ResolveUrl("~/views/help/index.html") %>" class="submits help" target="_blank"></a>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ActionsRightSub">
      <asp:LinkButton ID="buttonCreateProfile" runat="server" OnClick="buttonCreateProfile_OnClick" Text="Create Profile" CssClass="submits actions green"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#create').addClass("nav-current");
        });
    </script>
     <div class="size-4 column">
        Profile Name:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtProfileName" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>
     <div class="size-4 column">
        Profile Description:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtProfileDesc" runat="server" CssClass="descbox" TextMode="MultiLine"></asp:TextBox>
    </div>
   
</asp:Content>

