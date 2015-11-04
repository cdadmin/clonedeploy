<%@ Page Title="" Language="C#" MasterPageFile="~/views/tasks/Task.master" AutoEventWireup="true" Inherits="views.tasks.TaskMulticast" CodeFile="groups.aspx.cs" %>

<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#taskmulticast').addClass("nav-current");
        });
    </script>
    <div class="size-7 column">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="searchbox"></asp:TextBox>
    </div>
    <br class="clear"/>
    <p class="total">
        <asp:Label ID="lblTotal" runat="server"></asp:Label>
    </p>
    <asp:GridView ID="gvGroups" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" CssClass="Gridview" AlternatingRowStyle-CssClass="alt">
        <Columns>
            <asp:BoundField DataField="Id" HeaderText="groupID" InsertVisible="False" ReadOnly="True" SortExpression="groupID" Visible="False"/>
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-CssClass="width_200"/>
           
            <asp:TemplateField ItemStyle-CssClass="width_100">
                <ItemTemplate>
                    <asp:LinkButton ID="btnMulticast" runat="server" OnClick="btnMulticast_Click" Text="Multicast"/>
                    <asp:LinkButton ID="btnUnicast" runat="server" OnClick="btnUnicast_Click" Text="Unicast"/>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-CssClass="width_100">
                <ItemTemplate>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <div id="confirmbox" class="confirm-box-outer">
        <div class="confirm-box-inner">
            <h4>
                <asp:Label ID="lblTitle" runat="server" CssClass="modaltitle"></asp:Label>
            </h4>
            <asp:GridView ID="gvConfirm" runat="server" CssClass="Gridview gv-confirm" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="_groupname" HeaderText="Name" ItemStyle-CssClass="width_200"/>
                    <asp:BoundField DataField="_groupimage" HeaderText="Image" ItemStyle-CssClass="width_200"/>
                </Columns>
            </asp:GridView>
            <div class="confirm-box-btns">
                <asp:LinkButton ID="OkButton" OnClick="btnConfirm_Click" runat="server" Text="Yes" CssClass="confirm_yes"/>
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