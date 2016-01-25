<%@ Page Title="" Language="C#" MasterPageFile="~/views/tasks/Task.master" AutoEventWireup="true" Inherits="views.tasks.TaskCustom" CodeFile="ondemand.aspx.cs" %>

<asp:Content runat="server" ContentPlaceHolderID="Help">
     <a href="<%= ResolveUrl("~/views/help/index.html")%>" class="icon help" data-info="Help" target="_blank"></a>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="SubPageActionsRight">
     <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Start Multicast" CssClass="submits"/>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#taskond').addClass("nav-current");
        });
    </script>

    <div class="size-4 column">
        Image:
    </div>
         <div class="size-5 column">
        <asp:DropDownList ID="ddlComputerImage" runat="server" CssClass="ddlist" AutoPostBack="true" OnSelectedIndexChanged="ddlComputerImage_OnSelectedIndexChanged"/>
        </div>
    <br class="clear"/>
   
    <div class="size-4 column">
        Image Profile:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlImageProfile" runat="server" CssClass="ddlist"/>
    </div>
    
     <br class="clear"/>
   
    <div class="size-4 column">
        Client Count:
    </div>
    <div class="size-5 column">
        <asp:TextBox runat="server" ID="txtClientCount" CssClass="textbox"></asp:TextBox>
    </div>
</asp:Content>