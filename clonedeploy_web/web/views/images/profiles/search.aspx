<%@ Page Title="" Language="C#" MasterPageFile="~/views/images/profiles/profiles.master" AutoEventWireup="true" CodeFile="search.aspx.cs" Inherits="views_images_profiles_search" %>



<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
    <li>Search</li>
</asp:Content>


<asp:Content runat="server" ContentPlaceHolderID="SubHelp">
     <li role="separator" class="divider"></li>
     <li><a href="<%= ResolveUrl("~/views/help/images-profiles.aspx")%>"  target="_blank">Help</a></li>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ActionsRightSub">
      <a class="confirm btn btn-default" href="#">Delete Selected Profiles</a>
     <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
    <span class="caret"></span>
  </button>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#search').addClass("nav-current");
            $("[id*=gvProfiles] td").hover(function () {
                $("td", $(this).closest("tr")).addClass("hover_row");
            }, function () {
                $("td", $(this).closest("tr")).removeClass("hover_row");
            });
       
        });
    </script>
   
   
    <br class="clear"/>

    <asp:GridView ID="gvProfiles" runat="server" AllowSorting="True" DataKeyNames="Id" OnSorting="gridView_Sorting" AutoGenerateColumns="False" CssClass="Gridview" AlternatingRowStyle-CssClass="alt">
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
              <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="width_30 mobi-hide-smaller" HeaderStyle-CssClass="mobi-hide-smaller">
                <ItemTemplate>
                    <div style="width: 0">
                        <asp:LinkButton ID="btnHds" runat="server" CausesValidation="false" CommandName="" Text="+" OnClick="btnHds_Click"></asp:LinkButton>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:HyperLinkField DataNavigateUrlFields="Id,ImageId" DataNavigateUrlFormatString="~/views/images/profiles/general.aspx?imageid={1}&profileid={0}&cat=profiles" Text="View" ItemStyle-CssClass="chkboxwidth"/>
               <asp:TemplateField ShowHeader="False">
                   <ItemStyle CssClass="chkboxwidth"></ItemStyle>
                <ItemTemplate>
                    <asp:LinkButton runat="server" Id="profileClone" Text="Clone"  OnClick="profileClone_OnClick"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Id" HeaderText="ID" SortExpression="ID" Visible="False"/>
            <asp:BoundField DataField="ImageId" HeaderText="ImageID" SortExpression="ImageID" Visible="False"/>
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-CssClass="width_200"></asp:BoundField>
            <asp:TemplateField ShowHeader="True" HeaderText="Minimum Client Size" ItemStyle-CssClass="mobi-hide-smaller" HeaderStyle-CssClass="mobi-hide-smaller">
                <ItemTemplate>
                    <asp:Label ID="lblSizeClient" runat="server" CausesValidation="false" CssClass="lbl_file"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
           
          <asp:TemplateField>
                <ItemTemplate>
                    <tr>
                        <td id="tdHds" runat="server" visible="false" colspan="900">
                            <asp:GridView ID="gvHDs" AutoGenerateColumns="false" runat="server" CssClass="Gridview gv_parts hdlist" ShowHeader="false" Visible="false" AlternatingRowStyle-CssClass="alt">
                                <Columns>
                                    <asp:BoundField DataField="name" HeaderText="#" ItemStyle-CssClass="width_100"></asp:BoundField>
                                 
                                    <asp:TemplateField ShowHeader="True" HeaderText="Client Size" ItemStyle-CssClass="mobi-hide-smaller" HeaderStyle-CssClass="mobi-hide-smaller">
                                        <ItemTemplate>
                                            <asp:Label ID="lblHDSizeClient" runat="server" CausesValidation="false" CssClass="lbl_file"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                            </asp:GridView>
                        </td>


                    </tr>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            No Profiles Found
        </EmptyDataTemplate>
    </asp:GridView>
  
    <div id="confirmbox" class="confirm-box-outer">
        <div class="confirm-box-inner">
            <h4>
                <asp:Label ID="lblTitle" runat="server" Text="Delete The Selected Profiles?"></asp:Label>
            </h4>
            <div class="confirm-box-btns">
                <asp:LinkButton ID="ConfirmButton" OnClick="ButtonConfirmDelete_Click" runat="server" Text="Yes" CssClass="confirm_yes"/>
                <asp:LinkButton ID="CancelButton" runat="server" Text="No" CssClass="confirm_no"/>
            </div>
        </div>
    </div>
</asp:Content>

