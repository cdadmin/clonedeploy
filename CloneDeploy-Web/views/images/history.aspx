<%@ Page Title="" Language="C#" MasterPageFile="~/views/images/images.master" AutoEventWireup="true" CodeBehind="history.aspx.cs" Inherits="CloneDeploy_Web.views.images.history" %>
<asp:Content ID="Breadcrumb" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li>History</li>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Help">
    <li role="separator" class="divider"></li>
    <li>
        <a href="<%= ResolveUrl("~/views/help/images-history.aspx") %>" class="" target="_blank">Help</a>
    </li>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="SubPageActionsRight">
    <button type="button" class="btn btn-default dropdown-toggle width_140" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        <span class="caret"></span>
    </button>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#history').addClass("nav-current");

            $("[id*=gvHistory] td").hover(function() {
                $("td", $(this).closest("tr")).addClass("hover_row");
            }, function() {
                $("td", $(this).closest("tr")).removeClass("hover_row");
            });
        });

    </script>

    <div class="size-10 column">
        <asp:DropDownList ID="ddlType" runat="server" CssClass="ddlist" AutoPostBack="True" OnSelectedIndexChanged="ddlType_OnSelectedIndexChanged">
            <asp:ListItem>All</asp:ListItem>
            <asp:ListItem>Create</asp:ListItem>
            <asp:ListItem>Update</asp:ListItem>
            <asp:ListItem>Deploy</asp:ListItem>
            <asp:ListItem>Upload</asp:ListItem>
        </asp:DropDownList>
    </div>
    <div class="size-11 column">

        <asp:DropDownList runat="server" ID="ddlLimit" AutoPostBack="True" OnSelectedIndexChanged="ddl_OnSelectedIndexChanged" CssClass="ddlist">
            <asp:ListItem>25</asp:ListItem>
            <asp:ListItem>100</asp:ListItem>
            <asp:ListItem Selected="True">250</asp:ListItem>
            <asp:ListItem >500</asp:ListItem>
            <asp:ListItem>1000</asp:ListItem>
            <asp:ListItem>5000</asp:ListItem>
            <asp:ListItem>All</asp:ListItem>
        </asp:DropDownList>
    </div>
    <br class="clear"/>
    <asp:GridView ID="gvHistory" runat="server" DataKeyNames="Id" AutoGenerateColumns="False" CssClass="Gridview" AlternatingRowStyle-CssClass="alt">
        <Columns>
            <asp:BoundField DataField="Id" HeaderText="AuditId" Visible="False"/>
            <asp:BoundField DataField="UserName" HeaderText="User" ItemStyle-CssClass="width_200"></asp:BoundField>
            <asp:BoundField DataField="AuditType" HeaderText="Type" SortExpression="Mac" ItemStyle-CssClass="width_200"/>
            <asp:BoundField DataField="DateTime" HeaderText="Date" ItemStyle-CssClass="width_200"></asp:BoundField>
            <asp:BoundField DataField="Ip" HeaderText="Ip" ItemStyle-CssClass="width_200"></asp:BoundField>
            <asp:BoundField DataField="ObjectJson" HeaderText="Json" ItemStyle-CssClass="width_200 mobi-hide-smallest" HeaderStyle-CssClass="mobi-hide-smallest"></asp:BoundField>
        </Columns>
        <EmptyDataTemplate>
            No Image History Found
        </EmptyDataTemplate>
    </asp:GridView>


</asp:Content>