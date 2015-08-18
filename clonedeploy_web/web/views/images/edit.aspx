<%@ Page Title="" Language="C#" MasterPageFile="~/views/masters/Image.master" AutoEventWireup="true" Inherits="views.images.ImageEdit" CodeFile="edit.aspx.cs" %>

<%@ MasterType VirtualPath="~/views/masters/Image.master" %>
<%@ Reference virtualPath="~/views/masters/Site.master" %>
<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">

    <script type="text/javascript">
        $(document).ready(function() {
            $('#editoption').addClass("nav-current");
        });
    </script>
    <div class="size-4 column">
        Image Name:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtImageName" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>

    <div class="size-4 column">
        Image Description:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtImageDesc" runat="server" TextMode="MultiLine" CssClass="descbox"></asp:TextBox>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Protected:
    </div>
    <div class="size-5 column">
        <asp:CheckBox ID="chkProtected" runat="server"/>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Visible In On Demand:
    </div>
    <div class="size-5 column">
        <asp:CheckBox ID="chkVisible" runat="server"/>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        &nbsp;
    </div>
    <div class="size-5 column">
        <asp:LinkButton ID="btnUpdateImage" runat="server" OnClick="btnUpdateImage_Click" Text="Update Image" CssClass="submits"/>
    </div>
    <br class="clear"/>
    <div class="size-5 column" style="margin-left: -15px;">
        <h3>Image Directory Status:</h3>
    </div>
    <br class="clear"/>
    <div class="size-5 column">
        <asp:Label ID="lblImageHold" runat="server"></asp:Label><br/>
        <asp:Label ID="lblImage" runat="server"></asp:Label><br/>
        <asp:Label ID="lblImageHoldStatus" runat="server"></asp:Label><br/>
        <asp:Label ID="lblImageStatus" runat="server"></asp:Label>
        <br class="clear"/>

        <div class="size-5 column">
            <asp:LinkButton ID="btnFixImage" runat="server" OnClick="btnFixImage_Click" Text="Fix Image Directories" CssClass="submits"/>
        </div>
        <br class="clear"/>
    </div>

</asp:Content>