<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/admin.master" AutoEventWireup="true" CodeBehind="profiletemplate.aspx.cs" Inherits="CloneDeploy_Web.views.admin.profiletemplate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li>Image Profile Templates</li>
</asp:Content>

<asp:Content runat="server" ID="Help" ContentPlaceHolderID="Help">
    <li role="separator" class="divider"></li>
    <li>
        <a href="<%= ResolveUrl("~/views/help/admin-email.aspx") %>" target="_blank">Help</a>
    </li>
</asp:Content>

<asp:Content runat="server" ID="ActionsRight" ContentPlaceHolderID="SubPageActionsRight">
    <asp:LinkButton ID="btnUpdateSettings" runat="server" Text="Update Template" OnClick="btnUpdateSettings_OnClick" CssClass="btn btn-default"/>
    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        <span class="caret"></span>
    </button>

</asp:Content>

<asp:Content runat="server" ID="additional" ContentPlaceHolderID="AdditionalActions">
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="SubContent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#profileTemplate').addClass("nav-current");
        });
    </script>
    <div class="size-4 column">
        Image Type:
    </div>
    <div class="size-setting column">
        <asp:DropDownList ID="ddlImageType" runat="server" CssClass="ddlist" AutoPostBack="True" OnSelectedIndexChanged="ddlImageType_OnSelectedIndexChanged">
           
        </asp:DropDownList>
    </div>
    <br class="clear"/>
    <br/>
  
    <h2>General</h2>
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
    <br/>
  
    
    <div id="LinuxAll1" runat="server">
    <h2>PXE Boot Options</h2>
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
    <div class="size-4 column">
        Kernel Arguments:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtKernelArgs" runat="server" CssClass="textbox">
        </asp:TextBox>
    </div>
    <br class="clear"/>
    <br/>
    </div>
    

    <h2>Task Options</h2>
      <div class="size-9 column">
        Web Cancelable
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkWebCancel" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>

    <br class="clear"/>

    <div class="size-9 column">
        Task Completed Action
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlTaskComplete" runat="server" CssClass="ddlist">
            <asp:ListItem>Reboot</asp:ListItem>
            <asp:ListItem>Power Off</asp:ListItem>
            <asp:ListItem>Exit To Shell</asp:ListItem>
        </asp:DropDownList>
    </div>
   
     <br class="clear"/>
    
    <div id="mac1" runat="server"> 
    <div class="size-9 column">
        OS X Target Volume
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtTargetVolume" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    </div>

    <br class="clear"/>
    <br/>
  
    

    <h2>Deploy Options</h2>
    <div class="size-9 column">
    Change Computer Name
</div>
<div class="size-8 column">
    <asp:CheckBox ID="chkChangeName" runat="server" CssClass="textbox"></asp:CheckBox>
