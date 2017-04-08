<%@ Page Title="" Language="C#" MasterPageFile="~/views/groups/groups.master" AutoEventWireup="true" Inherits="views_groups_multicast" Codebehind="multicast.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
      <li><a href="<%= ResolveUrl("~/views/groups/edit.aspx") %>?groupid=<%= Group.Id %>" ><%= Group.Name %></a></li>
    <li>Multicast Options</li>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Help">
    <li role="separator" class="divider"></li>
     <li><a href="<%= ResolveUrl("~/views/help/groups-multicast.aspx")%>" target="_blank">Help</a></li>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="SubPageActionsRight">
       <asp:LinkButton ID="Submit" runat="server" OnClick="Submit_OnClick" Text="Update Group" CssClass="btn btn-default" />
     <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
    <span class="caret"></span>
  </button>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="SubContent" Runat="Server">
     <script type="text/javascript">
        $(document).ready(function() {
            $('#multicast').addClass("nav-current");
        });
    </script>
    
       <div class="size-4 column">
        Image:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlGroupImage" runat="server" CssClass="ddlist" AutoPostBack="true" OnSelectedIndexChanged="ddlGroupImage_OnSelectedIndexChanged"/>
    </div>
    <br class="clear"/>
   
    <div class="size-4 column">
        Image Profile:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlImageProfile" runat="server" CssClass="ddlist"/>
    </div>
    
    <br class="clear"/>
     <div class="size-4 column">
        Cluster Group:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlClusterGroup" runat="server" CssClass="ddlist"/>
    </div>

    <br class="clear"/>
  
</asp:Content>

