<%@ Page Title="" Language="C#" MasterPageFile="~/views/masters/User.master" AutoEventWireup="true" Inherits="views.users.EditUser" CodeFile="edit.aspx.cs" %>

<%@ MasterType VirtualPath="~/views/masters/User.master" %>
<%@ Reference virtualPath="~/views/masters/Site.master" %>

<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">

    <script type="text/javascript">
        $(document).ready(function() {
            $('#editoption').addClass("nav-current");
        });
    </script>
    <div class="size-4 column">
        User Name:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtUserName" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        User Membership:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddluserMembership" runat="server" CssClass="ddlist" OnSelectedIndexChanged="ddluserMembership_SelectedIndexChanged" AutoPostBack="true">
            <asp:ListItem>Administrator</asp:ListItem>
            <asp:ListItem>Power User</asp:ListItem>
            <asp:ListItem>User</asp:ListItem>
        </asp:DropDownList>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        User Password:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtUserPwd" runat="server" CssClass="textbox" TextMode="Password"></asp:TextBox>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Confirm Password:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtUserPwdConfirm" runat="server" CssClass="textbox" TextMode="Password"></asp:TextBox>
    </div>
    <br class="clear"/>
    <div id="permissions" runat="server" visible="false">
        <div class="size-4 column">
            On Demand Access:
        </div>
        <div class="size-5 column">
            <asp:CheckBox ID="chkOnd" runat="server"/>
        </div>
        <br class="clear"/>
        <div class="size-4 column">
            Debug Access:
        </div>
        <div class="size-5 column">
            <asp:CheckBox ID="chkDebug" runat="server"/>
        </div>
        <br class="clear"/>
        <div class="size-4 column">
            Diagnostics Access:
        </div>
        <div class="size-5 column">
            <asp:CheckBox ID="chkDiag" runat="server"/>
        </div>
        <br class="clear"/>
    </div>
    <div class="size-4 column">
        &nbsp;
    </div>
    <div class="size-5 column">
        <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Update User" CssClass="submits"/>
    </div>
    <br class="clear"/>
    <div id="management" runat="server" visible="false">
        <h4>Group Management:</h4>
        <asp:GridView ID="gvGroups" runat="server" AllowSorting="true" AutoGenerateColumns="False" OnSorting="gridView_Sorting" CssClass="Gridview" DataKeyNames="groupID" AlternatingRowStyle-CssClass="alt">
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
                <asp:BoundField DataField="groupID" HeaderText="groupID" InsertVisible="False" SortExpression="groupID" Visible="False"/>
                <asp:BoundField DataField="groupName" HeaderText="Name" SortExpression="groupName" ItemStyle-CssClass="width_200"/>
                <asp:BoundField DataField="groupImage" HeaderText="Image" SortExpression="groupImage" ItemStyle-CssClass="width_200 mobi-hide-smallest" HeaderStyle-CssClass="mobi-hide-smallest"/>
            </Columns>
            <EmptyDataTemplate>
                No Groups Found
            </EmptyDataTemplate>
        </asp:GridView>
    </div>


</asp:Content>