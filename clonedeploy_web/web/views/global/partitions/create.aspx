<%@ Page Title="" Language="C#" MasterPageFile="~/views/global/partitions/partitions.master" AutoEventWireup="true" CodeFile="create.aspx.cs" Inherits="views_global_partitions_create" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
    <li>New Partition Layout</li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SubContent2" Runat="Server">
     <script type="text/javascript">
        $(document).ready(function() {
            $('#create').addClass("nav-current");
        });
    </script>
    
     <div class="size-4 column">
        Partition Layout Name:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtLayoutName" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>
    
      <div class="size-4 column">
        Partition Table Type:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlLayoutType" runat="server" CssClass="ddlist">
            <asp:ListItem>MBR</asp:ListItem>
             <asp:ListItem>GPT</asp:ListItem>
        </asp:DropDownList>
    </div>
    <br class="clear"/>
    
     <div class="size-4 column">
        Imaging Environment:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlEnvironment" runat="server" CssClass="ddlist">
            <asp:ListItem>Windows</asp:ListItem>
             <asp:ListItem>Linux</asp:ListItem>
        </asp:DropDownList>
    </div>
    <br class="clear" />
     <div class="size-4 column">
        Priority:
    </div>
    <div class="size-5 column">
        <asp:textbox ID="txtPriority" runat="server" CssClass="textbox">

        </asp:textbox>
    </div>
    <br class="clear"/>

    <div class="size-4 column">
        &nbsp;
    </div>

    <div class="size-5 column">
         <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_OnClick" Text="Add Partition Layout" CssClass="submits" />
    </div>
   
    <br class="clear"/>
</asp:Content>

