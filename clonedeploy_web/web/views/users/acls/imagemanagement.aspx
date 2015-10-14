<%@ Page Title="" Language="C#" MasterPageFile="~/views/users/acls/acls.master" AutoEventWireup="true" CodeFile="imagemanagement.aspx.cs" Inherits="views_users_acls_imagemanagement" %>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SubContent2" Runat="Server">
     <script type="text/javascript">
        $(document).ready(function() {
            $('#image').addClass("nav-current");
        });
    </script>
     
    <asp:GridView ID="gvImages" runat="server" AllowSorting="true" AutoGenerateColumns="False" CssClass="Gridview" DataKeyNames="Id" AlternatingRowStyle-CssClass="alt">
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
            <asp:BoundField DataField="Id" Visible="False"/>
            <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-CssClass="width_200"/>
        </Columns>
        <EmptyDataTemplate>
            No Images Have Been Created
        </EmptyDataTemplate>
    </asp:GridView>
    
    <div class="size-5 column">
        <asp:LinkButton ID="buttonUpdate" runat="server" OnClick="buttonUpdate_OnClick" Text="Update Image Management" CssClass="submits" />
    </div>
</asp:Content>
