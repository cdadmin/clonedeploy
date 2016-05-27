<%@ Page Title="" Language="C#" MasterPageFile="~/views/global/scripts/scripts.master" AutoEventWireup="true" CodeFile="editcore.aspx.cs" Inherits="views_admin_scripts_editcore" ValidateRequest="False" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
     <li ><a href="<%= ResolveUrl("~/views/global/scripts/search.aspx?cat=sub1") %>">Scripts</a></li>
    <li>Edit Core Scripts</li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SubHelp" Runat="Server">
    <a href="<%= ResolveUrl("~/views/help/index.html") %>"   target="_blank">Help</a>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ActionsRightSub" Runat="Server">
      <asp:LinkButton ID="buttonSaveCore" runat="server" Text="Update" OnClick="buttonSaveCore_OnClick"  OnClientClick="update_click()"/>
</asp:Content>

<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="SubContent2">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#edit').addClass("nav-current");
        });
    </script>
     <asp:DropDownList runat="server" ID="ddlCoreScripts" OnSelectedIndexChanged="ddlCoreScripts_OnSelectedIndexChanged" AutoPostBack="True" CssClass="ddlist" Style="float: right; margin-right: 5px; margin-top: 5px; width: 200px;">
            </asp:DropDownList>
        <br class="clear"/>
           
        

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

