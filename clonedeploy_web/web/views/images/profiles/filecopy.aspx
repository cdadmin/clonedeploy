<%@ Page Title="" Language="C#" MasterPageFile="~/views/images/profiles/profiles.master" AutoEventWireup="true" CodeFile="filecopy.aspx.cs" Inherits="views_images_profiles_filecopy" %>


<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
    <li>
        <a href="<%= ResolveUrl("~/views/images/profiles/general.aspx") %>?imageid=<%= Image.Id %>&profileid=<%= ImageProfile.Id %>&cat=profiles"><%= ImageProfile.Name %></a>
    </li>
    <li>File Copy</li>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="SubHelp">
    <a href="<%= ResolveUrl("~/views/help/index.html") %>"   target="_blank">Help</a>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ActionsRightSub">
    <asp:LinkButton ID="btnUpdateFile" runat="server" OnClick="btnUpdateFile_OnClick" Text="Update File Options" CssClass="btn btn-default" />
     <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
    <span class="caret"></span>
  </button>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#filecopy').addClass("nav-current");
            $("[id*=gvFile] td").hover(function () {
                $("td", $(this).closest("tr")).addClass("hover_row");
            }, function () {
                $("td", $(this).closest("tr")).removeClass("hover_row");
            });
        });
    </script>
    <asp:GridView ID="gvFile" runat="server" AllowSorting="True" DataKeyNames="Id" OnSorting="gridView_Sorting" AutoGenerateColumns="False" CssClass="Gridview" AlternatingRowStyle-CssClass="alt">
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
           
             <asp:HyperLinkField Target="_blank" DataNavigateUrlFields="Id" DataNavigateUrlFormatString="~/views/global/filesandfolders/edit.aspx?cat=sub1&fileid={0}" Text="View" ItemStyle-CssClass="chkboxwidth"/>
              <asp:TemplateField ItemStyle-CssClass="width_50" HeaderText="Priority" SortExpression="Priority">
                <ItemTemplate>
                  
                        <asp:TextBox ID="txtPriority" runat="server" CssClass="textbox height_18"/>
                 
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Id" Visible="False"/>
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-CssClass="width_100"></asp:BoundField>
           
             <asp:TemplateField ItemStyle-CssClass="width_50" HeaderText="Destination Partition" SortExpression="Destination Partition">
                <ItemTemplate>
                  
                        <asp:TextBox ID="txtPartition" runat="server" CssClass="textbox height_18"/>
                  </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField ItemStyle-CssClass="width_100" HeaderText="Destination Path" SortExpression="Destination Path">
                <ItemTemplate>
                   
                        <asp:TextBox ID="txtPath" runat="server" CssClass="textbox height_18"/>
                   
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField ItemStyle-CssClass="width_100" HeaderText="Folder Copy Mode" SortExpression="Folder Copy Mode">
                <ItemTemplate>
                    <div id="settings">
                        <asp:DropDownList ID="ddlFolderMode" runat="server" CssClass="ddlist">
                            <asp:ListItem>Folder</asp:ListItem>
                            <asp:ListItem>Folder Contents</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>

           
        </Columns>
        <EmptyDataTemplate>
            No Files / Folders Found
        </EmptyDataTemplate>
    </asp:GridView>


</asp:Content>

