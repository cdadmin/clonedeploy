<%@ Page Title="" Language="C#" MasterPageFile="~/views/tasks/task.master" AutoEventWireup="true" Inherits="views.tasks.TaskCustom" Codebehind="ondemand.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
     <li>Start On Demand Multicast</li>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Help">
     <li role="separator" class="divider"></li>
     <li><a href="<%= ResolveUrl("~/views/help/tasks-startond.aspx")%>" data-info="Help" target="_blank">Help</a></li>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="SubPageActionsRight">
     <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Start Multicast" CssClass="btn btn-default" />
     <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
    <span class="caret"></span>
  </button>
</asp:Content>

<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#ond').addClass("nav-current");
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