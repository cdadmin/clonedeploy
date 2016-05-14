<%@ Page Title="" Language="C#" MasterPageFile="~/views/global/munki/munki.master" AutoEventWireup="true" CodeFile="catalogs.aspx.cs" Inherits="views_global_munki_catalogs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
    <li><a href="<%= ResolveUrl("~/views/global/munki/general.aspx") %>?manifestid=<%= ManifestTemplate.Id %>&cat=sub2"><%= ManifestTemplate.Name %></a></li>
    <li>Catalogs</li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubHelp" Runat="Server">
      <a href="<%= ResolveUrl("~/views/help/index.html") %>" class="submits help" target="_blank"></a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ActionsRightSub" Runat="Server">
     <asp:LinkButton ID="buttonUpdate" runat="server" OnClick="buttonUpdate_OnClick" Text="Update Manifest Template" CssClass="submits actions green" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="SubContent2" Runat="Server">
     <script type="text/javascript">
        $(document).ready(function() {
            $('#catalogs').addClass("nav-current");
        });


     

     </script>

  
    <asp:Label runat="server" ID="na" Visible="False"></asp:Label>
   
    
    <h4>Assigned Catalogs</h4>
    <asp:LinkButton runat="server" ID="showAssigned" Text="Show / Hide" OnClick="showAssigned_OnClick"></asp:LinkButton>
    <br class="clear"/>
    
    <div id="Assigned" runat="Server">
      <div class="size-7 column">
        <asp:TextBox ID="TextBox1" runat="server" CssClass="searchbox" OnTextChanged="search_Changed" AutoPostBack="True"></asp:TextBox>
    </div>
    <br class="clear"/>
     <asp:GridView ID="gvTemplateCatalogs" runat="server"  DataKeyNames="Id"  AutoGenerateColumns="false" CssClass="Gridview" AlternatingRowStyle-CssClass="alt">
        <Columns>
           
            <asp:TemplateField>
                
                <HeaderStyle CssClass="chkboxwidth"></HeaderStyle>
                <ItemStyle CssClass="chkboxwidth"></ItemStyle>
               
                <ItemTemplate>
                    <asp:CheckBox ID="chkSelector" runat="server" Checked="True"/>
                </ItemTemplate>
            </asp:TemplateField>
             
            <asp:BoundField DataField="Name" HeaderText="Catalog" SortExpression="Name" ItemStyle-CssClass="max_width300 width_300" />
              <asp:TemplateField ItemStyle-CssClass="width_50" HeaderText="Priority" SortExpression="Priority">
                <ItemTemplate>
                    <div id="settings">
                        <asp:TextBox ID="txtPriority" runat="server" Text='<%# Eval("Priority") %>' CssClass="textbox_specs"/>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            No Assigned Catalogs Found
        </EmptyDataTemplate>
    </asp:GridView>
    </div>
    

    <h4>Available Catalogs</h4>
    <asp:LinkButton runat="server" ID="showAvailable" Text="Show / Hide" OnClick="showAvailable_OnClick"></asp:LinkButton>
    <br class="clear"/>
    <div id="Available" runat="server">
      <p class="total">
        <asp:Label ID="lblTotal" runat="server"></asp:Label>
    </p>
    <div class="size-7 column">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="searchbox" OnTextChanged="search_Changed" AutoPostBack="True"></asp:TextBox>
    </div>
    <br class="clear"/>

     <asp:GridView ID="gvCatalogs" runat="server"  DataKeyNames="Name"  AutoGenerateColumns="false" CssClass="Gridview" AlternatingRowStyle-CssClass="alt">
        <Columns>
           
            <asp:TemplateField>
                
                <HeaderStyle CssClass="chkboxwidth"></HeaderStyle>
                <ItemStyle CssClass="chkboxwidth"></ItemStyle>
               
                <ItemTemplate>
                    <asp:CheckBox ID="chkSelector" runat="server"/>
                </ItemTemplate>
            </asp:TemplateField>
             
            <asp:BoundField DataField="Name" HeaderText="Catalog" SortExpression="Name" ItemStyle-CssClass="max_width300 width_300" />
              <asp:TemplateField ItemStyle-CssClass="width_50" HeaderText="Priority" SortExpression="Priority">
                <ItemTemplate>
                    <div id="settings">
                        <asp:TextBox ID="txtPriority" runat="server" CssClass="textbox_specs"/>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            No Available Catalogs Found
        </EmptyDataTemplate>
    </asp:GridView>
        </div>
</asp:Content>

