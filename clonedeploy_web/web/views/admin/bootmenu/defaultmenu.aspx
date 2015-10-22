<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/bootmenu/bootmenu.master" AutoEventWireup="true" CodeFile="defaultmenu.aspx.cs" Inherits="views_admin_bootmenu_defaultmenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubHelp" Runat="Server">
    <a href="<%= ResolveUrl("~/views/help/index.html") %>" class="icon help" data-info="Help" target="_blank"></a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ActionsRightSub" Runat="Server">
    <asp:LinkButton ID="btnSubmitDefault" runat="server" Text="Create Boot Files" OnClick="btnSubmit_Click" CssClass="submits" OnClientClick="get_shas();"/>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="SubContent2" Runat="Server">

<script type="text/javascript">
    $(document).ready(function() {
        $('#default').addClass("boot-active");
    });

    function get_shas() {
        $('#<%= consoleSha.ClientID %>').val(syslinux_sha512(document.getElementById('<%= txtDebugPwd.ClientID %>').value));
        $('#<%= addhostSha.ClientID %>').val(syslinux_sha512(document.getElementById('<%= txtAddPwd.ClientID %>').value));
        $('#<%= ondsha.ClientID %>').val(syslinux_sha512(document.getElementById('<%= txtOndPwd.ClientID %>').value));
        $('#<%= diagsha.ClientID %>').val(syslinux_sha512(document.getElementById('<%= txtdiagPwd.ClientID %>').value));
    }
</script>

<asp:HiddenField ID="consoleSha" runat="server"/>
<asp:HiddenField ID="addhostSha" runat="server"/>
<asp:HiddenField ID="ondsha" runat="server"/>
<asp:HiddenField ID="diagsha" runat="server"/>


<div id="divStandardMode" runat="server" visible="false">
    <div id="bootPasswords" runat="server" visible="false" style="margin-top: 0;">

        <div class="size-4 column">
            Kernel:
        </div>
        <div class="size-5 column">
            <asp:DropDownList ID="ddlHostKernel" runat="server" CssClass="ddlist">
            </asp:DropDownList>
        </div>
        <br class="clear"/>
        <div class="size-4 column">
            Boot Image:
        </div>
        <div class="size-5 column">
            <asp:DropDownList ID="ddlHostBootImage" runat="server" CssClass="ddlist">
            </asp:DropDownList>
        </div>
        <br class="clear"/>
        <div id="passboxes" runat="server">
            <br/>
            <h4>Boot Menu Passwords - Optional</h4>
            <div class="size-4 column">
                Client Console Password:
            </div>
            <div class="size-5 column">
                <asp:TextBox ID="txtDebugPwd" runat="server" CssClass="textbox" type="password"></asp:TextBox>
            </div>
            <br class="clear"/>
            <div class="size-4 column">
                Add Host Password:
            </div>
            <div class="size-5 column">
                <asp:TextBox ID="txtAddPwd" runat="server" CssClass="textbox" type="password"></asp:TextBox>
            </div>
            <br class="clear"/>
            <div class="size-4 column">
                On Demand Password:
            </div>
            <div class="size-5 column">
                <asp:TextBox ID="txtOndPwd" runat="server" CssClass="textbox" type="password"></asp:TextBox>
            </div>
            <br class="clear"/>
            <div class="size-4 column">
                Diagnostics Password:
            </div>
            <div class="size-5 column">
                <asp:TextBox ID="txtdiagPwd" runat="server" CssClass="textbox" type="password"></asp:TextBox>
            </div>
            <br class="clear"/>
        </div>
        <div id="grubPassBoxes" runat="server" visible="false" style="margin-top: 0;">
            <br/>
            <h4>Boot Menu Passwords - Optional</h4>
            <div class="size-4 column">
                Username:
            </div>
            <div class="size-5 column">
                <asp:TextBox ID="txtGrubUsername" runat="server" CssClass="textbox"></asp:TextBox>
            </div>
            <br class="clear"/>
            <div class="size-4 column">
                Password:
            </div>
            <div class="size-5 column">
                <asp:TextBox ID="txtGrubPassword" runat="server" CssClass="textbox" type="password"></asp:TextBox>
            </div>
            <br class="clear"/>
        </div>

    </div>

</div>

<div id="divProxyDHCP" runat="server" visible="false">
    <div class="size-4 column">
        <h4>BIOS</h4>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Kernel:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlBiosKernel" runat="server" CssClass="ddlist">
        </asp:DropDownList>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Boot Image:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlBiosBootImage" runat="server" CssClass="ddlist">
        </asp:DropDownList>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        <h4>EFI 32</h4>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Kernel:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlEfi32Kernel" runat="server" CssClass="ddlist">
        </asp:DropDownList>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Boot Image:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlEfi32BootImage" runat="server" CssClass="ddlist">
        </asp:DropDownList>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        <h4>EFI64</h4>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Kernel:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlEfi64Kernel" runat="server" CssClass="ddlist">
        </asp:DropDownList>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Boot Image:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlEfi64BootImage" runat="server" CssClass="ddlist">
        </asp:DropDownList>
    </div>
    <br class="clear"/>
    <br/>
    <div id="proxyPassBoxes" runat="server" visible="false" style="margin-top: 20px;">
        <div class="size-1 column">
            Boot Menu Password Will Be Applied To Any Matching
        </div>
        <br class="clear"/>
        <br/>
        <h4>Syslinux Boot Menu Passwords - Optional</h4>

        <div class="size-4 column">
            Client Console Password:
        </div>
        <div class="size-5 column">
            <asp:TextBox ID="txtProxDebugPwd" runat="server" CssClass="textbox" type="password"></asp:TextBox>
        </div>
        <br class="clear"/>
        <div class="size-4 column">
            Add Host Password:
        </div>
        <div class="size-5 column">
            <asp:TextBox ID="txtProxAddPwd" runat="server" CssClass="textbox" type="password"></asp:TextBox>
        </div>
        <br class="clear"/>
        <div class="size-4 column">
            On Demand Password:
        </div>
        <div class="size-5 column">
            <asp:TextBox ID="txtProxOndPwd" runat="server" CssClass="textbox" type="password"></asp:TextBox>
        </div>
        <br class="clear"/>
        <div class="size-4 column">
            Diagnostics Password:
        </div>
        <div class="size-5 column">
            <asp:TextBox ID="txtProxDiagPwd" runat="server" CssClass="textbox" type="password"></asp:TextBox>
        </div>
        <br class="clear"/>
    </div>
    <div id="grubProxyPasses" runat="server" visible="false" style="margin-top: 0;">
        <br/>
        <h4>Grub Boot Menu Passwords - Optional</h4>
        <div class="size-4 column">
            Username:
        </div>
        <div class="size-5 column">
            <asp:TextBox ID="txtGrubProxyUsername" runat="server" CssClass="textbox"></asp:TextBox>
        </div>
        <br class="clear"/>
        <div class="size-4 column">
            Password:
        </div>
        <div class="size-5 column">
            <asp:TextBox ID="txtGrubProxyPassword" runat="server" CssClass="textbox" type="password"></asp:TextBox>
        </div>
        <br class="clear"/>
    </div>
</div>
</asp:Content>

