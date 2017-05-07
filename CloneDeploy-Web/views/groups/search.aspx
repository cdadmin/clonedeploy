<%@ Page Title="" Language="C#" MasterPageFile="~/views/groups/groups.master" AutoEventWireup="true" Inherits="views.groups.GroupSearch" Codebehind="search.aspx.cs" %>

<asp:Content ID="Breadcrumb" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li>Search</li>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="SubPageActionsRight">
    <a class="confirm btn btn-default" href="#"> Delete Selected Groups</a>
    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        <span class="caret"></span>
    </button>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Help">
    <li role="separator" class="divider"></li>
    <li>
        <a href="<%= ResolveUrl("~/views/help/groups-search.aspx") %>" target="_blank">Help</a>
    </li>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#search').addClass("nav-current");
            $("[id*=gvGroups] td").hover(function() {
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

    <asp:GridView ID="gvGroups" runat="server" AllowSorting="true" AutoGenerateColumns="False" OnSorting="gridView_Sorting" CssClass="Gridview" DataKeyNames="Id" AlternatingRowStyle-CssClass="alt">
        <Columns>
            <asp:TemplateField>
                <HeaderStyle CssClass="chkboxwidth"></HeaderStyle>
                <ItemStyle CssClass="chkboxwidth"></ItemStyle>
                <HeaderTemplate>
                    <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="True" OnCheckedChanged="SelectAll_CheckedChanged"/>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="chkSelector" runat="server"/>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="~/views/groups/edit.aspx?groupid={0}" Text="View" ItemStyle-CssClass="chkboxwidth"/>
            <asp:BoundField DataField="Id" HeaderText="groupID" InsertVisible="False" SortExpression="Id" Visible="False"/>
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-CssClass="width_200"/>
            <asp:TemplateField ItemStyle-CssClass="width_200 mobi-hide-smaller" HeaderStyle-CssClass="mobi-hide-smaller" HeaderText="Multicast Image">
                <ItemTemplate>
                    <asp:Label ID="lblImage" runat="server" Text='<%# Bind("Image.Name") %>'/>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type" HeaderStyle-CssClass="mobi-hide-smallest" ItemStyle-CssClass="width_200 mobi-hide-smallest"/>
            <asp:TemplateField ShowHeader="True" HeaderText="Members">
                <ItemTemplate>
                    <asp:Label ID="lblCount" runat="server" CausesValidation="false" CssClass="lbl_file "></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

        </Columns>
        <EmptyDataTemplate>
            No Groups Found
        </EmptyDataTemplate>
    </asp:GridView>

    <div id="confirmbox" class="confirm-box-outer">
        <div class="confirm-box-inner">
            <h4>
                <asp:Label ID="lblTitle" runat="server" Text="Delete The Selected Groups?"></asp:Label>
            </h4>
            <div class="confirm-box-btns">
                <asp:LinkButton ID="ConfirmButton" OnClick="btnSubmit_Click" runat="server" Text="Yes" CssClass="confirm_yes"/>
                <asp:LinkButton ID="CancelButton" runat="server" Text="No" CssClass="confirm_no"/>
            </div>
        </div>
    </div>
</asp:Content>