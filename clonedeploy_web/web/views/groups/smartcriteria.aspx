<%@ Page Title="" Language="C#" MasterPageFile="~/views/groups/groups.master" AutoEventWireup="true" CodeFile="smartcriteria.aspx.cs" Inherits="views_groups_smartcriteria" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
      <li><a href="<%= ResolveUrl("~/views/groups/edit.aspx") %>?groupid=<%= Group.Id %>" ><%= Group.Name %></a></li>
    <li>Smart Criteria</li>
</asp:Content>

    <asp:Content runat="server" ContentPlaceHolderID="Help">
     <a href="<%= ResolveUrl("~/views/help/index.html")%>" class="submits actions" target="_blank">Help</a>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="SubPageActionsRight">
   <asp:LinkButton ID="btnUpdate" runat="server" Text="Update Criteria" OnClick="btnUpdate_OnClick" CssClass="submits actions"/>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="SubContent" Runat="Server">
      <script type="text/javascript">
        $(document).ready(function() {
            $('#smart').addClass("nav-current");
        });
    </script>

      <div class="size-4 column">
        Where Computer Name Contains:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtContains" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <div class="size-5 column">
        <asp:LinkButton ID="btnTestQuery" runat="server" Text="Test Query" OnClick="btnTestQuery_OnClick" CssClass="submits"/>
    </div>
    <br class="clear"/>
        
    <p class="total">
        <asp:Label ID="lblTotal" runat="server"></asp:Label>
    </p>
    <asp:GridView ID="gvComputers" runat="server" AllowSorting="True" DataKeyNames="Id" OnSorting="gvComputers_OnSorting" AutoGenerateColumns="False" CssClass="Gridview" AlternatingRowStyle-CssClass="alt">
        <Columns>
            <asp:BoundField DataField="Id" HeaderText="computerID" SortExpression="computerID" Visible="False"/>
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-CssClass="width_200"></asp:BoundField>
            <asp:BoundField DataField="Mac" HeaderText="MAC" SortExpression="Mac" ItemStyle-CssClass="width_200 mobi-hide-smallest" HeaderStyle-CssClass="mobi-hide-smallest"/>
        </Columns>
        <EmptyDataTemplate>
            No Computers Found
        </EmptyDataTemplate>
    </asp:GridView>
   
</asp:Content>

