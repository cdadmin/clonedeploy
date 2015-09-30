<%@ Page Title="" Language="C#" MasterPageFile="~/views/computers/computers.master" AutoEventWireup="true" Inherits="views.hosts.Addhosts" CodeFile="create.aspx.cs" %>

<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">
       
        $(document).ready(function() {
           
            $('#new').addClass("nav-current");
           
           $("#form1").validate({
                rules: {
                    <%=txtHostName.UniqueID %>: {
                        minlength: 2,
                        required: true
                    }
                 
                }, messages: {}
            });
        });
        
      
    </script>
    <div class="size-4 column">
        Host Name:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtHostName" runat="server" CssClass="textbox" ClientIDMode="Static"></asp:TextBox>
      
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Host MAC Address:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtHostMac" runat="server" CssClass="textbox" MaxLength="17"></asp:TextBox>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Image:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlHostImage" runat="server" CssClass="ddlist" AutoPostBack="true" OnSelectedIndexChanged="ddlHostImage_OnSelectedIndexChanged"/>
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
        Host Description:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtHostDesc" runat="server" CssClass="descbox" TextMode="MultiLine"></asp:TextBox>
    </div>
    <br class="clear"/>
   
   
    <div class="size-4 column">
        Create Another?
        <asp:CheckBox runat="server" ID="createAnother"/>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        &nbsp;
    </div>
    <div class="size-5 column">
        <asp:LinkButton ID="buttonAddHost" runat="server" OnClick="ButtonAddHost_Click" Text="Add Host" CssClass="submits" ClientIDMode="Static"/>
    </div>
</asp:Content>