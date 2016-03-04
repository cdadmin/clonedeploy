<%@ Page Title="" Language="C#" MasterPageFile="~/views/tasks/task.master" AutoEventWireup="true" Inherits="views.tasks.TaskActive" CodeFile="active.aspx.cs" %>

<asp:Content ID="Breadcrumb" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li>All Active Tasks</li>
    </asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Help">
     <a href="<%= ResolveUrl("~/views/help/index.html")%>" class="submits help" target="_blank"></a>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="SubPageActionsRight">
     <asp:LinkButton ID="cancelTasks" runat="server" Text="Cancel All Tasks" CssClass="submits actions green" OnClick="cancelTasks_Click"/>
</asp:Content>

<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#all').addClass("nav-current");
        });
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Timer ID="Timer" runat="server" Interval="2000" OnTick="Timer_Tick">
            </asp:Timer>
              <p class="total">
        <asp:Label ID="lblTotal" runat="server"></asp:Label>
    </p>
    <br class="clear" />
    <br/>
            <asp:GridView ID="gvTasks" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" CssClass="Gridview extraPad" AlternatingRowStyle-CssClass="alt">
                <Columns>
                    <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="chkboxwidth">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnCancel" runat="server" CausesValidation="false" CommandName="" Text="Cancel" OnClick="btnCancel_Click"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Id" HeaderText="taskID" SortExpression="_taskID" InsertVisible="False" ReadOnly="True" Visible="False"/>
                     <asp:TemplateField ItemStyle-CssClass="width_150" HeaderText="Name">
                <ItemTemplate>
                    <asp:Label ID="lblComputer" runat="server" Text='<%# Bind("Computer.Name") %>'/>
                </ItemTemplate>
            </asp:TemplateField>
                    <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="_taskStatus" ItemStyle-CssClass="width_50"/>
                    <asp:BoundField DataField="Partition" HeaderText="Partition" ItemStyle-CssClass="mobi-hide-smaller" HeaderStyle-CssClass="mobi-hide-smaller"/>
                    <asp:BoundField DataField="Elapsed" HeaderText="Elapsed" ItemStyle-CssClass="mobi-hide-small" HeaderStyle-CssClass="mobi-hide-small"/>
                    <asp:BoundField DataField="Remaining" HeaderText="Remaining" ItemStyle-CssClass="mobi-hide-small" HeaderStyle-CssClass="mobi-hide-small"/>
                    <asp:BoundField DataField="Completed" HeaderText="Completed"/>
                    <asp:BoundField DataField="Rate" HeaderText="Rate" ItemStyle-CssClass="mobi-hide-smallest" HeaderStyle-CssClass="mobi-hide-smallest"/>
                    
                </Columns>
                <EmptyDataTemplate>
                    No Active Tasks
                </EmptyDataTemplate>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
   
</asp:Content>