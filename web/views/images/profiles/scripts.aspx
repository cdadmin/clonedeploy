<%@ Page Title="" Language="C#" MasterPageFile="~/views/images/profiles/profiles.master" AutoEventWireup="true" CodeFile="scripts.aspx.cs" Inherits="views_images_profiles_scripts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
      <li><a href="<%= ResolveUrl("~/views/images/profiles/general.aspx") %>?imageid=<%= Image.Id %>&profileid=<%= ImageProfile.Id %>&cat=profiles"><%= ImageProfile.Name %></a></li>
    <li>Scripts</li>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="SubHelp">
    <li role="separator" class="divider"></li>
     <li><a href="<%= ResolveUrl("~/views/help/images-scripts.aspx")%>"  target="_blank">Help</a></li>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ActionsRightSub">
    <asp:LinkButton ID="btnUpdateScripts" runat="server" OnClick="btnUpdateScripts_OnClick" Text="Update Script Options " CssClass="btn btn-default"  />
     <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
    <span class="caret"></span>
  </button>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
     <script type="text/javascript">
        $(document).ready(function() {
            $('#scripts').addClass("nav-current");
            $("[id*=gvScripts] td").hover(function () {
                $("td", $(this).closest("tr")).addClass("hover_row");
            }, function () {
                $("td", $(this).closest("tr")).removeClass("hover_row");
            });
        });
    </script>
     <asp:GridView ID="gvScripts" runat="server" AllowSorting="True" DataKeyNames="Id" OnSorting="gridView_Sorting" AutoGenerateColumns="False" CssClass="Gridview" AlternatingRowStyle-CssClass="alt">
        <Columns>
            <asp:TemplateField>
                <HeaderStyle CssClass="chkboxwidth"></HeaderStyle>
                <ItemStyle CssClass="chkboxwidth"></ItemStyle>
                <HeaderTemplate>
                    Pre
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="chkPre" runat="server"/>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField>
                <HeaderStyle CssClass="chkboxwidth"></HeaderStyle>
                <ItemStyle CssClass="chkboxwidth"></ItemStyle>
                <HeaderTemplate>
                    Post
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="chkPost" runat="server"/>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:HyperLinkField Target="_blank" DataNavigateUrlFields="Id" DataNavigateUrlFormatString="~/views/global/scripts/edit.aspx?cat=sub1&scriptid={0}" Text="View" ItemStyle-CssClass="chkboxwidth"/>
             <asp:TemplateField ItemStyle-CssClass="width_100" HeaderText="Priority" SortExpression="Priority">
                                        <ItemTemplate>
                                          
                                                <asp:TextBox ID="txtPriority" runat="server" CssClass="textbox height_18"/>
                                          
                                        </ItemTemplate>
                                    </asp:TemplateField>
           
            <asp:BoundField DataField="Id" HeaderText="computerID" SortExpression="computerID" Visible="False"/>
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ></asp:BoundField>
            
            
          
         
            
        </Columns>
        <EmptyDataTemplate>
            No Scripts Found
        </EmptyDataTemplate>
    </asp:GridView>
    
 
</asp:Content>

