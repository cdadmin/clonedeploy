<%@ Page Title="" Language="C#" MasterPageFile="~/views/global/global.master" AutoEventWireup="true" CodeFile="kernel.aspx.cs" Inherits="views_global_profileupdater_kernel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub" runat="Server">
    <li>Image Profile Updater</li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Help" Runat="Server">
    <a href="<%= ResolveUrl("~/views/help/index.html") %>"   target="_blank">Help</a>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="SubPageActionsRight">
       <asp:LinkButton ID="btnUpdateKernel" runat="server" OnClick="btnUpdateKernel_OnClick" Text="Update Image Profiles" CssClass="btn btn-default" />
     <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
    <span class="caret"></span>
  </button>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#kernel').addClass("nav-current");
        });
    </script>
  
    <div class="size-4 column">
            New Kernel:
        </div>
        <div class="size-5 column">
            <asp:DropDownList ID="ddlKernel" runat="server" CssClass="ddlist">
            </asp:DropDownList>
        </div>
        <br class="clear"/>
    <br/>
     <asp:GridView ID="gvProfiles" runat="server" DataKeyNames="Id" AutoGenerateColumns="False" CssClass="Gridview" AlternatingRowStyle-CssClass="alt">
        <Columns>
            
            <asp:TemplateField>
                <HeaderStyle CssClass="chkboxwidth"></HeaderStyle>
                <ItemStyle CssClass="chkboxwidth"></ItemStyle>
                <HeaderTemplate>
                    <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelectAll_CheckedChanged"/>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="chkSelector" runat="server"/>
                </ItemTemplate>
            </asp:TemplateField>
              
             <asp:HyperLinkField DataNavigateUrlFields="Id,ImageId" DataNavigateUrlFormatString="~/views/images/profiles/general.aspx?imageid={1}&profileid={0}&cat=profiles" Text="View" ItemStyle-CssClass="chkboxwidth" Target="_blank"/>
             
            <asp:BoundField DataField="Id" HeaderText="ID" SortExpression="ID" Visible="False"/>
            <asp:BoundField DataField="ImageId" HeaderText="ImageID" SortExpression="ImageID" Visible="False"/>
          
                <asp:TemplateField ItemStyle-CssClass="mobi-hide-smaller" HeaderStyle-CssClass="width_200 mobi-hide-smaller" HeaderText="Image">
                <ItemTemplate>
                    <asp:Label ID="lblImage" runat="server" Text='<%# Bind("Image.Name") %>'/>
                </ItemTemplate>
            </asp:TemplateField>
          
              <asp:BoundField DataField="Name" HeaderText="Profile Name" SortExpression="Name" ItemStyle-CssClass="width_200"></asp:BoundField>
             <asp:BoundField DataField="Kernel" HeaderText="Current Kernel" SortExpression="Name" ItemStyle-CssClass=""></asp:BoundField>
           
        </Columns>
        <EmptyDataTemplate>
            No Profiles Found
        </EmptyDataTemplate>
    </asp:GridView>
  
</asp:Content>


