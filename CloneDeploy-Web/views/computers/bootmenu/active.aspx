<%@ Page Title="" Language="C#" MasterPageFile="~/views/computers/bootmenu/bootmenu.master" AutoEventWireup="true" Inherits="views_computers_bootmenu_active" Codebehind="active.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
    <li>Active</li>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="SubHelp">
   <li role="separator" class="divider"></li>
    <li><a href="<%= ResolveUrl("~/views/help/computers-bootmenu.aspx")%>" class="" target="_blank">Help</a></li>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ActionsRightSub">

    <button type="button" class="btn btn-default dropdown-toggle width_140" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" >
    <span class="caret"></span>
  </button>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
    <script type="text/javascript">     
        $(document).ready(function() {      
            $('#active').addClass("nav-current");
            $('#bootmenu').addClass("nav-current");
        });     
    </script>
     <div id="divProxy" runat="server" visible="false">
            <div class="size-6 column">
                Select A Menu:
            </div>
            <br class="clear"/>
            <div class="size-4 column">
                <asp:DropDownList ID="ddlProxyMode" runat="server" CssClass="ddlist" OnSelectedIndexChanged="EditProxy_Changed" AutoPostBack="true">
                    <asp:ListItem>bios</asp:ListItem>
                    <asp:ListItem>efi32</asp:ListItem>
                    <asp:ListItem>efi64</asp:ListItem>
                </asp:DropDownList>
            </div>
            <br class="clear"/>
        </div>
        <asp:Label ID="lblActiveBoot" runat="server"></asp:Label> 
        <asp:Label ID="lblFileName" runat="server"></asp:Label>
    
     <div id="aceEditor" runat="server" >
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

