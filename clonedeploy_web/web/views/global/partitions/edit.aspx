<%@ Page Title="" Language="C#" MasterPageFile="~/views/global/partitions/partitions.master" AutoEventWireup="true" CodeFile="edit.aspx.cs" Inherits="views_global_partitions_edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
     <script type="text/javascript">
        $(document).ready(function() {
            $('#edit').addClass("nav-current");
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
   
    <br class="clear"/>
     <br/>
     <asp:GridView ID="gvPartitions" runat="server"  Width = "550px"

AutoGenerateColumns = "false" Font-Names = "Arial"

Font-Size = "11pt" AlternatingRowStyle-BackColor = "#C2D69B" 

HeaderStyle-BackColor = "green" AllowPaging ="true"  ShowFooter = "true" 

OnPageIndexChanging = "OnPaging" onrowediting="EditCustomer"

onrowupdating="UpdateCustomer"  onrowcancelingedit="CancelEdit"

PageSize = "10" >
        <Columns>


<asp:TemplateField ItemStyle-Width = "30px"  HeaderText = "CustomerID">

    <ItemTemplate>

        <asp:Label ID="lblCustomerID" runat="server"

        Text='<%# Eval("Number")%>'></asp:Label>

    </ItemTemplate>

    <FooterTemplate>

        <asp:TextBox ID="txtCustomerID" Width = "40px"

            MaxLength = "5" runat="server"></asp:TextBox>

    </FooterTemplate>

</asp:TemplateField>

<asp:TemplateField ItemStyle-Width = "100px"  HeaderText = "Name">

    <ItemTemplate>

        <asp:Label ID="lblContactName" runat="server"

                Text='<%# Eval("Type")%>'></asp:Label>

    </ItemTemplate>

    <EditItemTemplate>

        <asp:TextBox ID="txtContactName" runat="server"

            Text='<%# Eval("FsType")%>'></asp:TextBox>

    </EditItemTemplate> 

    <FooterTemplate>

        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>

    </FooterTemplate>

</asp:TemplateField>

<asp:TemplateField ItemStyle-Width = "150px"  HeaderText = "Company">

    <ItemTemplate>

        <asp:Label ID="lblCompany" runat="server"

            Text='<%# Eval("Size")%>'></asp:Label>

    </ItemTemplate>

    <EditItemTemplate>

        <asp:TextBox ID="txtCompany" runat="server"

            Text='<%# Eval("Unit")%>'></asp:TextBox>

    </EditItemTemplate> 

    <FooterTemplate>

        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>

    </FooterTemplate>

</asp:TemplateField>

<asp:TemplateField>

    <ItemTemplate>

        <asp:LinkButton ID="lnkRemove" runat="server"

            CommandArgument = '<%# Eval("Id")%>'

         OnClientClick = "return confirm('Do you want to delete?')"

        Text = "Delete" OnClick = "DeletePartition"></asp:LinkButton>

    </ItemTemplate>

    <FooterTemplate>

        <asp:Button ID="btnAdd" runat="server" Text="Add"

            OnClick = "AddPartition" />

    </FooterTemplate>

</asp:TemplateField>

<asp:CommandField  ShowEditButton="True" />

</Columns>
          
        <EmptyDataTemplate>
            No Partitions Have Been Defined For This Layout
        </EmptyDataTemplate>
    </asp:GridView>
</asp:Content>