</div>
<br class="clear"/>

    
    <div id="LinuxBlock1" runat="server">
    <div class="size-9 column">
        Don't Expand Volumes
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkDownNoExpand" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    </div>

    <br class="clear"/>


    <div id="LinuxAll2" runat="server">
    <div class="size-9 column">
        Update BCD
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkAlignBCD" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>
    
     <div class="size-9 column">
        Randomize GUIDs
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkRandomize" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>

    <div class="size-9 column">
        Fix Boot Sector
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkRunFixBoot" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>
       <div class="size-9 column">
       Don't Update NVRAM
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkNvram" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>
    </div>
   

   
    <div id="mac2" runat="server">
    <div class="size-9 column">
        Erase Partitions (Standard Auto Only)
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkErase" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>
    </div>
   
    
    <div id="LinuxAll3" runat="server">
    <div class="size-9 column">
        Create Partitions Method
    </div>
    <div class="size-5 column">
        <asp:dropdownlist ID="ddlPartitionMethodLin" runat="server" CssClass="ddlist"  AutoPostBack="True">
            <asp:ListItem>Use Original MBR / GPT</asp:ListItem>
            <asp:ListItem>Standard</asp:ListItem>
            <asp:ListItem>Dynamic</asp:ListItem>
            <asp:ListItem>Custom Script</asp:ListItem>
        </asp:dropdownlist>
    </div>
    <br class="clear"/>
    </div>

    
    <div id="winpe1" runat="server">
    <div class="size-9 column">
        Create Partitions Method
    </div>
    <div class="size-5 column">
        <asp:dropdownlist ID="ddlPartitionMethodWin" runat="server" CssClass="ddlist"  AutoPostBack="True">
            <asp:ListItem>Standard</asp:ListItem>
            <asp:ListItem>Dynamic</asp:ListItem>
            <asp:ListItem>Custom Script</asp:ListItem>
        </asp:dropdownlist>
    </div>
    <br class="clear"/>
        </div>

    
    <div id="mac3" runat="server">
    <div class="size-9 column">
        Create Partitions Method
    </div>
    <div class="size-5 column">
        <asp:dropdownlist ID="ddlPartitionMethodMac" runat="server" CssClass="ddlist"  AutoPostBack="True">
            <asp:ListItem>Standard Auto</asp:ListItem>
             <asp:ListItem>Standard HFSP</asp:ListItem>
             <asp:ListItem>Standard APFS</asp:ListItem>
            <asp:ListItem>Standard Core Storage</asp:ListItem>
            <asp:ListItem>Dynamic</asp:ListItem>
            <asp:ListItem>Custom Script</asp:ListItem>
        </asp:dropdownlist>
    </div>
    <br class="clear"/>
    </div>


    
    <div id="LinuxAll4" runat="server">
     <div class="size-9 column">
        Force Standard EFI Partitions
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkForceEfi" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>
     <div class="size-9 column">
        Force Standard Legacy Partitions
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkForceLegacy" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>


    <div class="size-9 column">
        Force Dynamic Partition For Exact Hdd Match
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkDownForceDynamic" runat="server" AutoPostBack="True" OnCheckedChanged="chkForceDynamic_OnCheckedChanged"></asp:CheckBox>
    </div>

    <br class="clear"/>
    <br/>
    </div>

    <h2>Upload Options</h2>
    
    <div id="LinuxAll5" runat="server">
    <div class="size-9 column">
        Remove GPT Structures
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkRemoveGpt" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>
        </div>
    
    <div id="LinuxBlock2" runat="server">
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
        Compression Algorithm:
    </div>
    <div class="size-8 column">
        <asp:DropDownList ID="ddlCompAlg" runat="server" CssClass="ddlist">
            <asp:ListItem>gzip</asp:ListItem>
            <asp:ListItem>lz4</asp:ListItem>
            <asp:ListItem>none</asp:ListItem>
        </asp:DropDownList>
    </div>
    <br class="clear"/>
    <div class="size-9 column">
        Compression Level:
    </div>

    <div class="size-8 column">
        <asp:DropDownList ID="ddlCompLevel" runat="server" CssClass="ddlist">
            <asp:ListItem>1</asp:ListItem>
            <asp:ListItem>2</asp:ListItem>
            <asp:ListItem>3</asp:ListItem>
            <asp:ListItem>4</asp:ListItem>
            <asp:ListItem>5</asp:ListItem>
            <asp:ListItem>6</asp:ListItem>
            <asp:ListItem>7</asp:ListItem>
            <asp:ListItem>8</asp:ListItem>
            <asp:ListItem>9</asp:ListItem>
        </asp:DropDownList>
    </div>

<br class="clear"/>
        </div>
    
    <div id="LinuxFileWinpe1" runat="server">
    <div class="size-9 column">
        Enable Multicast Support:
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkWimMulticast" runat="server"/>
    </div>
    <br class="clear"/>
        </div>

    <div id="mac4" runat="server">
<div class="size-9 column">
    Use Simple Upload Schema
</div>
<div class="size-8 column">
    <asp:CheckBox ID="chkSimpleSchema" runat="server" CssClass="textbox"></asp:CheckBox>
</div>

<br class="clear"/>
        </div>

<div class="size-9 column">
    Only Upload Schema
</div>
<div class="size-8 column">
    <asp:CheckBox ID="chkSchemaOnly" runat="server" CssClass="textbox"></asp:CheckBox>
</div>
    <br class="clear"/>
    <br/>
  
    
    <div id="LinuxAll6" runat="server">
    <h2>Multicast Options</h2>
     <div class="size-4 column">
        Sender Arguments:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtSender" runat="server" CssClass="textbox">
        </asp:TextBox>
    </div>
    <br class="clear"/>

    <div class="size-4 column">
        Receiver Arguments:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtReceiver" runat="server" CssClass="textbox">
        </asp:TextBox>
    </div>
    <br class="clear"/>
        </div>
</asp:Content>
