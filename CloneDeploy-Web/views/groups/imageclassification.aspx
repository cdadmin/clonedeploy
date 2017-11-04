<%@ Page Title="" Language="C#" MasterPageFile="~/views/groups/groups.master" AutoEventWireup="true" CodeBehind="imageclassification.aspx.cs" Inherits="CloneDeploy_Web.views.groups.imageclassification" %>
<asp:Content ID="Breadcrumb" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
     <li>
        <a href="<%= ResolveUrl("~/views/groups/edit.aspx") %>?groupid=<%= Group.Id %>"><%= Group.Name %></a>
    </li>
    <li>Image Classifications</li>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Help">
    <li role="separator" class="divider"></li>
    <li>
        <a href="<%= ResolveUrl("~/views/help/groups-classifications.aspx") %>" class="" target="_blank">Help</a>
    </li>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="SubPageActionsRight">
    <asp:LinkButton ID="buttonUpdate" runat="server" OnClick="buttonUpdate_OnClick" Text="Update Classifications" CssClass="btn btn-default"></asp:LinkButton>
    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        <span class="caret"></span>
    </button>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#imageclassification').addClass("nav-current");

            $("[id*=gvClassifications] td").hover(function() {
                $("td", $(this).closest("tr")).addClass("hover_row");
            }, function() {
                $("td", $(this).closest("tr")).removeClass("hover_row");
            });
        });

    </script>

    <asp:GridView ID="gvClassifications" runat="server" AutoGenerateColumns="False" CssClass="Gridview" DataKeyNames="Id" AlternatingRowStyle-CssClass="alt">
        <Columns>
            <asp:TemplateField>
                <HeaderStyle CssClass="chkboxwidth"></HeaderStyle>
                <ItemStyle CssClass="chkboxwidth"></ItemStyle>
                <HeaderTemplate>
                    <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="True" OnCheckedChanged="SelectAll_CheckedChanged"/>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="chkSelector" runat="server"/>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Id" HeaderText="ID" InsertVisible="False" SortExpression="Id" Visible="False"/>
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-CssClass="width_200"/>
        </Columns>
        <EmptyDataTemplate>
            No Image Classifications Have Been Created
        </EmptyDataTemplate>
    </asp:GridView>
</asp:Content>