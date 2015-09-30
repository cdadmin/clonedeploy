<%@ Page Title="" Language="C#" MasterPageFile="~/views/global/global.master" AutoEventWireup="true" CodeFile="search.aspx.cs" Inherits="views_global_rooms_search" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li>Rooms</li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Level2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#rooms').addClass("nav-current");
        });
    </script>
    <div class="size-7 column">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="searchbox" OnTextChanged="search_Changed" AutoPostBack="True"></asp:TextBox>
    </div>
    <br class="clear"/>


    <asp:GridView ID="gvRooms" runat="server" AutoGenerateColumns="false" DataKeyNames="Id" CssClass="Gridview"
                  OnRowEditing="OnRowEditing" OnRowCancelingEdit="OnRowCancelingEdit" ShowFooter="True"
                  OnRowUpdating="OnRowUpdating" OnRowDeleting="OnRowDeleting" AlternatingRowStyle-CssClass="alt">
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

                    <asp:Button ID="btnAdd1" runat="server" Text="Add" OnClick="Insert"/>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField></asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>

