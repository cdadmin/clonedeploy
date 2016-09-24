<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/bootmenu/bootmenu.master" AutoEventWireup="true" CodeFile="applenetboot.aspx.cs" Inherits="views_admin_bootmenu_applenetboot" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubHelp" Runat="Server">
     <li role="separator" class="divider"></li>
    <li><a href="<%= ResolveUrl("~/views/help/admin-bootmenu.aspx") %>"   target="_blank">Help</a></li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ActionsRightSub" Runat="Server">
    <asp:LinkButton ID="btnSubmitDefault" runat="server" Text="Generate" OnClick="btnSubmitDefault_OnClick" CssClass="btn btn-default width_100"  />
     <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
    <span class="caret"></span>
  </button>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="SubContent2" Runat="Server">
    <script type="text/javascript">
            $(document).ready(function() {
                $('#apple').addClass("nav-current");
            });
        </script>
    <p class="margin-bottom-10 bold">Enter the IP Address that your CloneDeploy Proxy DHCP Server is bound to.  It may or may not be the same as your CloneDeploy Server IP.</p>
    <div class="size-4 column">
                NetBoot Server IP:
     </div>
            <div class="size-4 column">
               <asp:TextBox runat="server" Id="txtServerIp" ></asp:TextBox>
            </div>
            <br class="clear"/>
    <br/><br/>
    
        <p class="bold">Enter an Id and NetBoot name for each NetBoot Image your CloneDeploy Proxy DHCP Server will provide.</p>
        <p class="bold">An Id can be any number between 1 - 65535.</p>
        <p class="bold">1–4095 indicates a local image unique to the server.</p>
        <p class="bold">4096–65535 is a duplicate, identical image stored on multiple servers for load balancing.</p>

  <br class="clear"/>
    <br/>
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
    <br class="clear" />
    <asp:Label runat="server" ID="directions" CssClass="bold"></asp:Label>
    <br/>
    <p class="bold">Copy and Paste this string into your CloneDeploy Proxy DHCP config.ini file, for the apple-vendor-specific-information key</p>
    <asp:TextBox runat="server" ID="txtOut"></asp:TextBox>
</asp:Content>

