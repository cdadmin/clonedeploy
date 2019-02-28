<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/cluster/cluster.master" AutoEventWireup="true" CodeBehind="editcluster.aspx.cs" Inherits="CloneDeploy_Web.views.admin.cluster.editcluster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" runat="server">
    <li>Edit Cluster Group</li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubHelp" runat="server">
    <li role="separator" class="divider"></li>
    <li>
        <a href="<%= ResolveUrl("~/views/help/admin-cluster.aspx#admin") %>" target="_blank">Help</a>
    </li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ActionsRightSub" runat="server">
    <asp:LinkButton ID="btnUpdateCluster" runat="server" Text="Update Cluster Group" OnClick="btnUpdateCluster_OnClick" CssClass="btn btn-default"/>
    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        <span class="caret"></span></button>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="SubContent2" runat="server">

    <div class="size-4 column">
        Cluster Group Name:
    </div>
    <div class="size-5 column">
        <asp:TextBox runat="server" Id="txtClusterName" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Default Cluster Group:
    </div>
    <div class="size-5 column">
        <asp:CheckBox runat="server" Id="chkDefault"></asp:CheckBox>
    </div>
    <br class="clear"/>
    <br/>
    <h4>Select Servers And Roles For The Group:</h4>
    <asp:GridView ID="gvServers" runat="server" DataKeyNames="Id" AutoGenerateColumns="False" CssClass="Gridview" AlternatingRowStyle-CssClass="alt">
        <Columns>
            <asp:TemplateField>
                <HeaderStyle CssClass="chkboxwidth"></HeaderStyle>
                <ItemStyle CssClass="chkboxwidth"></ItemStyle>
                <HeaderTemplate>
                    <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelectAll_CheckedChanged"/>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="chkSelector" runat="server"/>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False"/>
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="DisplayName" ItemStyle-CssClass="width_200"></asp:BoundField>

            <asp:TemplateField ItemStyle-CssClass="width_100" HeaderText="Tftp Role">
                <ItemTemplate>
                    <asp:CheckBox ID="chkTftp" runat="server"/>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-CssClass="width_100" HeaderText="Multicast Role">
                <ItemTemplate>
                    <asp:CheckBox ID="chkMulticast" runat="server"/>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            No Servers Found
        </EmptyDataTemplate>
    </asp:GridView>

    <br class="clear"/>
    <br/>
    <h4>Select Distribution Points For The Group:</h4>
    <asp:GridView ID="gvDps" runat="server" DataKeyNames="Id" AutoGenerateColumns="False" CssClass="Gridview" AlternatingRowStyle-CssClass="alt">
        <Columns>
            <asp:TemplateField>
                <HeaderStyle CssClass="chkboxwidth"></HeaderStyle>
                <ItemStyle CssClass="chkboxwidth"></ItemStyle>
                <HeaderTemplate>
                    <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelectAll_CheckedChanged"/>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="chkSelector" runat="server"/>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False"/>
            <asp:BoundField DataField="DisplayName" HeaderText="Display Name" SortExpression="DisplayName"></asp:BoundField>

        </Columns>
        <EmptyDataTemplate>
            No Distribution Points Found
        </EmptyDataTemplate>
    </asp:GridView>
</asp:Content>