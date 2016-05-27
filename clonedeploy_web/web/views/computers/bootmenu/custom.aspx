<%@ Page Title="" Language="C#" MasterPageFile="~/views/computers/bootmenu/bootmenu.master" AutoEventWireup="true" CodeFile="custom.aspx.cs" Inherits="views_computers_bootmenu_custom" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
    <li>Custom</li>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="SubHelp">
       <a href="<%= ResolveUrl("~/views/help/index.html")%>" target="_blank">Help</a>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ActionsRightSub">
  <asp:LinkButton ID="buttonUpdate" runat="server" OnClick="buttonUpdate_OnClick" Text="Update Menu"  OnClientClick="update_click()"/>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
     <script type="text/javascript">     
        $(document).ready(function() {      
            $('#custom').addClass("nav-current");
            $('#bootmenu').addClass("nav-current");
        });     
    </script>

    <div class="size-4 column">
        Enable Custom Boot Menus:
    </div>
    <div class="size-5 column">
        <asp:CheckBox ID="chkEnabled" runat="server" OnCheckedChanged="chkEnabled_OnCheckedChanged" AutoPostBack="True"></asp:CheckBox>
      
    </div>
   
    <br class="clear"/>
     <br />
      <div id="divProxy" runat="server" visible="false">
            <div class="size-4 column">
                Select A Menu To Edit:
            </div>

            <div class="size-5 column">
                <asp:DropDownList ID="ddlProxyMode" runat="server" CssClass="ddlist" OnSelectedIndexChanged="ddlProxyMode_OnSelectedIndexChanged" AutoPostBack="True">
                    <asp:ListItem>bios</asp:ListItem>
                    <asp:ListItem>efi32</asp:ListItem>
                    <asp:ListItem>efi64</asp:ListItem>
                </asp:DropDownList>
            </div>
            <br class="clear"/>
        </div>
      <div class="size-4 column">
        Start From Template:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlTemplates" runat="server" CssClass="ddlist" OnSelectedIndexChanged="ddlTemplates_OnSelectedIndexChanged" AutoPostBack="True"/>
      
    </div>
    <br class="clear"/>
    
     <div class="size-4 column">
        Boot Menu Contents:
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

