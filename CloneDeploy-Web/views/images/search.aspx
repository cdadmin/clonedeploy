<%@ Page Title="" Language="C#" MasterPageFile="~/views/images/images.master" AutoEventWireup="true" Inherits="CloneDeploy_Web.views.images.ImageSearch" Codebehind="search.aspx.cs" %>

<asp:Content ID="Breadcrumb" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li>Search</li>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Help">
    <li role="separator" class="divider"></li>
    <li>
        <a href="<%= ResolveUrl("~/views/help/images-search.aspx") %>" target="_blank">Help</a>
    </li>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="SubPageActionsRight">
    <a class="confirm btn btn-default" href="#">Delete Selected Images</a>
    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        <span class="caret"></span>
    </button>

</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="AdditionalActions">
    <li>
        <asp:LinkButton ID="btnApproveImage" runat="server" OnClick="btnApproveImage_OnClick">Approve Selected Images</asp:LinkButton>
    </li>
</asp:Content>

<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">

    <script type="text/javascript">
        $(document).ready(function() {
            $('#search').addClass("nav-current");
            $("[id*=gvImages] td").hover(function() {
                $("td", $(this).closest("tr")).addClass("hover_row");
            }, function() {
                $("td", $(this).closest("tr")).removeClass("hover_row");
            });
        });

    </script>
    <p class="total">
        <asp:Label ID="lblTotal" runat="server"></asp:Label>
    </p>
    <div class="size-7 column">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="searchbox" OnTextChanged="search_Changed"></asp:TextBox>
    </div>
    <br class="clear"/>

    <asp:GridView ID="gvImages" runat="server" AllowSorting="True" AutoGenerateColumns="False" OnSorting="gridView_Sorting" DataKeyNames="Id" CssClass="Gridview" AlternatingRowStyle-CssClass="alt">
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
            <asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="~/views/images/edit.aspx?imageid={0}" Text="View" ItemStyle-CssClass="chkboxwidth"/>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:HiddenField ID="HiddenID" runat="server" Value='<%# Bind("Id") %>'/>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Id" HeaderText="imageID" SortExpression="Id" Visible="False"/>
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-CssClass="width_200"/>
            <asp:TemplateField ShowHeader="True" HeaderText="Size On Server" ItemStyle-CssClass="width_200 mobi-hide-smaller" HeaderStyle-CssClass="mobi-hide-smaller">
                <ItemTemplate>
                    <asp:Label ID="lblSize" runat="server" CausesValidation="false" CssClass="lbl_file"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Environment" HeaderText="Imaging Environment" ItemStyle-CssClass="width_200"></asp:BoundField>
            <asp:BoundField DataField="Approved" HeaderText="Approved" ItemStyle-CssClass="width_200"></asp:BoundField>
            <asp:BoundField DataField="LastUsed" HeaderText="Last Used" SortExpression="LastUsed" ItemStyle-CssClass="width_200"/>

            <asp:TemplateField>
                <ItemTemplate>
                    <tr>
                        <td id="tdHds" runat="server" visible="false" colspan="900">
                            <asp:GridView ID="gvHDs" AutoGenerateColumns="false" runat="server" CssClass="Gridview gv_parts hdlist" ShowHeader="false" Visible="false" AlternatingRowStyle-CssClass="alt">
                                <Columns>
                                    <asp:BoundField DataField="name" HeaderText="#" ItemStyle-CssClass="width_100"></asp:BoundField>
                                    <asp:TemplateField ShowHeader="True" HeaderText="Server Size" ItemStyle-CssClass="mobi-hide-smaller" HeaderStyle-CssClass="mobi-hide-smaller">
                                        <ItemTemplate>
                                            <asp:Label ID="lblHDSize" runat="server" CausesValidation="false" CssClass="lbl_file"></asp:Label>
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
            No Images Found
        </EmptyDataTemplate>
    </asp:GridView>

    <div id="confirmbox" class="confirm-box-outer">
        <div class="confirm-box-inner">
            <h4>
                <asp:Label ID="lblTitle" runat="server" Text="Delete The Selected Images?" CssClass="modaltitle"></asp:Label>
            </h4>
            <div class="confirm-box-btns">
                <asp:LinkButton ID="ConfirmButton" OnClick="btnSubmit_Click" runat="server" Text="Yes" CssClass="confirm_yes"/>
                <asp:LinkButton ID="CancelButton" runat="server" Text="No" CssClass="confirm_no"/>
            </div>
        </div>
    </div>
</asp:Content>