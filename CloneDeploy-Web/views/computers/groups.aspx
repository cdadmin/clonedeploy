<%@ Page Title="" Language="C#" MasterPageFile="~/views/computers/computers.master" AutoEventWireup="true" Inherits="views_computers_groups" Codebehind="groups.aspx.cs" %>

<asp:Content ID="Breadcrumb" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
   <li><a href="<%= ResolveUrl("~/views/computers/edit.aspx") %>?computerid=<%= Computer.Id %>" ><%= Computer.Name %></a></li>
    <li>Group Membership</li>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Help">
  <li role="separator" class="divider"></li>
    <li><a href="<%= ResolveUrl("~/views/help/computers-group.aspx")%>" class="" target="_blank">Help</a></li>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="SubPageActionsRight">
     <button type="button" class="btn btn-default dropdown-toggle width_140" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" >
    <span class="caret"></span>
  </button>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="SubContent" Runat="Server">
     <script type="text/javascript">
        $(document).ready(function() {
            $('#group').addClass("nav-current");
            $("[id*=gvGroups] td").hover(function () {
                $("td", $(this).closest("tr")).addClass("hover_row");
            }, function () {
                $("td", $(this).closest("tr")).removeClass("hover_row");
            });
        });
    </script>
   
   
     <asp:GridView ID="gvGroups" runat="server"  DataKeyNames="Id" AutoGenerateColumns="False" CssClass="Gridview" AlternatingRowStyle-CssClass="alt">
        <Columns>
           
        
                
             
              <asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="~/views/groups/edit.aspx?groupid={0}" Text="View" ItemStyle-CssClass="chkboxwidth" Target="_blank"/>
          
            <asp:BoundField DataField="Id" HeaderText="ID" SortExpression="Id" Visible="False"/>
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ></asp:BoundField>
           
           
             
            
          
        </Columns>
        <EmptyDataTemplate>
            No Groups Found
        </EmptyDataTemplate>
    </asp:GridView>
   
   
</asp:Content>
