<%@ Page Title="" Language="C#" MasterPageFile="~/views/global/munki/munki.master" AutoEventWireup="true" CodeFile="availablecatalogs.aspx.cs" Inherits="views_global_munki_availablecatalogs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
    <li><a href="<%= ResolveUrl("~/views/global/munki/general.aspx") %>?manifestid=<%= ManifestTemplate.Id %>&cat=sub2"><%= ManifestTemplate.Name %></a></li>
    <li>Catalogs</li>
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
    <li id="assigned"><a  href="<%= ResolveUrl("~/views/global/munki/assignedcatalogs.aspx") %>?manifestid=<%= ManifestTemplate.Id %>&cat=sub2"><span class="sub-nav-text">Assigned Catalogs</span></a></li>
   <li id="available"><a  href="<%= ResolveUrl("~/views/global/munki/availablecatalogs.aspx") %>?manifestid=<%= ManifestTemplate.Id %>&cat=sub2"><span class="sub-nav-text">Available Catalogs</span></a></li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="SubContent2" Runat="Server">
     <script type="text/javascript">
        $(document).ready(function() {
            $('#catalogs').addClass("nav-current");
            $('#available').addClass("nav-current");
            $("[id*=gvCatalogs] td").hover(function () {
                $("td", $(this).closest("tr")).addClass("hover_row");
            }, function () {
                $("td", $(this).closest("tr")).removeClass("hover_row");
            });
        });
     </script>
  
  
   
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

