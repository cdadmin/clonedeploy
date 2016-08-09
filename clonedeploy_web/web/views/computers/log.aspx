<%@ Page Title="" Language="C#" MasterPageFile="~/views/computers/computers.master" AutoEventWireup="true" CodeFile="log.aspx.cs" Inherits="views.computers.ComputerLog" EnableEventValidation="false"%>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li><a href="<%= ResolveUrl("~/views/computers/edit.aspx") %>?computerid=<%= Computer.Id %>" ><%= Computer.Name %></a></li>
    <li>Logs</li>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Help">
    <li role="separator" class="divider"></li>
    <li><a href="<%= ResolveUrl("~/views/help/index.html")%>" target="_blank">Help</a></li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SubContent" Runat="Server">
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
  
    <div id="SearchLogs" runat="server">
    <p class="total">
        <asp:Label ID="lblTotal" runat="server"></asp:Label>
    </p>
    <asp:GridView ID="gvLogs" runat="server" DataKeyNames="Id"  AutoGenerateColumns="False" CssClass="Gridview extraPad" AlternatingRowStyle-CssClass="alt">
        <Columns>
                 <asp:TemplateField>
                      
                <ItemStyle CssClass="chkboxwidth"></ItemStyle>
                <ItemTemplate>
                  
                      <asp:LinkButton ID="btnExport" runat="server" OnClick="btnExport_OnClick" Text="Export"/>
                 
                   
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderStyle CssClass="width_200"></HeaderStyle>
                <ItemStyle CssClass="width_200"></ItemStyle>
                <ItemTemplate>
                    <asp:LinkButton ID="btnView" runat="server" OnClick="btnView_OnClick" Text="View"/>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Id" Visible="False" />
            <asp:BoundField DataField="LogTime" HeaderText="Time" SortExpression="LogTime" ItemStyle-CssClass="width_200"></asp:BoundField>
            <asp:BoundField DataField="SubType" HeaderText="Type" SortExpression="SubType" ItemStyle-CssClass="mobi-hide-smallest" HeaderStyle-CssClass="mobi-hide-smallest"/>
          
            
       
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