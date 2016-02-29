<%@ Page Title="" Language="C#" MasterPageFile="~/views/global/boottemplates/boottemplates.master" AutoEventWireup="true" CodeFile="search.aspx.cs" Inherits="views_global_boottemplates_search" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
    <li>Search</li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SubHelp" Runat="Server">
    <a href="<%= ResolveUrl("~/views/help/index.html") %>" class="submits help" target="_blank"></a>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ActionsRightSub" Runat="Server">
    <a class="confirm actions green" href="#">Delete Selected Templates</a>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#search').addClass("nav-current");
        });
    </script>
    <div class="size-7 column">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="searchbox" OnTextChanged="txtSearch_OnTextChanged"></asp:TextBox>
    </div>
    <br class="clear"/>
    <p class="total">
        <asp:Label ID="lblTotal" runat="server"></asp:Label>
    </p>
    <asp:GridView ID="gvTemplates" runat="server" AllowSorting="True" DataKeyNames="Id" OnSorting="gvTemplates_OnSorting" AutoGenerateColumns="False" CssClass="Gridview" AlternatingRowStyle-CssClass="alt">
        <Columns>
            <asp:TemplateField>
                <HeaderStyle CssClass="chkboxwidth"></HeaderStyle>
                <ItemStyle CssClass="chkboxwidth"></ItemStyle>
                <HeaderTemplate>
                    <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelectAll_OnCheckedChanged"/>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="chkSelector" runat="server"/>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Id" HeaderText="computerID" SortExpression="computerID" Visible="False"/>
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-CssClass="width_200"></asp:BoundField>
          
          
         
            <asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="~/views/global/boottemplates/edit.aspx?cat=sub1&templateid={0}" Text="View"/>
        </Columns>
        <EmptyDataTemplate>
            No Templates Found
        </EmptyDataTemplate>
    </asp:GridView>
  
    <div id="confirmbox" class="confirm-box-outer">
        <div class="confirm-box-inner">
            <h4>
                <asp:Label ID="lblTitle" runat="server" Text="Delete The Selected Boot Menu Templates?"></asp:Label>
            </h4>
            <div class="confirm-box-btns">
                <asp:LinkButton ID="ConfirmButton" OnClick="ButtonConfirmDelete_Click" runat="server" Text="Yes" CssClass="confirm_yes"/>
                <asp:LinkButton ID="CancelButton" runat="server" Text="No" CssClass="confirm_no"/>
            </div>
        </div>
    </div>
</asp:Content>

