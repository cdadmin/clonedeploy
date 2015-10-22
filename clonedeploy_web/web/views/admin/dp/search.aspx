<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/dp/dp.master" AutoEventWireup="true" CodeFile="search.aspx.cs" Inherits="views_admin_dp_search" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="SubHelp">
     <a href="<%= ResolveUrl("~/views/help/index.html")%>" class="icon help" data-info="Help" target="_blank"></a>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ActionsRightSub">
   <a class="confirm" href="#">Delete Selected Distribution Points</a>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#search').addClass("nav-current");
        });
    </script>
    <div class="size-7 column">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="searchbox" OnTextChanged="search_Changed"></asp:TextBox>
    </div>
    <br class="clear"/>
    <p class="total">
        <asp:Label ID="lblTotal" runat="server"></asp:Label>
    </p>
    <asp:GridView ID="gvDps" runat="server" AllowSorting="True" DataKeyNames="Id" OnSorting="gridView_Sorting" AutoGenerateColumns="False" CssClass="Gridview" AlternatingRowStyle-CssClass="alt">
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
            <asp:BoundField DataField="DisplayName" HeaderText="Display Name" SortExpression="DisplayName" ItemStyle-CssClass="width_200"></asp:BoundField>
            <asp:BoundField DataField="Server" HeaderText="Server" SortExpression="Server" ItemStyle-CssClass="width_200 mobi-hide-smallest" HeaderStyle-CssClass="mobi-hide-smallest"/>
            <asp:BoundField DataField="ShareName" HeaderText="Share" SortExpression="ShareName" ItemStyle-CssClass="width_200 mobi-hide-smaller" HeaderStyle-CssClass="mobi-hide-smaller"/>
            <asp:BoundField DataField="IsPrimary" HeaderText="Primary" SortExpression="IsPrimary" ItemStyle-CssClass="width_200 mobi-hide-smaller" HeaderStyle-CssClass="mobi-hide-smaller"/>
         
            <asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="~/views/admin/dp/edit.aspx?level=2&dpid={0}" Text="View"/>
        </Columns>
        <EmptyDataTemplate>
            No Distribution Points Found
        </EmptyDataTemplate>
    </asp:GridView>
  
      <div id="confirmbox" class="confirm-box-outer">
        <div class="confirm-box-inner">
            <h4>
                <asp:Label ID="lblTitle" runat="server" Text="Delete The Selected Distribution Points?"></asp:Label>
            </h4>
            <div class="confirm-box-btns">
                <asp:LinkButton ID="ConfirmButton" OnClick="ButtonConfirmDelete_Click" runat="server" Text="Yes" CssClass="confirm_yes"/>
                <asp:LinkButton ID="CancelButton" runat="server" Text="No" CssClass="confirm_no"/>
            </div>
        </div>
    </div>
</asp:Content>

