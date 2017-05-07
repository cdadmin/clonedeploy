<%@ Page Title="" Language="C#" MasterPageFile="~/views/global/scripts/scripts.master" AutoEventWireup="true" Inherits="views_admin_scripts_create" ValidateRequest="False" Codebehind="create.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
    <li>New Script</li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SubHelp" Runat="Server">
    <li role="separator" class="divider"></li>
    <li>
        <a href="<%= ResolveUrl("~/views/help/global-scripts.aspx") %>" target="_blank">Help</a>
    </li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ActionsRightSub" Runat="Server">
    <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_OnClick" Text="Add Script" OnClientClick="update_click()" CssClass="btn btn-default width_100"/>
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
        Script Name:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtScriptName" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>

    <div class="size-4 column">
        Script Description:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtScriptDesc" runat="server" TextMode="MultiLine" CssClass="descbox"></asp:TextBox>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Script Contents:
    </div>
    <div id="aceEditor" runat="server">
        <br class="clear"/>
        <pre id="editor" class="editor height_1200"></pre>
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