<%@ Page Title="" Language="C#" MasterPageFile="~/views/masters/Task.master" AutoEventWireup="true" Inherits="views.tasks.TaskCustom" CodeFile="ondemand.aspx.cs" %>

<%@ MasterType VirtualPath="~/views/masters/Task.master" %>
<%@ Reference virtualPath="~/views/masters/Site.master" %>

<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#taskond').addClass("nav-current");
        });
    </script>
    <asp:Label ID="secureMsg" runat="server" Visible="false"></asp:Label>
    <div id="secure" runat="server" visible="false">
        <div class="size-4 column">
            <asp:DropDownList ID="ddlImage" runat="server" CssClass="ddlist">
            </asp:DropDownList>
            <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Start Multicast" CssClass="submits"/>
        </div>
    </div>
</asp:Content>