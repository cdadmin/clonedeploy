<%@ Page Title="" Language="C#" MasterPageFile="~/views/global/munki/munki.master" AutoEventWireup="true" Inherits="views_global_munki_availablemanifests" Codebehind="availablemanifests.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
    <li><a href="<%= ResolveUrl("~/views/global/munki/general.aspx") %>?manifestid=<%= ManifestTemplate.Id %>&cat=sub2"><%= ManifestTemplate.Name %></a></li>
    <li>Included Manifests</li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubHelp" Runat="Server">
    <li role="separator" class="divider"></li>
      <li><a href="<%= ResolveUrl("~/views/help/global-munki.aspx") %>"   target="_blank">Help</a></li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ActionsRightSub" Runat="Server">
     <asp:LinkButton ID="buttonUpdate" runat="server" OnClick="buttonUpdate_OnClick" Text="Update Template" CssClass="btn btn-default"  />
     <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
    <span class="caret"></span>
  </button>
</asp:Content>

<asp:Content runat="server" ID="TopNav" ContentPlaceHolderID="SubPageNavSub">
    <li id="assigned"><a  href="<%= ResolveUrl("~/views/global/munki/assignedmanifests.aspx") %>?manifestid=<%= ManifestTemplate.Id %>&cat=sub2"><span class="sub-nav-text">Assigned Included Manifests</span></a></li>
   <li id="available"><a  href="<%= ResolveUrl("~/views/global/munki/availablemanifests.aspx") %>?manifestid=<%= ManifestTemplate.Id %>&cat=sub2"><span class="sub-nav-text">Available Included Manifests</span></a></li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="SubContent2" Runat="Server">
     <script type="text/javascript">
        $(document).ready(function() {
            $('#manifests').addClass("nav-current");
            $('#available').addClass("nav-current");
            $("[id*=gvPkgInfos] td").hover(function () {
                $("td", $(this).closest("tr")).addClass("hover_row");
            }, function () {
                $("td", $(this).closest("tr")).removeClass("hover_row");
            });
        });
    </script>
    
  
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
             
            <asp:BoundField DataField="Name" HeaderText="Manifest Name" SortExpression="Name" ItemStyle-CssClass="width_200" />
              <asp:TemplateField>
                <HeaderTemplate>Condition</HeaderTemplate>
                <ItemTemplate>
                    <asp:TextBox ID="txtCondition" runat="server"/>
                </ItemTemplate>
            </asp:TemplateField>
            
        </Columns>
        <EmptyDataTemplate>
            No Available Manifests Found
        </EmptyDataTemplate>
    </asp:GridView>
        </div>
    </asp:Content>

