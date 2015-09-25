<%@ Page Title="" Language="C#" MasterPageFile="~/views/tasks/Task.master" AutoEventWireup="true" Inherits="views.tasks.TaskUnicast" CodeFile="hosts.aspx.cs" %>

<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#taskunicast').addClass("nav-current");
        });
    </script>
    <div class="size-7 column">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="searchbox" OnTextChanged="search_Changed"></asp:TextBox>
    </div>
    <br class="clear"/>
    <p class="total">
        <asp:Label ID="lblTotal" runat="server"></asp:Label>
    </p>
    <asp:GridView ID="gvHosts" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" CssClass="Gridview" AllowSorting="True" OnSorting="gridView_Sorting" AlternatingRowStyle-CssClass="alt">
        <Columns>
            <asp:BoundField DataField="Id" HeaderText="hostID" InsertVisible="False" ReadOnly="True" SortExpression="Id" Visible="False"/>
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-CssClass="width_200"/>
            <asp:BoundField DataField="Image" HeaderText="Image" SortExpression="Image" ItemStyle-CssClass="width_200 mobi-hide-smaller" HeaderStyle-CssClass="mobi-hide-smaller"/>
            <asp:BoundField DataField="Group" HeaderText="Group" SortExpression="Group" ItemStyle-CssClass="width_200 mobi-hide-small" HeaderStyle-CssClass="mobi-hide-small"/>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton ID="btnDeploy" runat="server" OnClick="btnDeploy_Click" Text="Deploy"/>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="Upload"/>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            No Hosts Found
        </EmptyDataTemplate>
    </asp:GridView>
    <div id="confirmbox" class="confirm-box-outer">
        <div class="confirm-box-inner">
            <h4>
                <asp:Label ID="lblTitle" runat="server" CssClass="modaltitle"></asp:Label>
            </h4>
            <asp:GridView ID="gvConfirm" runat="server" CssClass="Gridview gv-confirm " AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="Name" HeaderText="Name"/>
                    <asp:BoundField DataField="Mac" HeaderText="MAC" ItemStyle-CssClass="mobi-hide-smallest" HeaderStyle-CssClass="mobi-hide-smallest"/>
                    <asp:BoundField DataField="Image" HeaderText="Image"/>
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