<%@ Page Title="" Language="C#" MasterPageFile="~/views/groups/groups.master" AutoEventWireup="true" CodeFile="munki.aspx.cs" Inherits="views_groups_munki" %>

<asp:Content ID="Breadcrumb" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
   <li><a href="<%= ResolveUrl("~/views/groups/edit.aspx") %>?groupid=<%= Group.Id %>" ><%= Group.Name %></a></li>
    <li>Munki Manifest Templates</li>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Help">
     <a href="<%= ResolveUrl("~/views/help/index.html")%>" target="_blank">Help</a>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="SubPageActionsRight">
       <asp:LinkButton ID="btnAddSelected" runat="server" OnClick="btnAddSelected_OnClick" Text="Update Templates " />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="SubContent" Runat="Server">
     <script type="text/javascript">
        $(document).ready(function() {
            $('#munki').addClass("nav-current");
            $("[id*=gvManifestTemplates] td").hover(function () {
                $("td", $(this).closest("tr")).addClass("hover_row");
            }, function () {
                $("td", $(this).closest("tr")).removeClass("hover_row");
            });
        });
    </script>
   
   
     <asp:GridView ID="gvManifestTemplates" runat="server" AllowSorting="True" DataKeyNames="Id" OnSorting="gridView_Sorting" AutoGenerateColumns="False" CssClass="Gridview" AlternatingRowStyle-CssClass="alt">
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
             <asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="~/views/global/munki/general.aspx?cat=sub2&manifestid={0}" Text="View" ItemStyle-CssClass="chkboxwidth" Target="_blank"/>
          
            <asp:BoundField DataField="Id" HeaderText="ID" SortExpression="Id" Visible="False"/>
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ></asp:BoundField>
          
        </Columns>
        <EmptyDataTemplate>
            No Manifest Templates Found
        </EmptyDataTemplate>
    </asp:GridView>
   
    <asp:LinkButton runat="server" Id="effective" OnClick="effective_OnClick" CssClass="generic-btn" Text="Test"></asp:LinkButton>
</asp:Content>

