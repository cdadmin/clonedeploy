<%@ Page Title="" Language="C#" MasterPageFile="~/views/global/munki/munki.master" AutoEventWireup="true" CodeFile="managedinstalls.aspx.cs" Inherits="views_global_munki_managedinstalls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
    <li><a href="<%= ResolveUrl("~/views/global/munki/general.aspx") %>?manifestid=<%= ManifestTemplate.Id %>&cat=sub2"><%= ManifestTemplate.Name %></a></li>
    <li>Managed Installs</li>
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
            $('#managedinstalls').addClass("nav-current");
        });
    </script>
    
    <h4>Assigned Managed Installs</h4>
    <asp:LinkButton runat="server" ID="showAssigned" Text="Show / Hide" OnClick="showAssigned_OnClick"></asp:LinkButton>
    <br class="clear"/>
    
    <div id="Assigned" runat="Server">
          <p class="total">
        <asp:Label ID="lblTotalAssigned" runat="server"></asp:Label>
    </p>
      <div class="size-7 column">
        <asp:TextBox ID="txtSearchAssigned" runat="server" CssClass="searchbox" OnTextChanged="search_Changed" AutoPostBack="True"></asp:TextBox>
    </div>
    <br class="clear"/>

    <div class="size-11 column right">
    
        <asp:DropDownList runat="server" ID="ddlLimitAssigned" AutoPostBack="True" OnSelectedIndexChanged="ddlLimit_OnSelectedIndexChanged" CssClass="ddlist">
        <asp:ListItem>25</asp:ListItem>
        <asp:ListItem Selected="True">100</asp:ListItem>
         <asp:ListItem >250</asp:ListItem>
        <asp:ListItem >500</asp:ListItem>
          <asp:ListItem>1000</asp:ListItem>
         <asp:ListItem>5000</asp:ListItem>
        <asp:ListItem>All</asp:ListItem>
        </asp:DropDownList>
    </div>
     <asp:GridView ID="gvTemplateInstalls" runat="server"  DataKeyNames="Id"  AutoGenerateColumns="false" CssClass="Gridview" AlternatingRowStyle-CssClass="alt">
        <Columns>
           
            <asp:TemplateField>
                
                <HeaderStyle CssClass="chkboxwidth"></HeaderStyle>
                <ItemStyle CssClass="chkboxwidth"></ItemStyle>
               
                <ItemTemplate>
                    <asp:CheckBox ID="chkSelector" runat="server" Checked="True"/>
                </ItemTemplate>
            </asp:TemplateField>
             
           <asp:BoundField DataField="Name" HeaderText="Package Name" SortExpression="Name" />
            <asp:BoundField DataField="Version" HeaderText="Package Version" SortExpression="Version" />
            
        </Columns>
        <EmptyDataTemplate>
            No Assigned Managed Installs Found
        </EmptyDataTemplate>
    </asp:GridView>
    </div>
    
    <h4>Available Packages </h4>
    <asp:LinkButton runat="server" ID="showAvailable" Text="Show / Hide" OnClick="showAvailable_OnClick"></asp:LinkButton>
    <br class="clear"/>
    <div id="Available" runat="server">
     <p class="total">
        <asp:Label ID="lblTotalAvailable" runat="server"></asp:Label>
    </p>
    <div class="size-7 column">
        <asp:TextBox ID="txtSearchAvailable" runat="server" CssClass="searchbox" OnTextChanged="search_Changed" AutoPostBack="True"></asp:TextBox>
    </div>
        <br class="clear"/>
    <div class="size-11 column right">
    
        <asp:DropDownList runat="server" ID="ddlLimitAvailable" AutoPostBack="True" OnSelectedIndexChanged="ddlLimit_OnSelectedIndexChanged" CssClass="ddlist">
        <asp:ListItem>25</asp:ListItem>
        <asp:ListItem Selected="True">100</asp:ListItem>
         <asp:ListItem >250</asp:ListItem>
        <asp:ListItem >500</asp:ListItem>
          <asp:ListItem>1000</asp:ListItem>
         <asp:ListItem>5000</asp:ListItem>
        <asp:ListItem>All</asp:ListItem>
        </asp:DropDownList>
    </div>
    <br class="clear" />
     <asp:GridView ID="gvPkgInfos" runat="server"  DataKeyNames="Name"  AutoGenerateColumns="false" CssClass="Gridview" AlternatingRowStyle-CssClass="alt">
        <Columns>
           
            <asp:TemplateField>
                
                <HeaderStyle CssClass="chkboxwidth"></HeaderStyle>
                <ItemStyle CssClass="chkboxwidth"></ItemStyle>
                <HeaderTemplate>
                    <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelectAll_CheckedChanged"/>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="chkSelector" runat="server"/>
                </ItemTemplate>
            </asp:TemplateField>
             
            <asp:BoundField DataField="Name" HeaderText="Package Name" SortExpression="Name" />
            <asp:BoundField DataField="Version" HeaderText="Package Version" SortExpression="Version" />
             <asp:TemplateField>
                <HeaderTemplate>Include Version</HeaderTemplate>
                <HeaderStyle CssClass="chkboxwidth"></HeaderStyle>
                <ItemStyle CssClass="chkboxwidth"></ItemStyle>
               
                <ItemTemplate>
                    <asp:CheckBox ID="chkUseVersion" runat="server"/>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            No Available Packages Found
        </EmptyDataTemplate>
    </asp:GridView>
        </div>
</asp:Content>

