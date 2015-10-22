<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/bootmenu/bootmenu.master" AutoEventWireup="true" CodeFile="editor.aspx.cs" Inherits="views_admin_bootmenu_editor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubHelp" Runat="Server">
    <a href="<%= ResolveUrl("~/views/help/index.html") %>" class="icon help" data-info="Help" target="_blank"></a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ActionsRightSub" Runat="Server">
    <asp:LinkButton ID="btnSaveEditor" runat="server" Text="Save Changes" OnClick="saveEditor_Click" CssClass="submits" Style="margin: 0;" OnClientClick="update_click()"/>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="SubContent2" Runat="Server">
        <script type="text/javascript">
            $(document).ready(function() {
                $('#edit').addClass("boot-active");
            });

            function generate_sha() {
                $('#<%= txtAfterSha.ClientID %>').val(syslinux_sha512(document.getElementById('<%= txtBeforeSha.ClientID %>').value));
            }
        </script>
        <div id="proxyEditor" runat="server" visible="false">
            <div class="size-4 column">
                Select A Menu To Edit:
            </div>
            <div class="size-4 column">
                <asp:DropDownList ID="ddlEditProxyType" runat="server" CssClass="ddlist" OnSelectedIndexChanged="EditProxy_Changed" AutoPostBack="true">
                    <asp:ListItem>bios</asp:ListItem>
                    <asp:ListItem>efi32</asp:ListItem>
                    <asp:ListItem>efi64</asp:ListItem>
                </asp:DropDownList>
            </div>
            <br class="clear"/>
        </div>
        <div id="syslinuxShaGen" runat="server" visible="false">
            <h4>Syslinux Sha Generator</h4>
            <div class="size-1 column">
                <asp:LinkButton ID="btnGenerateSha" runat="server" Text="Generate" OnClientClick="generate_sha();" CssClass="submits" Style="float: left; margin: 0 15px 0 0;"/>
                <asp:TextBox ID="txtBeforeSha" runat="server" CssClass="textbox txt-generate" Style="width: 200px;"></asp:TextBox>
            </div>
            <br class="clear"/>
            <div class="size-1 column">
                <asp:TextBox ID="txtAfterSha" runat="server" CssClass="txtSha" Style="font-size: 12px; width: 100%" TextMode="MultiLine"></asp:TextBox>
            </div>
            <br class="clear"/>
        </div>
        <div id="grubShaGen" runat="server" visible="false">
            <h4>Grub Sha Generator</h4>
            <div class="size-1 column">
                <asp:LinkButton ID="btnGrubGen" runat="server" Text="Generate" CssClass="submits" Style="float: left; margin: 0 15px 0 0;" OnClick="btnGrubGen_Click"/>
                <asp:TextBox ID="txtGrubPass" runat="server" CssClass="textbox txt-generate" Style="width: 200px;"></asp:TextBox>
            </div>
            <br class="clear"/>
            <div class="size-1 column">
                <asp:TextBox ID="txtGrubSha" runat="server" CssClass="txtSha" Style="font-size: 12px; width: 100%" TextMode="MultiLine"></asp:TextBox>
            </div>
            <br class="clear"/>
        </div>
       
        <div class="full column">
            <asp:Label ID="lblFileName1" runat="server"></asp:Label>
        </div>
        <br class="clear"/>
        <pre id="editor" class="editor height_800"></pre>
        <asp:HiddenField ID="scriptEditorText" runat="server"/>


        <script>

            var editor = ace.edit("editor");
            editor.session.setValue($('#<%= scriptEditorText.ClientID %>').val());

            editor.setTheme("ace/theme/idle_fingers");
            editor.getSession().setMode("ace/mode/sh");
            editor.setOption("showPrintMargin", false);
            editor.session.setUseWrapMode(true);
            editor.session.setWrapLimitRange(120, 120);


            function update_click() {
                var editor = ace.edit("editor");
                $('#<%= scriptEditorText.ClientID %>').val(editor.session.getValue());
            }

        </script>
    
</asp:Content>

