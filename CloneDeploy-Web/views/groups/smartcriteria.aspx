<%@ Page Title="" Language="C#" MasterPageFile="~/views/groups/groups.master" AutoEventWireup="true" Inherits="CloneDeploy_Web.views.groups.views_groups_smartcriteria" Codebehind="smartcriteria.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li>
        <a href="<%= ResolveUrl("~/views/groups/edit.aspx") %>?groupid=<%= Group.Id %>"><%= Group.Name %></a>
    </li>
    <li>Smart Criteria</li>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Help">
    <li role="separator" class="divider"></li>
    <li>
        <a href="<%= ResolveUrl("~/views/help/groups-smart.aspx") %>" target="_blank">Help</a>
    </li>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="SubPageActionsRight">
    <asp:LinkButton ID="btnUpdate" runat="server" Text="Update Criteria" OnClick="btnUpdate_OnClick" CssClass="btn btn-default"/>
    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        <span class="caret"></span>
    </button>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="SubContent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('.smart').addClass("nav-current");
        });
    </script>

    <div class="size-4 column">
        Computer Name: &nbsp; <asp:DropDownList runat="server" ID="ddlSmartType" CssClass="ddl" Style="display:inline;width:100px;"> 
            <asp:ListItem>Like</asp:ListItem>
            <asp:ListItem>Not Like</asp:ListItem>
            </asp:DropDownList>
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtContains" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <div class="size-5 column" style="margin-top: 10px;">
        <asp:LinkButton ID="btnTestQuery" runat="server" Text="Test Query" OnClick="btnTestQuery_OnClick" CssClass="generic-btn"/>
    </div>
    <br class="clear"/>

    <p class="total">
        <asp:Label ID="lblTotal" runat="server"></asp:Label>
    </p>
    <asp:GridView ID="gvComputers" runat="server" AllowSorting="True" DataKeyNames="Id" OnSorting="gvComputers_OnSorting" AutoGenerateColumns="False" CssClass="Gridview" AlternatingRowStyle-CssClass="alt">
        <Columns>
            <asp:BoundField DataField="Id" HeaderText="computerID" SortExpression="computerID" Visible="False"/>
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-CssClass="width_200"></asp:BoundField>
            <asp:BoundField DataField="Mac" HeaderText="MAC" SortExpression="Mac" ItemStyle-CssClass="mobi-hide-smallest" HeaderStyle-CssClass="mobi-hide-smallest"/>
        </Columns>
        <EmptyDataTemplate>
            No Computers Found
        </EmptyDataTemplate>
    </asp:GridView>

</asp:Content>