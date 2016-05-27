<%@ Page Title="" Language="C#" MasterPageFile="~/views/images/profiles/profiles.master" AutoEventWireup="true" CodeFile="multicast.aspx.cs" Inherits="views_images_profiles_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
      <li><a href="<%= ResolveUrl("~/views/images/profiles/general.aspx") %>?imageid=<%= Image.Id %>&profileid=<%= ImageProfile.Id %>&cat=profiles"><%= ImageProfile.Name %></a></li>
    <li>Multicast Options</li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubHelp" Runat="Server">
     <a href="<%= ResolveUrl("~/views/help/index.html") %>"   target="_blank">Help</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ActionsRightSub" Runat="Server">
     <asp:LinkButton ID="buttonUpdateMulticast" runat="server" OnClick="buttonUpdateMulticast_OnClick" Text="Update Multicast Options " />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="SubContent2" Runat="Server">
      <script type="text/javascript">
        $(document).ready(function() {
            $('#multicast').addClass("nav-current");
        });
    </script>
    <div class="size-4 column">
        Sender Arguments:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtSender" runat="server" CssClass="textbox">
        </asp:TextBox>
    </div>
    <br class="clear"/>
    
     <div class="size-4 column">
        Receiver Arguments:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtReceiver" runat="server" CssClass="textbox">
        </asp:TextBox>
    </div>
    <br class="clear"/>
</asp:Content>

