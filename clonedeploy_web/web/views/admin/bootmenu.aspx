<%@ Page Title="" Language="C#" MasterPageFile="~/views/masters/Admin.master" AutoEventWireup="true" CodeFile="bootmenu.aspx.cs" Inherits="views.admin.AdminBootMenu" %>

<%@ MasterType VirtualPath="~/views/masters/Admin.master" %>
<%@ Reference VirtualPath="~/views/masters/Site.master" %>
<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#bootMenuSettings').addClass("nav-current");
        });
    </script>
    <asp:LinkButton ID="showTemplates" runat="server" CssClass="submits static-width " Text="Templates" OnClick="showTemplates_Click" ClientIDMode="Static"></asp:LinkButton>
    <asp:LinkButton ID="showEditor" runat="server" CssClass="submits static-width" Text="Editor" OnClick="showEditor_Click" ClientIDMode="Static"></asp:LinkButton>
    <br class="clear" />
    <div id="bootEditor" runat="server" visible="false">
        <script type="text/javascript">
            $(document).ready(function () {
                $('#showEditor').addClass("boot-active");
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
            <br class="clear" />
        </div>
        <div id="syslinuxShaGen" runat="server" visible="false">
            <h4>Syslinux Sha Generator</h4>
            <div class="size-1 column">
                <asp:LinkButton ID="btnGenerateSha" runat="server" Text="Generate" OnClientClick="generate_sha();" CssClass="submits" Style="float: left; margin: 0 15px 0 0;" />
                <asp:TextBox ID="txtBeforeSha" runat="server" CssClass="textbox txt-generate" Style="width: 200px;"></asp:TextBox>
            </div>
            <br class="clear" />
            <div class="size-1 column">
                <asp:TextBox ID="txtAfterSha" runat="server" CssClass="txtSha" Style="font-size: 12px; width: 100%" TextMode="MultiLine"></asp:TextBox>
            </div>
            <br class="clear" />
        </div>
        <div id="grubShaGen" runat="server" visible="false">
            <h4>Grub Sha Generator</h4>
            <div class="size-1 column">
                <asp:LinkButton ID="btnGrubGen" runat="server" Text="Generate" CssClass="submits" Style="float: left; margin: 0 15px 0 0;" OnClick="btnGrubGen_Click" />
                <asp:TextBox ID="txtGrubPass" runat="server" CssClass="textbox txt-generate" Style="width: 200px;"></asp:TextBox>
            </div>
            <br class="clear" />
            <div class="size-1 column">
                <asp:TextBox ID="txtGrubSha" runat="server" CssClass="txtSha" Style="font-size: 12px; width: 100%" TextMode="MultiLine"></asp:TextBox>
            </div>
            <br class="clear" />
        </div>
        <asp:LinkButton ID="btnSaveEditor" runat="server" Text="Save Changes" OnClick="saveEditor_Click" CssClass="submits" Style="margin: 0;" OnClientClick="update_click()" />
        <div class="full column">
            <asp:Label ID="lblFileName1" runat="server"></asp:Label>
        </div>
        <br class="clear" />
        <pre id="editor" class="editor height_800"></pre>
        <asp:HiddenField ID="scriptEditorText" runat="server" />

       
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

    </div>
    <div id="bootTemplates" runat="server" visible="false">
        <script type="text/javascript">
            $(document).ready(function () {
                $('#showTemplates').addClass("boot-active");
            });

        </script>
        <asp:DropDownList ID="ddlTemplate" runat="server" CssClass="ddlist" Style="float: right; margin-right: 5px; margin-top: 5px; width: 200px;" OnSelectedIndexChanged="ddlTemplate_SelectedIndexChanged" AutoPostBack="true">
        </asp:DropDownList>
        <br class="clear" />
        <div id="createNewTemplate" runat="server" visible="false">
            <div class="size-4 column">
                <asp:TextBox ID="txtNewTemplate" runat="server" CssClass="textbox new-template" Style="margin-left: -10px"></asp:TextBox>
            </div>
            <div class="size-4 column">
                <asp:LinkButton ID="btnNewTemplate" runat="server" Text="Create Template" OnClick="btnNewTemplate_Click" CssClass="submits" Style="float: left; margin: 0;" OnClientClick="update_click_new()" />
            </div>
            <br class="clear" />
            
             <pre id="editor1" class="editor height_800"></pre>
        <asp:HiddenField ID="scriptEditorNewTemplate" runat="server" />

       
        <script>

            var editor = ace.edit("editor1");
            editor.session.setValue($('#<%= scriptEditorNewTemplate.ClientID %>').val());

            editor.setTheme("ace/theme/idle_fingers");
            editor.getSession().setMode("ace/mode/sh");
            editor.setOption("showPrintMargin", false);
            editor.session.setUseWrapMode(true);
            editor.session.setWrapLimitRange(120, 120);


            function update_click_new() {
                var editor = ace.edit("editor1");
                $('#<%= scriptEditorNewTemplate.ClientID %>').val(editor.session.getValue());
    }

        </script>

        </div>
        <div id="modifyTemplate" runat="server" visible="false">
            <br class="clear" />
            <div class="size-4 column">
                <asp:TextBox ID="txtModifyTemplate" runat="server" CssClass="textbox new-template" Style="margin-left: -10px"></asp:TextBox>
            </div>
            <div class="size-4 column">
                <asp:LinkButton ID="delTemplate" runat="server" Text="Delete Template" OnClick="btnDeleteTemplate_Click" CssClass="submits" Style="float: left; margin: 0;" />
            </div>
            <div class="size-4 column">
                <asp:LinkButton ID="updateTemplate" runat="server" Text="Update Template" OnClick="btnUpdateTemplate_Click" CssClass="submits" Style="float: left; margin: 0;" OnClientClick="update_click_modify()" />
            </div>
               <pre id="editor2" class="editor height_800"></pre>
        <asp:HiddenField ID="scriptEditorModify" runat="server" />

       
        <script>

            var editor = ace.edit("editor2");
            editor.session.setValue($('#<%= scriptEditorModify.ClientID %>').val());

            editor.setTheme("ace/theme/idle_fingers");
            editor.getSession().setMode("ace/mode/sh");
            editor.setOption("showPrintMargin", false);
            editor.session.setUseWrapMode(true);
            editor.session.setWrapLimitRange(120, 120);


            function update_click_modify() {
                var editor = ace.edit("editor2");
                $('#<%= scriptEditorModify.ClientID %>').val(editor.session.getValue());
            }

        </script>
        </div>
        <br class="clear" />

        <div id="divPXEMode" runat="server" visible="false">
            <div id="bootPasswords" runat="server" visible="false" style="margin-top: 0;">
                <asp:HiddenField ID="consoleSha" runat="server" />
                <asp:HiddenField ID="addhostSha" runat="server" />
                <asp:HiddenField ID="ondsha" runat="server" />
                <asp:HiddenField ID="diagsha" runat="server" />
                <script type="text/javascript">
                    function get_shas() {
                        $('#<%= consoleSha.ClientID %>').val(syslinux_sha512(document.getElementById('<%= txtDebugPwd.ClientID %>').value));
                $('#<%= addhostSha.ClientID %>').val(syslinux_sha512(document.getElementById('<%= txtAddPwd.ClientID %>').value));
                $('#<%= ondsha.ClientID %>').val(syslinux_sha512(document.getElementById('<%= txtOndPwd.ClientID %>').value));
                $('#<%= diagsha.ClientID %>').val(syslinux_sha512(document.getElementById('<%= txtdiagPwd.ClientID %>').value));
            }
                </script>
                <div class="size-4 column">
                    Kernel:
                </div>
                <div class="size-5 column">
                    <asp:DropDownList ID="ddlHostKernel" runat="server" CssClass="ddlist">
                    </asp:DropDownList>
                </div>
                <br class="clear" />
                <div class="size-4 column">
                    Boot Image:
                </div>
                <div class="size-5 column">
                    <asp:DropDownList ID="ddlHostBootImage" runat="server" CssClass="ddlist">
                    </asp:DropDownList>
                </div>
                <br class="clear" />
                <div id="passboxes" runat="server">
                    <br />
                    <h4>Boot Menu Passwords - Optional</h4>
                    <div class="size-4 column">
                        Client Console Password:
                    </div>
                    <div class="size-5 column">
                        <asp:TextBox ID="txtDebugPwd" runat="server" CssClass="textbox" type="password"></asp:TextBox>
                    </div>
                    <br class="clear" />
                    <div class="size-4 column">
                        Add Host Password:
                    </div>
                    <div class="size-5 column">
                        <asp:TextBox ID="txtAddPwd" runat="server" CssClass="textbox" type="password"></asp:TextBox>
                    </div>
                    <br class="clear" />
                    <div class="size-4 column">
                        On Demand Password:
                    </div>
                    <div class="size-5 column">
                        <asp:TextBox ID="txtOndPwd" runat="server" CssClass="textbox" type="password"></asp:TextBox>
                    </div>
                    <br class="clear" />
                    <div class="size-4 column">
                        Diagnostics Password:
                    </div>
                    <div class="size-5 column">
                        <asp:TextBox ID="txtdiagPwd" runat="server" CssClass="textbox" type="password"></asp:TextBox>
                    </div>
                    <br class="clear" />
                </div>
                <div id="grubPassBoxes" runat="server" visible="false" style="margin-top: 0;">
                    <br />
                    <h4>Boot Menu Passwords - Optional</h4>
                    <div class="size-4 column">
                        Username:
                    </div>
                    <div class="size-5 column">
                        <asp:TextBox ID="txtGrubUsername" runat="server" CssClass="textbox"></asp:TextBox>
                    </div>
                    <br class="clear" />
                    <div class="size-4 column">
                        Password:
                    </div>
                    <div class="size-5 column">
                        <asp:TextBox ID="txtGrubPassword" runat="server" CssClass="textbox" type="password"></asp:TextBox>
                    </div>
                    <br class="clear" />
                </div>
                <div class="size-4 column">
                    &nbsp;
                </div>
                <div class="size-5 column">
                    <asp:LinkButton ID="btnSubmitDefault" runat="server" Text="Create Boot File" OnClick="btnSubmitDefault_Click" CssClass="submits" OnClientClick="get_shas();" />

                </div>
                <br class="clear" />
            </div>

        </div>

        <div id="divProxyDHCP" runat="server" visible="false">
            <div class="size-4 column">
                <h4>BIOS</h4>
            </div>
            <br class="clear" />
            <div class="size-4 column">
                Kernel:
            </div>
            <div class="size-5 column">
                <asp:DropDownList ID="ddlBiosKernel" runat="server" CssClass="ddlist">
                </asp:DropDownList>
            </div>
            <br class="clear" />
            <div class="size-4 column">
                Boot Image:
            </div>
            <div class="size-5 column">
                <asp:DropDownList ID="ddlBiosBootImage" runat="server" CssClass="ddlist">
                </asp:DropDownList>
            </div>
            <br class="clear" />
            <div class="size-4 column">
                <h4>EFI 32</h4>
            </div>
            <br class="clear" />
            <div class="size-4 column">
                Kernel:
            </div>
            <div class="size-5 column">
                <asp:DropDownList ID="ddlEfi32Kernel" runat="server" CssClass="ddlist">
                </asp:DropDownList>
            </div>
            <br class="clear" />
            <div class="size-4 column">
                Boot Image:
            </div>
            <div class="size-5 column">
                <asp:DropDownList ID="ddlEfi32BootImage" runat="server" CssClass="ddlist">
                </asp:DropDownList>
            </div>
            <br class="clear" />
            <div class="size-4 column">
                <h4>EFI64</h4>
            </div>
            <br class="clear" />
            <div class="size-4 column">
                Kernel:
            </div>
            <div class="size-5 column">
                <asp:DropDownList ID="ddlEfi64Kernel" runat="server" CssClass="ddlist">
                </asp:DropDownList>
            </div>
            <br class="clear" />
            <div class="size-4 column">
                Boot Image:
            </div>
            <div class="size-5 column">
                <asp:DropDownList ID="ddlEfi64BootImage" runat="server" CssClass="ddlist">
                </asp:DropDownList>
            </div>
            <br class="clear" />
            <br />
            <div id="proxyPassBoxes" runat="server" visible="false" style="margin-top: 20px;">
                <div class="size-1 column">
                    Boot Menu Password Will Be Applied To Any Matching
                </div>
                <br class="clear" />
                <br />
                <h4>Syslinux Boot Menu Passwords - Optional</h4>
                <asp:HiddenField ID="proxconsoleSha" runat="server" />
                <asp:HiddenField ID="proxaddhostSha" runat="server" />
                <asp:HiddenField ID="proxondsha" runat="server" />
                <asp:HiddenField ID="proxdiagsha" runat="server" />
                <script type="text/javascript">
                    function get_shasProxy() {
                        $('#<%= proxconsoleSha.ClientID %>').val(syslinux_sha512(document.getElementById('<%= txtProxDebugPwd.ClientID %>').value));
                $('#<%= proxaddhostSha.ClientID %>').val(syslinux_sha512(document.getElementById('<%= txtProxAddPwd.ClientID %>').value));
                $('#<%= proxondsha.ClientID %>').val(syslinux_sha512(document.getElementById('<%= txtProxOndPwd.ClientID %>').value));
                $('#<%= proxdiagsha.ClientID %>').val(syslinux_sha512(document.getElementById('<%= txtProxDiagPwd.ClientID %>').value));
            }
                </script>
                <div class="size-4 column">
                    Client Console Password:
                </div>
                <div class="size-5 column">
                    <asp:TextBox ID="txtProxDebugPwd" runat="server" CssClass="textbox" type="password"></asp:TextBox>
                </div>
                <br class="clear" />
                <div class="size-4 column">
                    Add Host Password:
                </div>
                <div class="size-5 column">
                    <asp:TextBox ID="txtProxAddPwd" runat="server" CssClass="textbox" type="password"></asp:TextBox>
                </div>
                <br class="clear" />
                <div class="size-4 column">
                    On Demand Password:
                </div>
                <div class="size-5 column">
                    <asp:TextBox ID="txtProxOndPwd" runat="server" CssClass="textbox" type="password"></asp:TextBox>
                </div>
                <br class="clear" />
                <div class="size-4 column">
                    Diagnostics Password:
                </div>
                <div class="size-5 column">
                    <asp:TextBox ID="txtProxDiagPwd" runat="server" CssClass="textbox" type="password"></asp:TextBox>
                </div>
                <br class="clear" />
            </div>
            <div id="grubProxyPasses" runat="server" visible="false" style="margin-top: 0;">
                <br />
                <h4>Grub Boot Menu Passwords - Optional</h4>
                <div class="size-4 column">
                    Username:
                </div>
                <div class="size-5 column">
                    <asp:TextBox ID="txtGrubProxyUsername" runat="server" CssClass="textbox"></asp:TextBox>
                </div>
                <br class="clear" />
                <div class="size-4 column">
                    Password:
                </div>
                <div class="size-5 column">
                    <asp:TextBox ID="txtGrubProxyPassword" runat="server" CssClass="textbox" type="password"></asp:TextBox>
                </div>
                <br class="clear" />
            </div>
            <div class="size-4 column">
                &nbsp;
            </div>
            <div class="size-5 column">
                <asp:LinkButton ID="bntProxSubmit" runat="server" Text="Create Boot File" OnClick="btnProxSubmitDefault_Click" CssClass="submits" OnClientClick="get_shasProxy();" />

            </div>
            <br class="clear" />
        </div>
    </div>
</asp:Content>
