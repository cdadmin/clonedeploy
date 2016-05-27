<%@ Page Title="" Language="C#" MasterPageFile="~/views/images/profiles/profiles.master" AutoEventWireup="true" CodeFile="task.aspx.cs" Inherits="views_images_profiles_task" %>


<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
    <li><a href="<%= ResolveUrl("~/views/images/profiles/general.aspx") %>?imageid=<%= Image.Id %>&profileid=<%= ImageProfile.Id %>&cat=profiles"><%= ImageProfile.Name %></a></li>
    <li>Task Options</li>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="SubHelp">
    <a href="<%= ResolveUrl("~/views/help/index.html") %>"   target="_blank">Help</a>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ActionsRightSub">
    <asp:LinkButton ID="btnUpdateTask" runat="server" OnClick="btnUpdateTask_OnClick" Text="Update Task Options" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
     <script type="text/javascript">
        $(document).ready(function() {
            $('#task').addClass("nav-current");
        });
    </script>
     <div class="size-9 column">
        Don't Download Core Scripts
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkGlobalNoCore" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>
    
    <div class="size-9 column">
        Don't Set Hardware Clock
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkGlobalNoClock" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    
      <br class="clear"/>
    
    <div class="size-9 column">
        Web Cancellable
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkWebCancel" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>

    <br class="clear"/>
    
    <div class="size-9 column">
        Task Completed Action
    </div>
    <div class="size-8 column">
        <asp:DropDownList ID="ddlTaskComplete" runat="server" CssClass="ddlist">
            <asp:ListItem>Reboot</asp:ListItem>
            <asp:ListItem>Power Off</asp:ListItem>
            <asp:ListItem>Exit To Shell</asp:ListItem>
        </asp:DropDownList>
    </div>
   
</asp:Content>

