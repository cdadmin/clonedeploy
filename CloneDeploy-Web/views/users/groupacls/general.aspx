<%@ Page Title="" Language="C#" MasterPageFile="~/views/users/groupacls/groupacls.master" AutoEventWireup="true" Inherits="CloneDeploy_Web.views.users.groupacls.views_users_groupacls_general" Codebehind="general.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
    <li>General</li>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="SubHelp">
    <li role="separator" class="divider"></li>
    <li>
        <a href="<%= ResolveUrl("~/views/help/users-acl.aspx") %>" target="_blank">Help</a>
    </li>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ActionsRightSub">
    <asp:LinkButton ID="buttonUpdate" runat="server" OnClick="buttonUpdate_OnClick" Text="Update Access Control" CssClass="btn btn-default"/>
    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        <span class="caret"></span>
    </button>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
<script type="text/javascript">
    $(document).ready(function() {
        $('#general').addClass("nav-current");
    });
</script>
<div class="size-4 column">&nbsp;</div>
<div class="size-10 column">Create</div>
<div class="size-10 column">Read</div>
<div class="size-10 column">Update</div>
<div class="size-10 column">Delete</div>
<div class="size-10 column">Search</div>
<div class="clear"></div>

<div class="size-4 column">
    Computers
</div>

<div class="size-10 column">
    <asp:CheckBox runat="server" Id="ComputerCreate"/>
</div>
<div class="size-10 column">
    <asp:CheckBox runat="server" Id="ComputerRead"/>
</div>
<div class="size-10 column">
    <asp:CheckBox runat="server" Id="ComputerUpdate"/>
</div>
<div class="size-10 column">
    <asp:CheckBox runat="server" Id="ComputerDelete"/>
</div>
<div class="size-10 column">
    <asp:CheckBox runat="server" Id="ComputerSearch"/>
</div>
<br class="clear"/>
<br/>
<div class="size-4 column">
    Groups
</div>

<div class="size-10 column">
    <asp:CheckBox runat="server" Id="GroupCreate"/>
</div>
<div class="size-10 column">
    <asp:CheckBox runat="server" Id="GroupRead"/>
</div>
<div class="size-10 column">
    <asp:CheckBox runat="server" Id="GroupUpdate"/>
</div>
<div class="size-10 column">
    <asp:CheckBox runat="server" Id="GroupDelete"/>
</div>
<div class="size-10 column">
    <asp:CheckBox runat="server" Id="GroupSearch"/>
</div>

<br class="clear"/>
<br/>

<div class="size-4 column">
    Smart Groups
</div>

<div class="size-10 column">
    <asp:CheckBox runat="server" Id="SmartCreate"/>
</div>
<div class="size-10 column">
    &nbsp;
</div>
<div class="size-10 column">
    <asp:CheckBox runat="server" Id="SmartUpdate"/>
</div>
<div class="size-10 column">
    &nbsp;
</div>

<br class="clear"/>
<br/>
<div class="size-4 column">
    Images
</div>

<div class="size-10 column">
    <asp:CheckBox runat="server" Id="ImageCreate"/>
</div>
<div class="size-10 column">
    <asp:CheckBox runat="server" Id="ImageRead"/>
</div>
<div class="size-10 column">
    <asp:CheckBox runat="server" Id="ImageUpdate"/>
</div>
<div class="size-10 column">
    <asp:CheckBox runat="server" Id="ImageDelete"/>
</div>
<div class="size-10 column">
    <asp:CheckBox runat="server" Id="ImageSearch"/>
</div>

<br class="clear"/>
<br/>
<div class="size-4 column">
    Image Profiles
</div>

<div class="size-10 column">
    <asp:CheckBox runat="server" Id="ProfileCreate"/>
</div>
<div class="size-10 column">
    <asp:CheckBox runat="server" Id="ProfileRead"/>
</div>
<div class="size-10 column">
    <asp:CheckBox runat="server" Id="ProfileUpdate"/>
</div>
<div class="size-10 column">
    <asp:CheckBox runat="server" Id="ProfileDelete"/>
</div>
<div class="size-10 column">
    <asp:CheckBox runat="server" Id="ProfileSearch"/>
</div>

<br class="clear"/>
<br/>
<div class="size-4 column">
    Global Properties
</div>

<div class="size-10 column">
    <asp:CheckBox runat="server" Id="GlobalCreate"/>
</div>
<div class="size-10 column">
    <asp:CheckBox runat="server" Id="GlobalRead"/>
</div>
<div class="size-10 column">
    <asp:CheckBox runat="server" Id="GlobalUpdate"/>
</div>
<div class="size-10 column">
    <asp:CheckBox runat="server" Id="GlobalDelete"/>
</div>

<br class="clear"/>
<br/>
<div class="size-4 column">
    Admin Settings
</div>

<div class="size-10 column">
    &nbsp;
</div>
<div class="size-10 column">
    <asp:CheckBox runat="server" Id="AdminRead"/>
</div>
<div class="size-10 column">
    <asp:CheckBox runat="server" Id="AdminUpdate"/>
</div>
<div class="size-10 column">
    &nbsp;
</div>
<br class="clear"/>
<br/>
<hr/>
<br/>
<div class="size-4 column">
    &nbsp;
</div>
<div class="size-10 column">
    Upload
</div>
<div class="size-10 column">
    Deploy
</div>
<div class="size-10 column">
    Multicast
</div>

<br class="clear"/>

<div class="size-4 column">
    Imaging Tasks
</div>

<div class="size-10 column">
    <asp:CheckBox runat="server" Id="ImageTaskUpload"/>
</div>
<div class="size-10 column">
    <asp:CheckBox runat="server" Id="ImageTaskDeploy"/>
</div>
<div class="size-10 column">
    <asp:CheckBox runat="server" Id="ImageTaskMulticast"/>
</div>

<br class="clear"/>

<br/>
<hr/>
<br/>

<br/>
<div class="size-4 column">
    &nbsp;
</div>
<div class="size-10 column">
    Yes / No
</div>
<br class="clear"/>
<div class="size-4 column">
    Approve Images
</div>

<div class="size-10 column">
    <asp:CheckBox runat="server" Id="ApproveImage"/>
</div>

<br class="clear"/>
<br/>
<div class="size-4 column">
    Allow On Demand
</div>

<div class="size-10 column">
    <asp:CheckBox runat="server" Id="AllowOnd"/>
</div>

<br class="clear"/>
<br/>
<div class="size-4 column">
    Allow Debug
</div>

<div class="size-10 column">
    <asp:CheckBox runat="server" Id="AllowDebug"/>
</div>


</asp:Content>