<%@ Page Title="" Language="C#" MasterPageFile="~/views/tasks/task.master" AutoEventWireup="true" CodeFile="activeunicast.aspx.cs" Inherits="views_tasks_activeunicast" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
     <li>Active Unicasts</li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Help" Runat="Server">
     <li role="separator" class="divider"></li>
    <li><a href="<%= ResolveUrl("~/views/help/tasks-activeunicast.aspx")%>"  target="_blank">Help</a></li>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="SubPageActionsRight">
     
     <button type="button" class="btn btn-default dropdown-toggle width_140" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
    <span class="caret"></span>
  </button>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="SubContent" Runat="Server">
     <script type="text/javascript">
        $(document).ready(function() {
            $('#unicast').addClass("nav-current");
        
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
            <asp:GridView ID="gvUcTasks" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" CssClass="Gridview extraPad" AlternatingRowStyle-CssClass="alt">
                <Columns>
                     <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="chkboxwidth">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnCancel" runat="server" CausesValidation="false" CommandName="" Text="Cancel" OnClick="btnCancel_Click"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Id" HeaderText="taskID" SortExpression="Id" InsertVisible="False" ReadOnly="True" Visible="False"/>
                      <asp:TemplateField ItemStyle-CssClass="width_150 mobi-hide-smaller" HeaderText="Name">
                <ItemTemplate>
                    <asp:Label ID="lblComputer" runat="server" Text='<%# Bind("Computer.Name") %>'/>
                </ItemTemplate>
            </asp:TemplateField>
                    <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" ItemStyle-CssClass="width_50"/>
                    <asp:BoundField DataField="Partition" HeaderText="Partition" ItemStyle-CssClass="mobi-hide-smaller" HeaderStyle-CssClass="mobi-hide-smaller"/>
                    <asp:BoundField DataField="Elapsed" HeaderText="Elapsed" ItemStyle-CssClass="mobi-hide-small" HeaderStyle-CssClass="mobi-hide-small"/>
                    <asp:BoundField DataField="Remaining" HeaderText="Remaining" ItemStyle-CssClass="mobi-hide-small" HeaderStyle-CssClass="mobi-hide-small"/>
                    <asp:BoundField DataField="Completed" HeaderText="Completed"/>
                    <asp:BoundField DataField="Rate" HeaderText="Rate" ItemStyle-CssClass="mobi-hide-smallest" HeaderStyle-CssClass="mobi-hide-smallest"/>
                   
                </Columns>
                <EmptyDataTemplate>
                    No Active Unicasts
                </EmptyDataTemplate>
            </asp:GridView>
           
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

