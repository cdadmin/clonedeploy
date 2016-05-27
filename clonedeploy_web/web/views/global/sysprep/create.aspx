<%@ Page Title="" Language="C#" MasterPageFile="~/views/global/sysprep/sysprep.master" AutoEventWireup="true" CodeFile="create.aspx.cs" Inherits="views_global_sysprep_create" ValidateRequest="False" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
    <li>New Sysprep Customization</li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SubHelp" Runat="Server">
    <a href="<%= ResolveUrl("~/views/help/index.html") %>"   target="_blank">Help</a>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ActionsRightSub" Runat="Server">
      <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_OnClick" Text="Add Sysprep Tag"  />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server" >
    <script type="text/javascript">
        $(document).ready(function() {
            $('#create').addClass("nav-current");
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
        Opening Tag:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtOpenTag" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>
    
      <div class="size-4 column">
        Closing Tag:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtCloseTag" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>

    <div class="size-4 column">
        Description:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtSysprepDesc" runat="server" TextMode="MultiLine" CssClass="descbox"></asp:TextBox>
    </div>
    <br class="clear"/>
   <div class="size-4 column">
        Contents:
    </div>
    <br class="clear"/>
    

     <asp:TextBox ID="txtContent" runat="server" CssClass="sysprepcontent" TextMode="MultiLine"></asp:TextBox>

 
</asp:Content>



