<%@ Page Title="" Language="C#" MasterPageFile="~/views/global/global.master" AutoEventWireup="true" Inherits="views_global_sites_search" Codebehind="search.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub" runat="Server">
    <li>Sites</li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Help" Runat="Server">
    <li role="separator" class="divider"></li>
    <li>
        <a href="<%= ResolveUrl("~/views/help/global-sites.aspx") %>" target="_blank">Help</a>
    </li>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SubPageActionsRight" Runat="Server">
    <button type="button" class="btn btn-default dropdown-toggle width_140" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        <span class="caret"></span>
    </button>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#sites').addClass("nav-current");
        });
    </script>
    <div class="size-7 column">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="searchbox" OnTextChanged="search_Changed" AutoPostBack="True"></asp:TextBox>
    </div>
    <br class="clear"/>


    <asp:GridView ID="gvSites" runat="server" AutoGenerateColumns="false" DataKeyNames="Id" CssClass="Gridview extraPad"
                  OnRowEditing="OnRowEditing" OnRowCancelingEdit="OnRowCancelingEdit" ShowFooter="True"
                  OnRowUpdating="OnRowUpdating" OnRowDeleting="OnRowDeleting" OnRowDataBound="gvSites_OnRowDataBound" AlternatingRowStyle-CssClass="alt">
        <Columns>

            <asp:TemplateField HeaderText="Name" ItemStyle-Width="150">
                <ItemTemplate>
                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtName" runat="server" Text='<%# Eval("Name") %>'></asp:TextBox>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox ID="txtNameAdd" runat="server"></asp:TextBox>
                </FooterTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Cluster Group" ItemStyle-Width="150">
                <ItemTemplate>
                    <asp:Label ID="lblDp" runat="server" Text='<%# Eval("ClusterGroup.Name") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="ddlDp" runat="server" CssClass="ddlist"></asp:DropDownList>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:DropDownList ID="ddlDpAdd" runat="server" CssClass="ddlist"></asp:DropDownList>
                </FooterTemplate>
            </asp:TemplateField>

            <asp:TemplateField ShowHeader="False">
                <EditItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True"
                                    CommandName="Update" Text="Update" CssClass="btn btn-default">
                    </asp:LinkButton>
                    &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False"
                                          CommandName="Cancel" Text="Cancel" CssClass="btn btn-default">
                    </asp:LinkButton>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False"
                                    CommandName="Edit" Text="Edit" CssClass="btn btn-default">
                    </asp:LinkButton>
                    <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False"
                                    CommandName="Delete" Text="Delete" CssClass="btn btn-default">
                    </asp:LinkButton>
                </ItemTemplate>
                <FooterTemplate>

                    <asp:LinkButton ID="btnAdd1" runat="server" Text="Add" OnClick="Insert" CssClass="btn btn-default"/>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField></asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>