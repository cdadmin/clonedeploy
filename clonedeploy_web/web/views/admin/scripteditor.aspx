<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/Admin.master" AutoEventWireup="true" Inherits="views.admin.AdminSysprep" CodeFile="scripteditor.aspx.cs" ValidateRequest="false" %>

<%@ MasterType VirtualPath="~/views/admin/Admin.master" %>
<%@ Reference VirtualPath="~/views/masters/Site.master" %>
<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#scriptEditorSettings').addClass("nav-current");
        });
    </script>
    <asp:LinkButton ID="buttonShowCustom" runat="server" Text="Host Scripts" OnClick="buttonShowCustom_OnClick" CssClass="submits static-width-nomarg" ClientIDMode="Static"/>
    <asp:LinkButton ID="buttonShowCore" runat="server" Text="Core Scripts" OnClick="buttonShowCore_OnClick" CssClass="submits static-width-nomarg" ClientIDMode="Static"/>
    <div id="Custom" runat="server" visible="False">
        <script type="text/javascript">
            $(document).ready(function() {
                $('#buttonShowCustom').addClass("boot-active");
            });

        </script>
        <br class="clear"/>
      
        
            <asp:DropDownList runat="server" ID="ddlCustomScripts" OnSelectedIndexChanged="ddlCustomScripts_OnSelectedIndexChanged" AutoPostBack="True" CssClass="ddlist" Style="float: right; margin-right: 5px; margin-top: 5px; width: 200px;">
            </asp:DropDownList>
  
             
        <br class="clear"/>
         <asp:LinkButton ID="btnUpdateCustom" runat="server" Text="Update" OnClick="btnUpdateCustom_OnClick" CssClass="submits static-width-nomarg left" OnClientClick="update_click()"/>
             <asp:LinkButton ID="buttonDeleteScript" runat="server" Text="Delete" OnClick="buttonDeleteScript_OnClick" CssClass="submits static-width-nomarg left" />
             <asp:LinkButton ID="buttonCreateScript" runat="server" Text="Create" OnClick="buttonCreateScript_OnClick" CssClass="submits static-width-nomarg left" OnClientClick="update_click()"/>
        <div class="size-4 column">
        <asp:TextBox ID="txtNewScript" runat="server" CssClass="textbox new-template" ></asp:TextBox>
            </div>
    </div>

    <div id="Core" runat="server" visible="False">
        <script type="text/javascript">
            $(document).ready(function() {
                $('#buttonShowCore').addClass("boot-active");
            });

        </script>
        <br class="clear"/>
        
            <asp:DropDownList runat="server" ID="ddlCoreScripts" OnSelectedIndexChanged="ddlCoreScripts_OnSelectedIndexChanged" AutoPostBack="True" CssClass="ddlist" Style="float: right; margin-right: 5px; margin-top: 5px; width: 200px;">
            </asp:DropDownList>
        <br class="clear"/>
             <asp:LinkButton ID="buttonSaveCore" runat="server" Text="Update" OnClick="buttonSaveCore_OnClick" CssClass="submits left static-width" OnClientClick="update_click()"/>
        

    </div>
    <div id="aceEditor" runat="server" visible="False">
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
