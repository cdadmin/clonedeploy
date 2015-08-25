<%@ Page Title="" Language="C#" MasterPageFile="~/views/masters/Image.master" AutoEventWireup="true" CodeFile="create.aspx.cs" Inherits="views_images_profiles_create" %>

<%@ MasterType VirtualPath="~/views/masters/Image.master" %>
<%@ Reference virtualPath="~/views/masters/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="SubContent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#optionsoption').addClass("nav-current");
        });
    </script>
     <div class="size-4 column">
        Profile Name:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtProfileName" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>
     <div class="size-4 column">
        Profile Description:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtProfileDesc" runat="server" CssClass="descbox" TextMode="MultiLine"></asp:TextBox>
    </div>
    <br class="clear"/>
    <h3 class="txt-left">Global Options</h3>
    <div class="size-4 column">
        Kernel:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlKernel" runat="server" CssClass="ddlist">
        </asp:DropDownList>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Boot Image:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlBootImage" runat="server" CssClass="ddlist">
        </asp:DropDownList>
    </div>
    <br class="clear"/>
     <div class="size-9 column">
        Don't Download Core Scripts
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkGlobalNoCore" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>
    
    <div class="size-9 column">
        Don't Set Hardware Clock
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkGlobalNoClock" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>
    
    <div class="size-9 column">
        Power Off After Task
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkGlobalPoweroff" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>
    
    <div class="size-9 column">
        Return To Console After Task
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkGlobalReturnConsole" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>
    <br/><br/>
    <h3 class="txt-left">Upload Options</h3>
    <div class="size-9 column">
        Don't Shrink Volumes
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkUpNoShrink" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>
    
    <div class="size-9 column">
        Don't Shrink LVM Volumes
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkUpNoShrinkLVM" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>
    
    <div class="size-9 column">
        Calculate Size Debug
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkUpDebugResize" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>

    <br/><br/>
    <h3 class="txt-left">Deploy Options</h3>
    <div class="size-9 column">
        Don't Expand Volume
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkDownNoExpand" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>
    
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
        Expand Partitions Without Resizable Volumes
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkDownExpandNonResizable" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>
    
     <div class="size-9 column">
        Force Dynamic Partition For Exact Hdd Match
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkDownForceDynamic" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>
    
    <div class="size-9 column">
        Align BCD To Partition
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkAlignBCD" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>
    
     <div class="size-9 column">
        Run Fix Boot
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkRunFixBoot" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>
    <br/><br/>
    <h3 class="txt-left">Custom Partition Script</h3>
    <div class="size-1 column">
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
        <asp:LinkButton ID="buttonUpdateHost" runat="server" OnClick="buttonCreateProfile_Click" Text="Update Host" CssClass="submits" OnClientClick="update_click()"/>
    </div>
</asp:Content>
