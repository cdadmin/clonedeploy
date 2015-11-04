<%@ Page Title="" Language="C#" MasterPageFile="~/views/tasks/Task.master" AutoEventWireup="true" CodeFile="activeunicast.aspx.cs" Inherits="views_tasks_activeunicast" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Help" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ActionsLeftSub" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="SubPageActionsRight" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="SubContent" Runat="Server">
     <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
      <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Timer ID="Timer" runat="server" Interval="2000" OnTick="Timer_Tick">
            </asp:Timer>
            <asp:GridView ID="gvUcTasks" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" CssClass="Gridview" AlternatingRowStyle-CssClass="alt">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="taskID" SortExpression="Id" InsertVisible="False" ReadOnly="True" Visible="False"/>
                    <asp:BoundField DataField="ComputerId" HeaderText="Name" SortExpression="Name" ItemStyle-CssClass="width_150"/>
                    <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" ItemStyle-CssClass="width_50"/>
                    <asp:BoundField DataField="Partition" HeaderText="Partition" ItemStyle-CssClass="mobi-hide-smaller" HeaderStyle-CssClass="mobi-hide-smaller"/>
                    <asp:BoundField DataField="Elapsed" HeaderText="Elapsed" ItemStyle-CssClass="mobi-hide-small" HeaderStyle-CssClass="mobi-hide-small"/>
                    <asp:BoundField DataField="Remaining" HeaderText="Remaining" ItemStyle-CssClass="mobi-hide-small" HeaderStyle-CssClass="mobi-hide-small"/>
                    <asp:BoundField DataField="Completed" HeaderText="Completed"/>
                    <asp:BoundField DataField="Rate" HeaderText="Rate" ItemStyle-CssClass="mobi-hide-smallest" HeaderStyle-CssClass="mobi-hide-smallest"/>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnCancel" runat="server" CausesValidation="false" CommandName="" Text="Cancel" OnClick="btnCancel_Click"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    No Active Unicasts
                </EmptyDataTemplate>
            </asp:GridView>
           
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

