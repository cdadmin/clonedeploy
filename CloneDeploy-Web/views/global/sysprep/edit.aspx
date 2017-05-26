<%@ Page Title="" Language="C#" MasterPageFile="~/views/global/sysprep/sysprep.master" AutoEventWireup="true" Inherits="CloneDeploy_Web.views.global.sysprep.views_global_sysprep_edit" ValidateRequest="False" Codebehind="edit.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
    <li><%= SysprepTag.Name %></li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SubHelp" Runat="Server">
    <li role="separator" class="divider"></li>
    <li>
        <a href="<%= ResolveUrl("~/views/help/global-sysprep.aspx") %>" target="_blank">Help</a>
    </li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ActionsRightSub" Runat="Server">
    <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_OnClick" Text="Update Sysprep Tag" CssClass="btn btn-default"/>
    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        <span class="caret"></span>
    </button>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">

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