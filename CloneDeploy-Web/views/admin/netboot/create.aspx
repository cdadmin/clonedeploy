<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/netboot/netboot.master" AutoEventWireup="true" CodeBehind="create.aspx.cs" Inherits="CloneDeploy_Web.views.admin.netboot.create" %>
<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
    <li>New</li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SubHelp" Runat="Server">
    <li role="separator" class="divider"></li>
    <li>
        <a href="<%= ResolveUrl("~/views/help/admin-netboot.aspx") %>" target="_blank">Help</a>
    </li>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ActionsRightSub">
    <asp:LinkButton ID="buttonAddNetBoot" runat="server" OnClick="buttonAddNetBoot_OnClick" Text="Add NetBoot Profile" CssClass="btn btn-default"/>
    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        <span class="caret"></span>
    </button>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#create').addClass("nav-current");
        });
    </script>
    <div class="size-4 column">
        Profile Name:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtDisplayName" runat="server" CssClass="textbox" ClientIDMode="Static"></asp:TextBox>

    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Proxy DHCP Bound Ip Address:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtIp" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>


    <br/>
    <div class="size-1 column">
        <asp:GridView ID="gvNetBoot" runat="server" AutoGenerateColumns="false" DataKeyNames="ImageId" CssClass="Gridview extraPad"
                      OnRowEditing="OnRowEditing" OnRowCancelingEdit="OnRowCancelingEdit" ShowFooter="True"
                      OnRowUpdating="OnRowUpdating" OnRowDeleting="OnRowDeleting" AlternatingRowStyle-CssClass="alt">
            <Columns>

                <asp:TemplateField HeaderText="NBI Id" ItemStyle-Width="150">
                    <ItemTemplate>
                        <asp:Label ID="lblImageId" runat="server" Text='<%# Eval("ImageId") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtIdEdit" runat="server" Text='<%# Eval("ImageId") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtIdAdd" runat="server"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="NBI Name" ItemStyle-Width="150">
                    <ItemTemplate>
                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtNameEdit" runat="server" Text='<%# Eval("Name") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtNameAdd" runat="server"></asp:TextBox>
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
                                        CommandName="Delete" Text="Delete">
                        </asp:LinkButton>
                    </ItemTemplate>
                    <FooterTemplate>

                        <asp:LinkButton ID="btnAdd1" runat="server" Text="Add" OnClick="btnAdd1_OnClick" CssClass="btn-default"/>
                    </FooterTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>
    </div>
    <br class="clear"/>


</asp:Content>