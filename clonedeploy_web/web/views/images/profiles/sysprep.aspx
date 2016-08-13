<%@ Page Title="" Language="C#" MasterPageFile="~/views/images/profiles/profiles.master" AutoEventWireup="true" CodeFile="sysprep.aspx.cs" Inherits="views_images_profiles_sysprep" %>


<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
      <li><a href="<%= ResolveUrl("~/views/images/profiles/general.aspx") %>?imageid=<%= Image.Id %>&profileid=<%= ImageProfile.Id %>&cat=profiles"><%= ImageProfile.Name %></a></li>
    <li>Sysprep Tags</li>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="SubHelp">
    <a href="<%= ResolveUrl("~/views/help/index.html") %>"   target="_blank">Help</a>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ActionsRightSub">
    <asp:LinkButton ID="btnUpdateSysprep" runat="server" OnClick="btnUpdateSysprep_OnClick" Text="Update Sysprep Options" CssClass="btn btn-default"  />
     <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
    <span class="caret"></span>
  </button>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
     <script type="text/javascript">
        $(document).ready(function() {
            $('#sysprep').addClass("nav-current");
            $("[id*=gvSysprep] td").hover(function () {
                $("td", $(this).closest("tr")).addClass("hover_row");
            }, function () {
                $("td", $(this).closest("tr")).removeClass("hover_row");
            });
        });
    </script>
     <asp:GridView ID="gvSysprep" runat="server" AllowSorting="True" DataKeyNames="Id" OnSorting="gridView_Sorting" AutoGenerateColumns="False" CssClass="Gridview" AlternatingRowStyle-CssClass="alt">
        <Columns>
             <asp:TemplateField>
                <HeaderStyle CssClass="chkboxwidth"></HeaderStyle>
                <ItemStyle CssClass="chkboxwidth"></ItemStyle>
                <HeaderTemplate>
                    Enabled
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="chkEnabled" runat="server"/>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:HyperLinkField Target="_blank" DataNavigateUrlFields="Id" DataNavigateUrlFormatString="~/views/global/sysprep/edit.aspx?cat=sub1&syspreptagid={0}" Text="View" ItemStyle-CssClass="chkboxwidth"/>
            <asp:BoundField DataField="Id" Visible="False"/>
               <asp:TemplateField ItemStyle-CssClass="width_100" HeaderText="Priority" SortExpression="Priority">
                                        <ItemTemplate>
                                           
                                                <asp:TextBox ID="txtPriority" runat="server" CssClass="textbox height_18"/>
                                          
                                        </ItemTemplate>
                                    </asp:TemplateField>
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ></asp:BoundField>
          
            
          
         
            
        </Columns>
        <EmptyDataTemplate>
            No Sysprep Tags Found
        </EmptyDataTemplate>
    </asp:GridView>
    
 
</asp:Content>

