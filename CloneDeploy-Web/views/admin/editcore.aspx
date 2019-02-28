<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/admin.master" AutoEventWireup="true" Inherits="CloneDeploy_Web.views.global.scripts.views_admin_scripts_editcore"  Codebehind="editcore.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
  
    <li>Edit Core Scripts</li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Help" Runat="Server">
    <li role="separator" class="divider"></li>
    <li>
        <a href="<%= ResolveUrl("~/views/help/admin-core.aspx") %>" target="_blank">Help</a>
    </li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="SubPageActionsRight" Runat="Server">
    <asp:LinkButton ID="buttonSaveCore" runat="server" Text="Update" OnClick="buttonSaveCore_OnClick" OnClientClick="update_click()" CssClass="btn btn-default width_100"/>
    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        <span class="caret"></span>
    </button>
</asp:Content>

<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="SubContent">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#editcore').addClass("nav-current");
        });
    </script>
    <asp:DropDownList runat="server" ID="ddlCoreScripts" OnSelectedIndexChanged="ddlCoreScripts_OnSelectedIndexChanged" AutoPostBack="True" CssClass="ddlist" Style="float: right; margin-right: 5px; margin-top: 20px; width: 200px;">
    </asp:DropDownList>
    <br class="clear"/>
    <br/>

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