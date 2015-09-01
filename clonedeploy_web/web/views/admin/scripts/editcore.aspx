<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/scripts/scripts.master" AutoEventWireup="true" CodeFile="editcore.aspx.cs" Inherits="views_admin_scripts_editcore" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
     <li ><a href="<%= ResolveUrl("~/views/admin/scripts/search.aspx?cat=sub1") %>">Scripts</a></li>
    <li>Edit Core Scripts</li>
</asp:Content>

<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="SubContent2">
     <asp:DropDownList runat="server" ID="ddlCoreScripts" OnSelectedIndexChanged="ddlCoreScripts_OnSelectedIndexChanged" AutoPostBack="True" CssClass="ddlist" Style="float: right; margin-right: 5px; margin-top: 5px; width: 200px;">
            </asp:DropDownList>
        <br class="clear"/>
             <asp:LinkButton ID="buttonSaveCore" runat="server" Text="Update" OnClick="buttonSaveCore_OnClick" CssClass="submits left static-width" OnClientClick="update_click()"/>
        

    <div id="aceEditor" runat="server">
        <br class="clear"/>
        <pre id="editor" class="editor height_600"></pre>
        <asp:HiddenField ID="scriptEditor" runat="server"/>


        <script>

            var editor = ace.edit("editor");
            editor.session.setValue($('#<%= scriptEditor.ClientID %>').val());

            editor.setTheme("ace/theme/idle_fingers");
            editor.getSession().setMode("ace/mode/sh");
            editor.setOption("showPrintMargin", false);
            editor.session.setUseWrapMode(true);
            editor.session.setWrapLimitRange(120, 120);


            function update_click() {
                var editor = ace.edit("editor");
                $('#<%= scriptEditor.ClientID %>').val(editor.session.getValue());
            }

        </script>
    </div>
</asp:Content>

