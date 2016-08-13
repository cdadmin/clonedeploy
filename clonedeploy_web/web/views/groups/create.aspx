<%@ Page Title="" Language="C#" MasterPageFile="~/views/groups/groups.master" AutoEventWireup="true" Inherits="views.groups.GroupCreate" CodeFile="create.aspx.cs" %>

<asp:Content ID="Breadcrumb" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li>New</li>
    </asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Help">
     <li><a href="<%= ResolveUrl("~/views/help/index.html")%>" target="_blank">Help</a></li>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="SubPageActionsRight">
     <asp:LinkButton ID="Submit" runat="server" OnClick="Submit_Click" Text="Add Group" CssClass="btn btn-default"/>
     <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
    <span class="caret"></span>
  </button>
</asp:Content>

<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#create').addClass("nav-current");
        });
    </script>
    <div class="size-4 column">
        Group Name:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtGroupName" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Group Type:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlGroupType" runat="server" CssClass="ddlist" >
            <asp:ListItem>standard</asp:ListItem>
            <asp:ListItem>smart</asp:ListItem>
        </asp:DropDownList>
    </div>
    <br class="clear"/>
   
    
    <div class="size-4 column">
        Group Description:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtGroupDesc" runat="server" CssClass="descbox" TextMode="MultiLine"></asp:TextBox>
    </div>
</asp:Content>