<%@ Page Title="" Language="C#" MasterPageFile="~/views/masters/Profile.master" AutoEventWireup="true" CodeFile="search.aspx.cs" Inherits="views_images_profiles_search" %>

<%@ MasterType VirtualPath="~/views/masters/Profile.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
    <li>Search</li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#search').addClass("nav-current");
        });
    </script>
   
   
    <br class="clear"/>

    <asp:GridView ID="gvProfiles" runat="server" AllowSorting="True" DataKeyNames="Id" OnSorting="gridView_Sorting" AutoGenerateColumns="False" CssClass="Gridview" AlternatingRowStyle-CssClass="alt">
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
            <asp:BoundField DataField="Id" HeaderText="ID" SortExpression="ID" Visible="False"/>
            <asp:BoundField DataField="ImageId" HeaderText="ImageID" SortExpression="ImageID" Visible="False"/>
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-CssClass="width_200"></asp:BoundField>
            <asp:HyperLinkField DataNavigateUrlFields="Id,ImageId" DataNavigateUrlFormatString="~/views/images/profiles/chooser.aspx?imageid={1}&profileid={0}&subid=profiles" Text="View"/>
        </Columns>
        <EmptyDataTemplate>
            No Profiles Found
        </EmptyDataTemplate>
    </asp:GridView>
    <a class="confirm left" href="#">Delete Selected Profiles</a>
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

