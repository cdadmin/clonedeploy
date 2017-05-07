<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/logs/logs.master" AutoEventWireup="true" CodeBehind="ond.aspx.cs" Inherits="CloneDeploy_Web.views.admin.logs.ond" %>
<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
    <li>On Demand Logs</li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SubHelp" Runat="Server">
    <li role="separator" class="divider"></li>
    <li>
        <a href="<%= ResolveUrl("~/views/help/admin-logs.aspx") %>" target="_blank">Help</a>
    </li>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ActionsRightSub">
    <button type="button" class="btn btn-default dropdown-toggle width_140" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        <span class="caret"></span>
    </button>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#ond').addClass("nav-current");
            $("[id*=gvLogs] td").hover(function() {
                $("td", $(this).closest("tr")).addClass("hover_row");
            }, function() {
                $("td", $(this).closest("tr")).removeClass("hover_row");
            });
        });
    </script>

    <div class="size-4 column" style="float: right; margin: 0;">
        <asp:DropDownList ID="ddlLimit" runat="server" CssClass="ddlist" Style="float: right; width: 75px;" AutoPostBack="true" OnSelectedIndexChanged="ddlLimit_SelectedIndexChanged">
            <asp:ListItem>10</asp:ListItem>
            <asp:ListItem>25</asp:ListItem>
            <asp:ListItem>50</asp:ListItem>
            <asp:ListItem>100</asp:ListItem>
            <asp:ListItem>All</asp:ListItem>
        </asp:DropDownList>
        <br class="clear"/>

    </div>
    <br class="clear"/>
    <asp:GridView ID="gvLogs" runat="server" DataKeyNames="Id" AutoGenerateColumns="False" CssClass="Gridview extraPad" AlternatingRowStyle-CssClass="alt">
        <Columns>

            <asp:TemplateField>
                <HeaderStyle CssClass="chkboxwidth"></HeaderStyle>
                <ItemStyle CssClass="chkboxwidth"></ItemStyle>
                <ItemTemplate>
                    <asp:LinkButton ID="btnView" runat="server" OnClick="btnView_OnClick" Text="View"/>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemStyle CssClass="chkboxwidth"></ItemStyle>
                <ItemTemplate>
                    <asp:LinkButton ID="btnExport" runat="server" OnClick="btnExport_OnClick" Text="Export"/>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Id" Visible="False"/>
            <asp:BoundField DataField="Mac" HeaderText="Mac" SortExpression="Mac" ItemStyle-CssClass="width_200"></asp:BoundField>
            <asp:BoundField DataField="LogTime" HeaderText="Time" SortExpression="LogTime" ItemStyle-CssClass="width_200"></asp:BoundField>
            <asp:BoundField DataField="SubType" HeaderText="Type" SortExpression="SubType"/>


        </Columns>
        <EmptyDataTemplate>
            No Logs Found
        </EmptyDataTemplate>
    </asp:GridView>

    <div id="ViewLog" runat="server" Visible="False">
        <asp:GridView ID="gvLogView" runat="server" CssClass="Gridview log" ShowHeader="false">
        </asp:GridView>
    </div>

</asp:Content>