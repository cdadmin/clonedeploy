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
   
<asp:GridView ID="gvPartitions" runat="server" AutoGenerateColumns="false" DataKeyNames="Id" CssClass="Gridview"
 OnRowEditing="OnRowEditing" OnRowCancelingEdit="OnRowCancelingEdit" ShowFooter="True" OnRowDataBound="gvPartitions_OnDataBound"
OnRowUpdating="OnRowUpdating" OnRowDeleting="OnRowDeleting" EmptyDataText="No Partitions Have Been Defined For This Layout.">
<Columns>
    <asp:TemplateField HeaderText="Type" ItemStyle-Width="150">
        <ItemTemplate>
            <asp:Label ID="lblType" runat="server" Text='<%# Eval("Type") %>'></asp:Label>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:dropdownlist ID="ddlType" runat="server" Text='<%# Eval("Type") %>' OnSelectedIndexChanged="ddlType_OnSelectedIndexChanged" AutoPostBack="True">
                 <asp:listitem>Primary</asp:listitem>
             <asp:listitem>Extended</asp:listitem>
             <asp:listitem>Logical</asp:listitem>
            </asp:dropdownlist>
        </EditItemTemplate>
        <FooterTemplate>
            <asp:dropdownlist ID="ddlTypeAdd" runat="server" OnSelectedIndexChanged="ddlTypeAdd_OnSelectedIndexChanged" AutoPostBack="true">
             <asp:listitem>Primary</asp:listitem>
             <asp:listitem>Extended</asp:listitem>
             <asp:listitem>Logical</asp:listitem>
         </asp:dropdownlist>
        </FooterTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Number" ItemStyle-Width="150">
        <ItemTemplate>
            <asp:Label ID="lblNumber" runat="server" Text='<%# Eval("Number") %>'></asp:Label>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:DropDownList ID="ddlNumber" runat="server"></asp:DropDownList>
        </EditItemTemplate>
         <FooterTemplate>
            <asp:dropdownlist ID="ddlNumberAdd" runat="server"></asp:dropdownlist>
        </FooterTemplate>
    </asp:TemplateField>
     <asp:TemplateField HeaderText="FS Type" ItemStyle-Width="150">
        <ItemTemplate>
            <asp:Label ID="lblFsType" runat="server" Text='<%# Eval("FsType") %>'></asp:Label>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:dropdownlist ID="ddlFsType" runat="server" Text='<%# Eval("FsType") %>'>
                 <asp:ListItem>ntfs</asp:ListItem>
            </asp:dropdownlist>
        </EditItemTemplate>
          <FooterTemplate>
             <asp:dropdownlist ID="ddlFsTypeAdd" runat="server">
             <asp:ListItem>ntfs</asp:ListItem>
         </asp:dropdownlist>
        </FooterTemplate>
    </asp:TemplateField>
     <asp:TemplateField HeaderText="Size" ItemStyle-Width="150">
        <ItemTemplate>
            <asp:Label ID="lblSize" runat="server" Text='<%# Eval("Size") %>'></asp:Label>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtSize" runat="server" Text='<%# Eval("Size") %>'></asp:TextBox>
        </EditItemTemplate>
          <FooterTemplate>
            <asp:TextBox ID="txtSizeAdd" runat="server"></asp:TextBox>
        </FooterTemplate>
    </asp:TemplateField>
     <asp:TemplateField HeaderText="Unit" ItemStyle-Width="150">
        <ItemTemplate>
            <asp:Label ID="lblUnit" runat="server" Text='<%# Eval("Unit") %>'></asp:Label>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:dropdownlist ID="ddlUnit" runat="server" Text='<%# Eval("Unit") %>'>
                 <asp:ListItem>MB</asp:ListItem>
                <asp:ListItem>GB</asp:ListItem>
               <asp:ListItem>Percent</asp:ListItem>
            </asp:dropdownlist>
        </EditItemTemplate>
          <FooterTemplate>
              <asp:dropdownlist ID="ddlUnitAdd" runat="server">
               <asp:ListItem>MB</asp:ListItem>
                <asp:ListItem>GB</asp:ListItem>
               <asp:ListItem>Percent</asp:ListItem>
           </asp:dropdownlist>
        </FooterTemplate>
    </asp:TemplateField>
     <asp:TemplateField HeaderText="Boot/Active" ItemStyle-Width="150">
        <ItemTemplate>
            <asp:checkbox ID="lblBoot" runat="server" Checked='<%# Eval("Boot").ToString() == "1" ? true:false %>' Enabled="false"></asp:checkbox>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:checkbox ID="chkBoot" runat="server" ></asp:checkbox>
        </EditItemTemplate>
          <FooterTemplate>
            <asp:checkbox ID="chkBootAdd" runat="server"></asp:checkbox>

        </FooterTemplate>
    </asp:TemplateField>
    	<asp:TemplateField ShowHeader="False">
		<EditItemTemplate>
			<asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" 
				CommandName="Update" Text="Update"></asp:LinkButton>
			&nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" 
				CommandName="Cancel" Text="Cancel"></asp:LinkButton>
		</EditItemTemplate>
		<ItemTemplate>
			<asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
				CommandName="Edit" Text="Edit"></asp:LinkButton>
            <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False" 
				CommandName="Delete" Text="Delete"></asp:LinkButton>
		</ItemTemplate>
              <FooterTemplate>

               <asp:Button ID="btnAdd1" runat="server" Text="Add" OnClick="Insert" />
        </FooterTemplate>
	</asp:TemplateField>
    <asp:TemplateField >
       
        
    </asp:TemplateField>
</Columns>
</asp:GridView>


</asp:Content>

