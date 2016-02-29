<%@ Page Title="" Language="C#" MasterPageFile="~/views/users/user.master" AutoEventWireup="true" Inherits="views.users.UserSearch" CodeFile="search.aspx.cs" %>

<asp:Content ID="Breadcrumb" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li>Search</li>
    </asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Help">
     <a href="<%= ResolveUrl("~/views/help/index.html")%>" class="submits help" target="_blank"></a>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="SubPageActionsRight">
          <a class="confirm actions green" href="#">Delete Selected Users</a>

</asp:Content>

<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#searchuser').addClass("nav-current");
        });
    </script>
     <p class="total">
        <asp:Label ID="lblTotal" runat="server"></asp:Label>
    </p>
    <div class="size-7 column">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="searchbox" OnTextChanged="search_Changed"></asp:TextBox>
    </div>
    <br class="clear"/>
   
    <asp:GridView ID="gvUsers" runat="server" AllowSorting="True" DataKeyNames="Id" OnSorting="gridView_Sorting" AutoGenerateColumns="False" CssClass="Gridview" AlternatingRowStyle-CssClass="alt">
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
            <asp:BoundField DataField="Id" HeaderText="userID" SortExpression="Id" Visible="False"/>
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name"/>
            <asp:BoundField DataField="Membership" HeaderText="Membership" SortExpression="Membership" ItemStyle-CssClass="mobi-hide-smallest" HeaderStyle-CssClass="mobi-hide-smallest"/>
            <asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="~/views/users/edit.aspx?userid={0}" Text="View"/>
        </Columns>
        <EmptyDataTemplate>
            No Users Found
        </EmptyDataTemplate>
    </asp:GridView>
  
    <div id="confirmbox" class="confirm-box-outer">
        <div class="confirm-box-inner">
            <h4>
                <asp:Label ID="lblTitle" runat="server" Text="Delete The Selected Users?" CssClass="modaltitle"></asp:Label>
            </h4>
            <div class="confirm-box-btns">
                <asp:LinkButton ID="ConfirmButton" OnClick="btnSubmit_Click" runat="server" Text="Yes" CssClass="confirm_yes"/>
                <asp:LinkButton ID="CancelButton" runat="server" Text="No" CssClass="confirm_no"/>
            </div>
        </div>
    </div>
</asp:Content>