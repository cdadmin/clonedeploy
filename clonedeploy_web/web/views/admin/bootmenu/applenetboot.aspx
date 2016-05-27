<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/bootmenu/bootmenu.master" AutoEventWireup="true" CodeFile="applenetboot.aspx.cs" Inherits="views_admin_bootmenu_applenetboot" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubHelp" Runat="Server">
    <a href="<%= ResolveUrl("~/views/help/index.html") %>"   target="_blank">Help</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ActionsRightSub" Runat="Server">
    <asp:LinkButton ID="btnSubmitDefault" runat="server" Text="Generate" OnClick="btnSubmitDefault_OnClick"  />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="SubContent2" Runat="Server">
    <script type="text/javascript">
            $(document).ready(function() {
                $('#apple').addClass("nav-current");
            });
        </script>
    <div class="size-4 column">
                NetBoot Server IP:
     </div>
            <div class="size-4 column">
               <asp:TextBox runat="server" Id="txtServerIp" ></asp:TextBox>
            </div>
            <br class="clear"/>
    
  
    <div class="size-1 column">
    <asp:GridView ID="gvNetBoot" runat="server" AutoGenerateColumns="false"  CssClass="Gridview" ShowFooter="True" OnRowDeleting="OnRowDeleting">
<Columns>
   
    <asp:TemplateField HeaderText="NetBoot Id" ItemStyle-Width="150">
        <ItemTemplate>
            <asp:Label ID="lblImageId" runat="server" Text='<%# Eval("ImageId") %>'></asp:Label>
        </ItemTemplate>
     
         <FooterTemplate>
            <asp:TextBox ID="txtIdAdd" runat="server"></asp:TextBox>
        </FooterTemplate>
    </asp:TemplateField>
    
     <asp:TemplateField HeaderText="NetBoot Name" ItemStyle-Width="150">
        <ItemTemplate>
            <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
        </ItemTemplate>
     
          <FooterTemplate>
            <asp:TextBox ID="txtNameAdd" runat="server"></asp:TextBox>
        </FooterTemplate>
    </asp:TemplateField>
   
    
    	<asp:TemplateField ShowHeader="False">
	
		<ItemTemplate>
			
            <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False" 
				CommandName="Delete" Text="Delete"></asp:LinkButton>
		</ItemTemplate>
              <FooterTemplate>

               <asp:LinkButton ID="btnAdd1" runat="server" Text="Add" OnClick="btnAdd1_OnClick" CssClass="btn-default" />
        </FooterTemplate>
	</asp:TemplateField>
    <asp:TemplateField >
       
        
    </asp:TemplateField>
</Columns>
</asp:GridView>
        </div>

    <asp:TextBox runat="server" ID="txtOut"></asp:TextBox>
</asp:Content>

