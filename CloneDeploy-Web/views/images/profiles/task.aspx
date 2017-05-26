<%@ Page Title="" Language="C#" MasterPageFile="~/views/images/profiles/profiles.master" AutoEventWireup="true" Inherits="CloneDeploy_Web.views.images.profiles.views_images_profiles_task" Codebehind="task.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
    <li>
        <a href="<%= ResolveUrl("~/views/images/profiles/general.aspx") %>?imageid=<%= Image.Id %>&profileid=<%= ImageProfile.Id %>&cat=profiles"><%= ImageProfile.Name %></a>
    </li>
    <li>Task Options</li>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="SubHelp">
    <li role="separator" class="divider"></li>
    <li>
        <a href="<%= ResolveUrl("~/views/help/images-taskoptions.aspx") %>" target="_blank">Help</a>
    </li>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ActionsRightSub">
    <asp:LinkButton ID="btnUpdateTask" runat="server" OnClick="btnUpdateTask_OnClick" Text="Update Task Options" CssClass="btn btn-default"/>
    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        <span class="caret"></span>
    </button>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#task').addClass("nav-current");
        });
    </script>

    <div class="size-9 column">
        Web Cancelable
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkWebCancel" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>

    <br class="clear"/>

    <div class="size-9 column">
        Task Completed Action
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlTaskComplete" runat="server" CssClass="ddlist">
            <asp:ListItem>Reboot</asp:ListItem>
            <asp:ListItem>Power Off</asp:ListItem>
            <asp:ListItem>Exit To Shell</asp:ListItem>
        </asp:DropDownList>
    </div>

</asp:Content>