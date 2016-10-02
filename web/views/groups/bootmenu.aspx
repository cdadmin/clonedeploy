<%@ Page Title="" Language="C#" MasterPageFile="~/views/groups/groups.master" AutoEventWireup="true" CodeFile="bootmenu.aspx.cs" Inherits="views.groups.GroupBootMenu" %>

<asp:Content ID="Breadcrumb" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
   <li><a href="<%= ResolveUrl("~/views/groups/edit.aspx") %>?groupid=<%= Group.Id %>" ><%= Group.Name %></a></li>
    <li>Boot Menu</li>
</asp:Content>


    <asp:Content runat="server" ContentPlaceHolderID="Help">
        <li role="separator" class="divider"></li>
     <li><a href="<%= ResolveUrl("~/views/help/groups-bootmenu.aspx")%>" target="_blank">Help</a></li>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="SubPageActionsRight">
  <asp:LinkButton ID="buttonUpdate" runat="server" OnClick="buttonUpdate_OnClick" Text="Update Menu" OnClientClick="update_click()" CssClass="btn btn-default"/>
     <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
    <span class="caret"></span>
  </button>
</asp:Content>
 
<asp:Content ID="Content1" ContentPlaceHolderID="SubContent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#bootmenu').addClass("nav-current");
        });
    </script>
    
    <div class="size-4 column">
        Set As Default For New Group Members:
    </div>
    <div class="size-5 column">
        <asp:CheckBox ID="chkDefault" runat="server" OnCheckedChanged="chkDefault_OnCheckedChanged" AutoPostBack="True"></asp:CheckBox>
      
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