<%@ Page Title="" Language="C#" MasterPageFile="~/views/tasks/Task.master" AutoEventWireup="true" Inherits="views.tasks.TaskUnicast" CodeFile="computers.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
     <li>Start Computer Task</li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Help" Runat="Server">
    <a href="<%= ResolveUrl("~/views/help/index.html")%>" class="submits help" target="_blank"></a>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SubPageActionsRight" Runat="Server">
</asp:Content>
<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#computer').addClass("nav-current");
        });
    </script>
      <p class="total">
        <asp:Label ID="lblTotal" runat="server"></asp:Label>
    </p>
    <div class="size-7 column">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="searchbox" OnTextChanged="search_Changed"></asp:TextBox>
    </div>
    <br class="clear"/>
  
    <asp:GridView ID="gvComputers" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" CssClass="Gridview extraPad" AllowSorting="True" OnSorting="gridView_Sorting" AlternatingRowStyle-CssClass="alt">
        <Columns>
             <asp:TemplateField>

                  <ItemStyle CssClass="chkboxwidth"></ItemStyle>
                <ItemTemplate>
                    <asp:LinkButton ID="btnDeploy" runat="server" OnClick="btnDeploy_Click" Text="Deploy"/>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                 <ItemStyle CssClass="chkboxwidth"></ItemStyle>
                <ItemTemplate>
                    <asp:LinkButton ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="Upload"/>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Id" HeaderText="computerID" InsertVisible="False" ReadOnly="True" SortExpression="Id" Visible="False"/>
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-CssClass="width_200"/>
            <asp:BoundField DataField="Image.Name" HeaderText="Image" SortExpression="Image" ItemStyle-CssClass="width_200 mobi-hide-smaller" HeaderStyle-CssClass="mobi-hide-smaller"/>
            <asp:TemplateField ItemStyle-CssClass="width_250">
                <ItemTemplate>
                </ItemTemplate>
            </asp:TemplateField>
           
        </Columns>
        <EmptyDataTemplate>
            No Computers Found
        </EmptyDataTemplate>
    </asp:GridView>
    <div id="confirmbox" class="confirm-box-outer">
        <div class="confirm-box-inner">
            <h4>
                <asp:Label ID="lblTitle" runat="server" CssClass="modaltitle"></asp:Label>
            </h4>
            <asp:GridView ID="gvConfirm" runat="server" CssClass="Gridview gv-confirm" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="Name" HeaderText="Name"/>
                    <asp:BoundField DataField="Mac" HeaderText="MAC" ItemStyle-CssClass="mobi-hide-smallest" HeaderStyle-CssClass="mobi-hide-smallest"/>
                    <asp:BoundField DataField="Image.Name" HeaderText="Image"/>
                </Columns>
            </asp:GridView>
            <div class="confirm-box-btns">
                <asp:LinkButton ID="OkButton" OnClick="OkButton_Click" runat="server" Text="Yes" CssClass="confirm_yes"/>
                <asp:LinkButton ID="CancelButton" runat="server" Text="No" CssClass="confirm_no"/>
            </div>
        </div>
    </div>
    <div id="incorrectChecksum" class="confirm-box-outer">
        <div class="confirm-box-inner">
            <h4>
                <asp:Label ID="lblIncorrectChecksum" runat="server" CssClass="modaltitle"></asp:Label>
            </h4>

            <div class="confirm-box-btns">
                <asp:LinkButton ID="LinkButton1" OnClick="OkButtonChecksum_Click" runat="server" Text="Yes" CssClass="confirm_yes"/>
                <asp:LinkButton ID="LinkButton2" runat="server" Text="No" CssClass="confirm_no"/>
            </div>

        </div>
    </div>
</asp:Content>