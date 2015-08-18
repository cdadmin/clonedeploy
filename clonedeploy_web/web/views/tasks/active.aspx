<%@ Page Title="" Language="C#" MasterPageFile="~/views/masters/Task.master" AutoEventWireup="true" Inherits="views.tasks.TaskActive" CodeFile="active.aspx.cs" %>

<%@ MasterType VirtualPath="~/views/masters/Task.master" %>
<%@ Reference virtualPath="~/views/masters/Site.master" %>

<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#taskactive').addClass("nav-current");
        });
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel" runat="server">
        <ContentTemplate>
            <asp:Timer ID="TimerMC" runat="server" Interval="2000" OnTick="TimerMC_Tick">
            </asp:Timer>
            <h2>Multicasts</h2>
            <asp:GridView ID="gvMcTasks" runat="server" CssClass="Gridview" AutoGenerateColumns="False" DataKeyNames="Id" AlternatingRowStyle-CssClass="alt">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="mcTaskID" InsertVisible="False" ReadOnly="True" SortExpression="Id" Visible="False"/>
                    <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="width_30">
                        <ItemTemplate>
                            <div style="width: 0">
                                <asp:LinkButton ID="btnMembers" runat="server" CausesValidation="false" CommandName="" Text="+" OnClick="btnMembers_Click"></asp:LinkButton>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="mcTaskName"/>
                    <asp:BoundField DataField="Pid" HeaderText="PID" SortExpression="mcPID" ItemStyle-CssClass="mobi-hide-smallest" HeaderStyle-CssClass="mobi-hide-smallest"/>
                    <asp:TemplateField ShowHeader="True" HeaderText="Partition" ItemStyle-CssClass="mobi-hide-smaller" HeaderStyle-CssClass="mobi-hide-smaller">
                        <ItemTemplate>
                            <asp:Label ID="lblPartition" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="True" HeaderText="Elapsed" ItemStyle-CssClass="mobi-hide-small" HeaderStyle-CssClass="mobi-hide-small">
                        <ItemTemplate>
                            <asp:Label ID="lblElapsed" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="True" HeaderText="Remaining" ItemStyle-CssClass="mobi-hide-small" HeaderStyle-CssClass="mobi-hide-small">
                        <ItemTemplate>
                            <asp:Label ID="lblRemaining" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="True" HeaderText="Completed">
                        <ItemTemplate>
                            <asp:Label ID="lblCompleted" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="True" HeaderText="Rate" ItemStyle-CssClass="mobi-hide-smallest" HeaderStyle-CssClass="mobi-hide-smallest">
                        <ItemTemplate>
                            <asp:Label ID="lblRate" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnCancelMc" runat="server" CausesValidation="false" CommandName="" Text="Cancel" OnClick="btnCancelMc_Click"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <tr>
                                <td runat="server" id="tdMembers" colspan="700" visible="false">
                                    <asp:GridView ID="gvMembers" AutoGenerateColumns="false" runat="server" CssClass="Gridview gv_members" ShowHeader="false" Visible="false" AlternatingRowStyle-CssClass="alt">
                                        <Columns>
                                            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-CssClass="width_150"/>
                                            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" ItemStyle-CssClass="width_50"/>
                                            <asp:BoundField DataField="Partition" HeaderText="Partition" ItemStyle-CssClass="mobi-hide-smaller" HeaderStyle-CssClass="mobi-hide-smaller"/>
                                            <asp:BoundField DataField="Elapsed" HeaderText="Elapsed" ItemStyle-CssClass="mobi-hide-small" HeaderStyle-CssClass="mobi-hide-small"/>
                                            <asp:BoundField DataField="Remaining" HeaderText="Remaining" ItemStyle-CssClass="mobi-hide-small" HeaderStyle-CssClass="mobi-hide-small"/>
                                            <asp:BoundField DataField="Completed" HeaderText="Completed"/>
                                            <asp:BoundField DataField="Rate" HeaderText="Rate" ItemStyle-CssClass="mobi-hide-smallest" HeaderStyle-CssClass="mobi-hide-smallest"/>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    No Active Multicasts
                </EmptyDataTemplate>
                <HeaderStyle CssClass="taskgridheader"></HeaderStyle>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Timer ID="Timer" runat="server" Interval="2000" OnTick="Timer_Tick">
            </asp:Timer>
            <h2>Unicasts</h2>
            <asp:GridView ID="gvUcTasks" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" CssClass="Gridview" AlternatingRowStyle-CssClass="alt">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="taskID" SortExpression="Id" InsertVisible="False" ReadOnly="True" Visible="False"/>
                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-CssClass="width_150"/>
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
            <h2>All Tasks</h2>
            <asp:LinkButton ID="btnShowAll" runat="server" CausesValidation="false" CommandName="" Text="Show / Hide" OnClick="btnShowAll_Click" CssClass="submits left"></asp:LinkButton>
            <br class="clear"/>
            <asp:GridView ID="gvTasks" Visible="false" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" CssClass="Gridview" AlternatingRowStyle-CssClass="alt">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="taskID" SortExpression="_taskID" InsertVisible="False" ReadOnly="True" Visible="False"/>
                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="_taskName" ItemStyle-CssClass="width_150"/>
                    <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="_taskStatus" ItemStyle-CssClass="width_50"/>
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
                    No Active Tasks
                </EmptyDataTemplate>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:LinkButton ID="cancelTasks" runat="server" Text="Cancel All Tasks" CssClass="submits" OnClick="cancelTasks_Click"/>
</asp:Content>