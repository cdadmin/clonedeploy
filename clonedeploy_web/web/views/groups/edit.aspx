<%@ Page Title="" Language="C#" MasterPageFile="~/views/masters/Group.master" AutoEventWireup="true" Inherits="views.groups.GroupEdit" CodeFile="edit.aspx.cs" %>

<%@ MasterType VirtualPath="~/views/masters/Group.master" %>
<%@ Reference virtualPath="~/views/masters/Site.master" %>

<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">

    <script type="text/javascript">
        $(document).ready(function() {
            $('#editoption').addClass("nav-current");
        });
    </script>
    <div class="size-4 column">
        Group Name:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtGroupName" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Group Type:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlGroupType" runat="server" CssClass="ddlist" OnTextChanged="groupType_ddlChanged" AutoPostBack="true">
            <asp:ListItem>standard</asp:ListItem>
            <asp:ListItem>smart</asp:ListItem>
        </asp:DropDownList>
    </div>
    <br class="clear"/>
    <div id="smartParameters" runat="server" visible="false">
        <div class="size-4 column">
            Smart Expression:
        </div>
        <div class="size-5 column">
            <asp:TextBox ID="txtExpression" runat="server" CssClass="textbox"></asp:TextBox>
        </div>
        <br class="clear"/>
    </div>
    <div class="size-4 column">
        Group Image:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlGroupImage" runat="server" CssClass="ddlist">
        </asp:DropDownList>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Group Description:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtGroupDesc" runat="server" CssClass="descbox" TextMode="MultiLine"></asp:TextBox>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Group Kernel:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlGroupKernel" runat="server" CssClass="ddlist">
        </asp:DropDownList>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Group Boot Image:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlGroupBootImage" runat="server" CssClass="ddlist">
        </asp:DropDownList>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Group Arguments:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtGroupArguments" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Sender Arguments:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtGroupSenderArgs" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
    </div>
    <div class="size-5 column">
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Group Scripts:
    </div>
    <div class="size-5 column">
        <asp:ListBox ID="lbScripts" runat="server" SelectionMode="Multiple"></asp:ListBox>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        &nbsp;
    </div>
    <div class="size-5 column">
        <asp:LinkButton ID="btnSubmit" runat="server" Text="Update Group" OnClick="btnSubmit_Click" CssClass="submits"/>
    </div>
    <br class="clear"/>
    <hr/>
    <div id="standardGroup" runat="server" visible="false">
        <h5 style="text-align: left">
            Current Members - Select To Remove
        </h5>
        <asp:GridView ID="gvRemove" runat="server" AutoGenerateColumns="False" CssClass="Gridview" AlternatingRowStyle-CssClass="alt" DataKeyNames="Id">
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
                <asp:BoundField DataField="Id" HeaderText="hostID" InsertVisible="False" SortExpression="Id" Visible="False"/>
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-CssClass="width_200"/>
                <asp:BoundField DataField="Mac" HeaderText="MAC" SortExpression="Mac" ItemStyle-CssClass="mobi-hide-smallest" HeaderStyle-CssClass="mobi-hide-smallest"/>
                <asp:BoundField DataField="Group" HeaderText="Group" SortExpression="Group" ItemStyle-CssClass="mobi-hide-smaller" HeaderStyle-CssClass="mobi-hide-smaller"/>
            </Columns>
            <EmptyDataTemplate>
                No Hosts Found
            </EmptyDataTemplate>
        </asp:GridView>
        <h5 style="text-align: left">
            New Members - Select To Add
        </h5>
        <div class="size-7 column">
            <asp:TextBox ID="txtSearchHosts" runat="server" CssClass="searchbox label-host-search" AutoPostBack="True" OnTextChanged="txtSearchHosts_TextChanged"></asp:TextBox>
        </div>
        <br class="clear"/>
        <p class="total">
            <asp:Label ID="lblTotal" runat="server"></asp:Label>
        </p>
        <asp:GridView ID="gvAdd" runat="server" AutoGenerateColumns="False" CssClass="Gridview" AlternatingRowStyle-CssClass="alt" DataKeyNames="Id">
            <Columns>
                <asp:TemplateField>
                    <HeaderStyle CssClass="chkboxwidth"></HeaderStyle>
                    <ItemStyle CssClass="chkboxwidth"></ItemStyle>
                    <HeaderTemplate>
                        <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="True" OnCheckedChanged="SelectAllAdd_CheckedChanged"/>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkSelector" runat="server"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Id" HeaderText="hostID" SortExpression="Id" InsertVisible="False" Visible="False"/>
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-CssClass="width_200"/>
                <asp:BoundField DataField="Mac" HeaderText="MAC" SortExpression="Mac" ItemStyle-CssClass="mobi-hide-smallest" HeaderStyle-CssClass="mobi-hide-smallest"/>
                <asp:BoundField DataField="Group" HeaderText="Group" SortExpression="Group" ItemStyle-CssClass="mobi-hide-smaller" HeaderStyle-CssClass="mobi-hide-smaller"/>
            </Columns>
            <EmptyDataTemplate>
                No Hosts Found
            </EmptyDataTemplate>
        </asp:GridView>
        <div class="row">
        </div>
    </div>
    <div id="smartGroup" runat="server" visible="false">
        <h2>Expression Tester</h2>
        <div class="size-7 column">
            <asp:TextBox ID="txtSmartSearch" runat="server" AutoPostBack="True" CssClass="searchbox label-host-search" OnTextChanged="txtSearchHosts_TextChanged"></asp:TextBox>
        </div>
        <br class="clear"/>

        <p class="total">
            <asp:Label ID="lblSmartTotal" runat="server"></asp:Label>
        </p>
        <asp:GridView ID="gvSmartHosts" runat="server" AutoGenerateColumns="False" CssClass="Gridview" AlternatingRowStyle-CssClass="alt" DataKeyNames="Id">
            <Columns>

                <asp:BoundField DataField="Id" HeaderText="hostID" SortExpression="Id" Visible="False"/>
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-CssClass="width_200"/>
                <asp:BoundField DataField="Mac" HeaderText="MAC" SortExpression="Mac" ItemStyle-CssClass="width_200 mobi-hide-smallest" HeaderStyle-CssClass="mobi-hide-smallest"/>
                <asp:BoundField DataField="Group" HeaderText="Group" SortExpression="Group" ItemStyle-CssClass="mobi-hide-smaller" HeaderStyle-CssClass="mobi-hide-smaller"/>
            </Columns>
            <EmptyDataTemplate>
                No Hosts Found
            </EmptyDataTemplate>
        </asp:GridView>

    </div>

</asp:Content>