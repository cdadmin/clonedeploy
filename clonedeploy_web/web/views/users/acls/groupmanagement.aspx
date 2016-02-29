<%@ Page Title="" Language="C#" MasterPageFile="~/views/users/acls/acls.master" AutoEventWireup="true" CodeFile="groupmanagement.aspx.cs" Inherits="views_users_acls_groupmanagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
    <li>Group Based Computer Management</li>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="SubHelp">
    <a href="<%= ResolveUrl("~/views/help/index.html") %>" class="submits help" target="_blank"></a>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ActionsRightSub">
     <asp:LinkButton ID="buttonUpdate" runat="server" OnClick="buttonUpdate_OnClick" Text="Update Group Management" CssClass="submits actions green" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
     <script type="text/javascript">
        $(document).ready(function() {
            $('#group').addClass("nav-current");
        });
    </script>
     
    <asp:GridView ID="gvGroups" runat="server" AllowSorting="true" AutoGenerateColumns="False" CssClass="Gridview" DataKeyNames="Id" AlternatingRowStyle-CssClass="alt">
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
            <asp:BoundField DataField="Id" HeaderText="groupID" InsertVisible="False" SortExpression="Id" Visible="False"/>
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-CssClass="width_200"/>
            
            <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type" HeaderStyle-CssClass="mobi-hide-smallest" ItemStyle-CssClass="width_200 mobi-hide-smallest"/>
         
        </Columns>
        <EmptyDataTemplate>
            No Groups Have Been Created
        </EmptyDataTemplate>
    </asp:GridView>
    
 
</asp:Content>

