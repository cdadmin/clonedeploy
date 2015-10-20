<%@ Page Title="" Language="C#" MasterPageFile="~/views/computers/computers.master" AutoEventWireup="true" Inherits="views.hosts.Searchhosts" CodeFile="search.aspx.cs" %>

<asp:Content ID="Breadcrumb" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li>Search</li>
    </asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Help">
     <a href="<%= ResolveUrl("~/views/help/index.html")%>" class="icon help" data-info="Help" target="_blank"></a>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="SubPageActionsRight">
    <a class="confirm left" href="#">Delete Selected Computers</a>
</asp:Content>
<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#search').addClass("nav-current");
        });
    </script>
    <div class="size-7 column">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="searchbox" OnTextChanged="search_Changed"></asp:TextBox>
    </div>
    <br class="clear"/>
    
    <div class="full column">
    <p class="total">
        <asp:Label ID="lblTotal" runat="server"></asp:Label>
    </p>
    <asp:GridView ID="gvHosts" runat="server" AllowSorting="True" DataKeyNames="Id" OnSorting="gridView_Sorting" AutoGenerateColumns="False" CssClass="Gridview" AlternatingRowStyle-CssClass="alt">
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
             <asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="~/views/computers/edit.aspx?hostid={0}" Text="View" ItemStyle-CssClass="chkboxwidth"/>
            <asp:BoundField DataField="Id" HeaderText="hostID" SortExpression="hostID" Visible="False"/>
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-CssClass="width_200"></asp:BoundField>
            <asp:BoundField DataField="Mac" HeaderText="MAC" SortExpression="Mac" ItemStyle-CssClass="width_200 mobi-hide-smallest" HeaderStyle-CssClass="mobi-hide-smallest"/>
            <asp:BoundField DataField="Image.Name" HeaderText="Image" SortExpression="Image.Name" ItemStyle-CssClass="width_200 mobi-hide-smaller" HeaderStyle-CssClass="mobi-hide-smaller"/>
         
          
        </Columns>
        <EmptyDataTemplate>
            No Computers Found
        </EmptyDataTemplate>
    </asp:GridView>
    </div>
    <div id="confirmbox" class="confirm-box-outer">
        <div class="confirm-box-inner">
            <h4>
                <asp:Label ID="lblTitle" runat="server" Text="Delete The Selected Hosts?"></asp:Label>
            </h4>
            <div class="confirm-box-btns">
                <asp:LinkButton ID="ConfirmButton" OnClick="ButtonConfirmDelete_Click" runat="server" Text="Yes" CssClass="confirm_yes"/>
                <asp:LinkButton ID="CancelButton" runat="server" Text="No" CssClass="confirm_no"/>
            </div>
        </div>
    </div>
</asp:Content>