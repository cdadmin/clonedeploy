<%@ Page Title="" Language="C#" MasterPageFile="~/views/images/profiles/profiles.master" AutoEventWireup="true" CodeFile="upload.aspx.cs" Inherits="views_images_profiles_upload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
     <li><a href="<%= ResolveUrl("~/views/images/profiles/chooser.aspx") %>?imageid=<%= Image.Id %>&profileid=<%= ImageProfile.Id %>&cat=profiles"><%= ImageProfile.Name %></a></li>
    <li>Upload Options</li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
     <script type="text/javascript">
        $(document).ready(function() {
            $('#upload').addClass("nav-current");
        });
    </script>
    
      <div class="size-9 column">
        Remove GPT Structures
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkRemoveGpt" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>
    
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
    <div class="size-4 column">
    Compression Algorithm:
</div>
<div class="size-setting column">
    <asp:DropDownList ID="ddlCompAlg" runat="server" CssClass="ddlist">
        <asp:ListItem>gzip</asp:ListItem>
        <asp:ListItem>lz4</asp:ListItem>
         <asp:ListItem>none</asp:ListItem>
    </asp:DropDownList>
</div>
<br class="clear"/>
<div class="size-4 column">
    Compression Level:
</div>

<div class="size-setting column">
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
     <div class="size-4 column">
        &nbsp;
    </div>
    <div class="size-5 column">
        <asp:LinkButton ID="btnUpdateUpload" runat="server" OnClick="btnUpdateUpload_OnClick" Text="Update Upload Options" CssClass="submits"/>
    </div>
</asp:Content>

