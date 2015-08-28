<%@ Page Title="" Language="C#" MasterPageFile="~/views/images/profiles/profiles.master" AutoEventWireup="true" CodeFile="partition.aspx.cs" Inherits="views_images_profiles_partition" %>
<%@ MasterType VirtualPath="~/views/images/profiles/profiles.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
      <li><a href="<%= ResolveUrl("~/views/images/profiles/chooser.aspx") %>?imageid=<%= Master.Image.Id %>&profileid=<%= Master.ImageProfile.Id %>&cat=profiles"><%= Master.ImageProfile.Name %></a></li>
    <li>Partitions</li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
     <script type="text/javascript">
        $(document).ready(function() {
            $('#partition').addClass("nav-current");
        });
    </script>
   
     <div class="size-9 column">
        Create Original Partitions From MBR / GPT
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkDownOriginalMbr" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>
    
    <div class="size-9 column">
        Create Original Partitions Dynamically
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkDownOriginalDynamic" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>
    
     <div class="size-9 column">
        Force Dynamic Partition For Exact Hdd Match
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkDownForceDynamic" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>
    
 
    <br class="clear"/>
    <br/><br/>
    <h3 class="txt-left">Custom Partition Script</h3>
    <div class="full column">
        <pre id="editor" class="editor height_400"></pre>
     <asp:HiddenField ID="scriptEditorText" runat="server"/>

<script>

    var editor = ace.edit("editor");
    editor.session.setValue($('#<%= scriptEditorText.ClientID %>').val());

    editor.setTheme("ace/theme/idle_fingers");
    editor.getSession().setMode("ace/mode/sh");
    editor.setOption("showPrintMargin", false);
    editor.session.setUseWrapMode(true);
    editor.session.setWrapLimitRange(60, 60);


    function update_click() {
        var editor = ace.edit("editor");
        $('#<%= scriptEditorText.ClientID %>').val(editor.session.getValue());
    }

</script>
    <br class="clear"/>
       
    </div>
    <br class="clear"/>
      <div class="size-4 column">
        &nbsp;
    </div>
    <div class="size-5 column">
        <asp:LinkButton ID="btnUpdatePartitions" runat="server" OnClick="btnUpdatePartitions_OnClick" Text="Update Partition Options" CssClass="submits"/>
    </div>
</asp:Content>

