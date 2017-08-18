<%@ Page Title="" Language="C#" MasterPageFile="~/views/global/global.master" AutoEventWireup="true" CodeBehind="search.aspx.cs" Inherits="CloneDeploy_Web.views.global.imageclassifications.search" %>
<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li>Image Classifications</li>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Help" Runat="Server">
    <li role="separator" class="divider"></li>
    <li>
        <a href="<%= ResolveUrl("~/views/help/global-imageclass.aspx") %>" target="_blank">Help</a>
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
            $('#imageclassification').addClass("nav-current");
        });
    </script>
   
    <br class="clear"/>


    <asp:GridView ID="gvImageClass" runat="server" AutoGenerateColumns="false" DataKeyNames="Id" CssClass="Gridview extraPad"
                  OnRowEditing="OnRowEditing" OnRowCancelingEdit="OnRowCancelingEdit" ShowFooter="True"
                  OnRowUpdating="OnRowUpdating" OnRowDeleting="OnRowDeleting"  AlternatingRowStyle-CssClass="alt">
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

          

            <asp:TemplateField ShowHeader="False">
                <EditItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True"
                                    CommandName="Update" Text="Update">
                    </asp:LinkButton>
                    &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False"
                                          CommandName="Cancel" Text="Cancel">
                    </asp:LinkButton>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False"
                                    CommandName="Edit" Text="Edit">
                    </asp:LinkButton>
                    <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False"
                                    CommandName="Delete" Text="Delete">
                    </asp:LinkButton>
                </ItemTemplate>
                <FooterTemplate>

                    <asp:LinkButton ID="btnAdd1" runat="server" Text="Add" OnClick="Insert" CssClass="submits"/>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField></asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
