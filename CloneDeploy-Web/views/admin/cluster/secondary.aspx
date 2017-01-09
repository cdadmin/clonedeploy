<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/cluster/cluster.master" AutoEventWireup="true" CodeBehind="secondary.aspx.cs" Inherits="CloneDeploy_Web.views.admin.cluster.secondary" %>
<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" runat="server">
    <li>Secondary Servers</li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubHelp" runat="server">
       <li role="separator" class="divider"></li>
    <li><a href="<%= ResolveUrl("~/views/help/admin-secondary.aspx") %>"   target="_blank">Help</a></li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ActionsRightSub" runat="server">
     <a class="confirm btn btn-default" href="#">Delete Selected Servers</a>
     <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
    <span class="caret"></span>
  </button>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="SubContent2" runat="server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#secondary').addClass("nav-current");
            $("[id*=gvServers] td").hover(function () {
                $("td", $(this).closest("tr")).addClass("hover_row");
            }, function () {
                $("td", $(this).closest("tr")).removeClass("hover_row");
            });
        });
    </script>
    
     <div class="size-7 column">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="searchbox" OnTextChanged="search_Changed"></asp:TextBox>
    </div>
    <br class="clear"/>
   
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
             <asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="~/views/admin/cluster/editsecondary.aspx?level=2&ssid={0}" Text="View" ItemStyle-CssClass="chkboxwidth"/>
            <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False"/>
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="DisplayName" ItemStyle-CssClass="width_200"></asp:BoundField>
            <asp:BoundField DataField="ImageRole" HeaderText="Image Role" SortExpression="Server" ItemStyle-CssClass="width_200"/>
            <asp:BoundField DataField="TftpRole" HeaderText="Tftp Role" SortExpression="Server" ItemStyle-CssClass="width_200"/>
            <asp:BoundField DataField="MulticastRole" HeaderText="Multicast Role" SortExpression="Server" />
           
         
           
        </Columns>
        <EmptyDataTemplate>
            No Servers Found
        </EmptyDataTemplate>
    </asp:GridView>
  
      <div id="confirmbox" class="confirm-box-outer">
        <div class="confirm-box-inner">
            <h4>
                <asp:Label ID="lblTitle" runat="server" Text="Delete The Selected Servers?"></asp:Label>
            </h4>
            <div class="confirm-box-btns">
                <asp:LinkButton ID="ConfirmButton" OnClick="ButtonConfirmDelete_Click" runat="server" Text="Yes" CssClass="confirm_yes"/>
                <asp:LinkButton ID="CancelButton" runat="server" Text="No" CssClass="confirm_no"/>
            </div>
        </div>
    </div>
</asp:Content>
