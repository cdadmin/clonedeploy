﻿<%@ Page Title="" Language="C#" MasterPageFile="~/views/tasks/task.master" AutoEventWireup="true" CodeBehind="activeond.aspx.cs" Inherits="CloneDeploy_Web.views.tasks.activeond" %>
<asp:Content ID="Breadcrumb" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li>Active Unregistered On Demand Tasks</li>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Help">
    <li role="separator" class="divider"></li>
    <li>
        <a href="<%= ResolveUrl("~/views/help/tasks-ond.aspx") %>" target="_blank">Help</a>
    </li>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="SubPageActionsRight">
  <button type="button" class="btn btn-default dropdown-toggle width_140" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        <span class="caret"></span>
    </button>
</asp:Content>

<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#unregond').addClass("nav-current");
        });
    </script>
    

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Timer ID="Timer" runat="server" Interval="2000" OnTick="Timer_Tick">
            </asp:Timer>
            <p class="total">
                <asp:Label ID="lblTotal" runat="server"></asp:Label>
            </p>
            <br class="clear"/>
            <p class="total">
                <asp:Label ID="lblTotalNotOwned" runat="server"></asp:Label>
            </p>
            <br class="clear"/>
            <br/>
            <asp:GridView ID="gvTasks" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" CssClass="Gridview extraPad" AlternatingRowStyle-CssClass="alt">
                <Columns>
                    <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="chkboxwidth">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnCancel" runat="server" CausesValidation="false" CommandName="" Text="Cancel" OnClick="btnCancel_Click"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Id" HeaderText="taskID" SortExpression="_taskID" InsertVisible="False" ReadOnly="True" Visible="False"/>
                    <asp:BoundField DataField="Arguments" HeaderText="Mac" SortExpression="Arguments" ItemStyle-CssClass="width_50"/>
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