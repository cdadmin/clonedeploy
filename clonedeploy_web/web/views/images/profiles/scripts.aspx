<%@ Page Title="" Language="C#" MasterPageFile="~/views/images/profiles/profiles.master" AutoEventWireup="true" CodeFile="scripts.aspx.cs" Inherits="views_images_profiles_scripts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
      <li><a href="<%= ResolveUrl("~/views/images/profiles/chooser.aspx") %>?imageid=<%= Image.Id %>&profileid=<%= ImageProfile.Id %>&cat=profiles"><%= ImageProfile.Name %></a></li>
    <li>Scripts</li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
     <script type="text/javascript">
        $(document).ready(function() {
            $('#scripts').addClass("nav-current");
        });
    </script>
     <asp:GridView ID="gvScripts" runat="server" AllowSorting="True" DataKeyNames="Id" OnSorting="gridView_Sorting" AutoGenerateColumns="False" CssClass="Gridview" AlternatingRowStyle-CssClass="alt">
        <Columns>
            <asp:TemplateField>
                <HeaderStyle CssClass="chkboxwidth"></HeaderStyle>
                <ItemStyle CssClass="chkboxwidth"></ItemStyle>
                <HeaderTemplate>
                    Pre
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="chkPre" runat="server"/>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField>
                <HeaderStyle CssClass="chkboxwidth"></HeaderStyle>
                <ItemStyle CssClass="chkboxwidth"></ItemStyle>
                <HeaderTemplate>
                    Post
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="chkPost" runat="server"/>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Id" HeaderText="hostID" SortExpression="hostID" Visible="False"/>
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-CssClass="width_200"></asp:BoundField>
            <asp:BoundField DataField="Priority" HeaderText="Priority" SortExpression="Priority" ItemStyle-CssClass="width_200 mobi-hide-smallest" HeaderStyle-CssClass="mobi-hide-smallest"/>
          
         
            <asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="~/views/global/scripts/edit.aspx?cat=sub1&scriptid={0}" Text="View"/>
        </Columns>
        <EmptyDataTemplate>
            No Scripts Found
        </EmptyDataTemplate>
    </asp:GridView>
    
    <br class="clear"/>
      <div class="size-4 column">
        &nbsp;
    </div>
    <div class="size-5 column">
        <asp:LinkButton ID="btnUpdateScripts" runat="server" OnClick="btnUpdateScripts_OnClick" Text="Update Script Options" CssClass="submits" />
    </div>
</asp:Content>

