<%@ Page Title="" Language="C#" MasterPageFile="~/views/tasks/task.master" AutoEventWireup="true" Inherits="views.tasks.TaskMulticast" CodeFile="groups.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
     <li>Start Group Task</li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Help" Runat="Server">
     <li role="separator" class="divider"></li>
    <li><a href="<%= ResolveUrl("~/views/help/tasks-startgroup.aspx")%>"  target="_blank">Help</a></li>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="SubPageActionsRight">
     
     <button type="button" class="btn btn-default dropdown-toggle width_140" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
    <span class="caret"></span>
  </button>
</asp:Content>

<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#group').addClass("nav-current");
        });
    </script>
    <p class="total">
        <asp:Label ID="lblTotal" runat="server"></asp:Label>
    </p>
    <div class="size-7 column">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="searchbox"></asp:TextBox>
    </div>
    <br class="clear"/>
    
    <asp:GridView ID="gvGroups" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" CssClass="Gridview extraPad" AlternatingRowStyle-CssClass="alt">
        <Columns>
               <asp:TemplateField ItemStyle-CssClass="chkboxwidth">
                <ItemTemplate >
                    <asp:LinkButton ID="btnMulticast" runat="server" OnClick="btnMulticast_Click" Text="Multicast"/>

                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField ItemStyle-CssClass="chkboxwidth">
                <ItemTemplate>
                    <asp:LinkButton ID="btnUnicast" runat="server" OnClick="btnUnicast_Click" Text="Unicast"/>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Id" HeaderText="groupID" InsertVisible="False" ReadOnly="True" SortExpression="groupID" Visible="False"/>
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-CssClass="width_200"/>
             <asp:TemplateField ItemStyle-CssClass="width_250">
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