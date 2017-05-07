<%@ Page Title="" Language="C#" MasterPageFile="~/views/users/user.master" AutoEventWireup="true" Inherits="views_users_addmembers" Codebehind="addmembers.aspx.cs" %>

<asp:Content ID="Breadcrumb" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li>
        <a href="<%= ResolveUrl("~/views/users/editgroup.aspx") %>?groupid=<%= CloneDeployUserGroup.Id %>"><%= CloneDeployUserGroup.Name %></a>
    </li>
    <li>Add Members</li>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Help">
    <li role="separator" class="divider"></li>
    <li>
        <a href="<%= ResolveUrl("~/views/help/users-groupaddmembers.aspx") %>" target="_blank">Help</a>
    </li>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="SubPageActionsRight">
    <asp:LinkButton ID="btnAddSelected" runat="server" OnClick="btnAddSelected_OnClick" Text="Add Selected Users" CssClass="btn btn-default"/>
    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        <span class="caret"></span>
    </button>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="SubContent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#addmembers').addClass("nav-current");

            $("[id*=gvUser] td").hover(function() {
                $("td", $(this).closest("tr")).addClass("hover_row");
            }, function() {
                $("td", $(this).closest("tr")).removeClass("hover_row");
            });
        });
    </script>
    <p class="total">
        <asp:Label ID="lblTotal" runat="server"></asp:Label>
    </p>
    <div class="size-7 column">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="searchbox" OnTextChanged="search_Changed"></asp:TextBox>
    </div>
    <br class="clear"/>


    <asp:GridView ID="gvUsers" runat="server" DataKeyNames="Id" AutoGenerateColumns="False" CssClass="Gridview" AlternatingRowStyle-CssClass="alt">
        <Columns>
            <asp:TemplateField>

                <ItemStyle CssClass="chkboxwidth"></ItemStyle>
                <HeaderTemplate>
                    <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelectAll_CheckedChanged"/>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="chkSelector" runat="server"/>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Id" HeaderText="computerID" SortExpression="computerID" Visible="False"/>
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-CssClass="width_200"></asp:BoundField>
            <asp:BoundField DataField="Membership" HeaderText="Role" SortExpression="Mac" ItemStyle-CssClass="width_200 mobi-hide-smallest" HeaderStyle-CssClass="mobi-hide-smallest"/>
            <asp:TemplateField ItemStyle-CssClass="mobi-hide-smaller" HeaderStyle-CssClass="mobi-hide-smaller" HeaderText="Current Group">
                <ItemTemplate>
                    <asp:Label ID="lblImage" runat="server" Text='<%# Bind("UserGroup.Name") %>'/>
                </ItemTemplate>
            </asp:TemplateField>


        </Columns>
        <EmptyDataTemplate>
            No Users Found
        </EmptyDataTemplate>
    </asp:GridView>


</asp:Content>