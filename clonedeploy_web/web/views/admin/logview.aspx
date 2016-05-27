<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/admin.master" AutoEventWireup="true" Inherits="views.admin.Logview" CodeFile="logview.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li>Logs</li>
</asp:Content>

<asp:Content runat="server" ID="Help" ContentPlaceHolderID="Help">
      <a href="<%= ResolveUrl("~/views/help/index.html")%>"  target="_blank">Help</a>
</asp:Content>

<asp:Content runat="server" ID="ActionsRight" ContentPlaceHolderID="SubPageActionsRight">
   <asp:LinkButton ID="btnExportLog" runat="server" Text="Export Log"  OnClick="btnExportLog_Click"></asp:LinkButton>
</asp:Content>

<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#log').addClass("nav-current");
            $("[id*=gvLogs] td").hover(function () {
                $("td", $(this).closest("tr")).addClass("hover_row");
            }, function () {
                $("td", $(this).closest("tr")).removeClass("hover_row");
            });
        });
    </script>
    <div class="size-7 column">
        <asp:DropDownList ID="ddlLog" runat="server" CssClass="ddlist" AutoPostBack="True">
        </asp:DropDownList>
    </div>
    <br class="clear"/>
    
    <div id="fileView" runat="server" Visible="False">
    <div class="size-4 column" style="float: right; margin: 0;">
        <asp:DropDownList ID="ddlLimit" runat="server" CssClass="ddlist" Style="float: right; width: 75px;" AutoPostBack="true" OnSelectedIndexChanged="ddlLimit_SelectedIndexChanged">
            <asp:ListItem>10</asp:ListItem>
            <asp:ListItem>25</asp:ListItem>
            <asp:ListItem>50</asp:ListItem>
            <asp:ListItem>100</asp:ListItem>
            <asp:ListItem>All</asp:ListItem>
        </asp:DropDownList>
        <br class="clear"/>
        
    </div>
    <br class="clear"/>
    <asp:GridView ID="GridView1" runat="server" CssClass="Gridview log" ShowHeader="false">
    </asp:GridView>
        </div>
    
    <div id="dbView" runat="server" Visible="False">
        <asp:DropDownList ID="ddlDbLimit" runat="server" CssClass="ddlist" Style="float: right; width: 75px;" AutoPostBack="true" OnSelectedIndexChanged="ddlDbLimit_OnSelectedIndexChanged">
            <asp:ListItem>25</asp:ListItem>
            <asp:ListItem>50</asp:ListItem>
            <asp:ListItem>100</asp:ListItem>
            <asp:ListItem>250</asp:ListItem>
            <asp:ListItem>1000</asp:ListItem>
            <asp:ListItem>All</asp:ListItem>
        </asp:DropDownList>
        
         <asp:GridView ID="gvLogs" runat="server" DataKeyNames="Id"  AutoGenerateColumns="False" CssClass="Gridview extraPad" AlternatingRowStyle-CssClass="alt">
        <Columns>
               
            <asp:TemplateField>
                <HeaderStyle CssClass="chkboxwidth"></HeaderStyle>
                <ItemStyle CssClass="chkboxwidth"></ItemStyle>
                <ItemTemplate>
                    <asp:LinkButton ID="btnView" runat="server" OnClick="btnView_OnClick" Text="View"/>
                </ItemTemplate>
            </asp:TemplateField>
              <asp:TemplateField>
                     <ItemStyle CssClass="chkboxwidth"></ItemStyle>
                <ItemTemplate>
                    <asp:LinkButton ID="btnExport" runat="server" OnClick="btnExport_OnClick" Text="Export"/>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Id" Visible="False"/>
            <asp:BoundField DataField="Mac" HeaderText="Mac" SortExpression="Mac" ItemStyle-CssClass="width_200"></asp:BoundField>
            <asp:BoundField DataField="LogTime" HeaderText="Time" SortExpression="LogTime" ItemStyle-CssClass="width_200"></asp:BoundField>
            <asp:BoundField DataField="SubType" HeaderText="Type" SortExpression="SubType" />
          
            
       
             </Columns>
        <EmptyDataTemplate>
            No Logs Found
        </EmptyDataTemplate>
    </asp:GridView>
        
      
    </div>
       <div id="ViewLog" runat="server" Visible="False">
       <asp:GridView ID="gvLogView" runat="server" CssClass="Gridview log" ShowHeader="false">
    </asp:GridView>
             </div>

</asp:Content>