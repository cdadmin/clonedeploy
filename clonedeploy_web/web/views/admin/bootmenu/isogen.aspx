<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/bootmenu/bootmenu.master" AutoEventWireup="true" CodeFile="isogen.aspx.cs" Inherits="views_admin_bootmenu_isogen" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubHelp" Runat="Server">
    <a href="<%= ResolveUrl("~/views/help/index.html") %>" class="submits help" target="_blank"></a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ActionsRightSub" Runat="Server">
      <asp:LinkButton ID="btnSaveEditor" runat="server" Text="Generate" OnClick="btnSaveEditor_OnClick" CssClass="submits actions green" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="SubContent2" Runat="Server">
    <script type="text/javascript">
            $(document).ready(function() {
                $('#iso').addClass("nav-current");
            });
        </script>
     <div class="size-4 column">
                Build Type:
            </div>
            <div class="size-4 column">
                <asp:DropDownList ID="ddlBuildType" runat="server" CssClass="ddlist" >
                    <asp:ListItem>ISO</asp:ListItem>
                    <asp:ListItem>USB</asp:ListItem>
                    
                </asp:DropDownList>
            </div>
            <br class="clear"/>
     <div class="size-4 column">
        Kernel:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlKernel" runat="server" CssClass="ddlist">
        </asp:DropDownList>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Boot Image:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlBootImage" runat="server" CssClass="ddlist">
        </asp:DropDownList>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Kernel Arguments:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtKernelArgs" runat="server" CssClass="textbox">
        </asp:TextBox>
    </div>
</asp:Content>

