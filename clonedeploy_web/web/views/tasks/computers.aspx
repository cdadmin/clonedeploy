<%@ Page Title="" Language="C#" MasterPageFile="~/views/tasks/task.master" AutoEventWireup="true" Inherits="views.tasks.TaskUnicast" CodeFile="computers.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
     <li>Start Computer Task</li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Help" Runat="Server">
    <li><a href="<%= ResolveUrl("~/views/help/index.html")%>"  target="_blank">Help</a></li>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="SubPageActionsRight">
     <asp:LinkButton ID="btnListDeploy" runat="server" OnClick="btnListDeploy_Click" Text="Deploy Selected" CssClass="btn btn-default"></asp:LinkButton>
    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
    <span class="caret"></span>
  </button>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="AdditionalActions">
    <li><asp:LinkButton ID="btnListPermanentDeploy" runat="server" OnClick="btnListPermanentDeploy_Click" Text="Permanent Deploy Selected" ></asp:LinkButton></li>
    <li><asp:LinkButton ID="btnListUpload" runat="server" OnClick="btnListUpload_Click" Text="Upload Selected" ></asp:LinkButton></li>
</asp:Content>
<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#computer').addClass("nav-current");

            $("[id*=gvComputers] td").hover(function () {
                $("td", $(this).closest("tr")).addClass("hover_row");
            }, function () {
                $("td", $(this).closest("tr")).removeClass("hover_row");
            });
        });
    </script>
      <p class="total">
        <asp:Label ID="lblTotal" runat="server"></asp:Label>
    </p>
    <div class="size-7 column">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="searchbox" OnTextChanged="search_Changed"></asp:TextBox>
    </div>
    <br class="clear"/>
  
     <div class="size-11 column">
    <asp:DropDownList runat="server" ID="ddlLimit" AutoPostBack="True" OnSelectedIndexChanged="ddlLimit_OnSelectedIndexChanged" CssClass="ddlist">
        <asp:ListItem>25</asp:ListItem>
        <asp:ListItem>100</asp:ListItem>
         <asp:ListItem Selected="True">250</asp:ListItem>
        <asp:ListItem >500</asp:ListItem>
          <asp:ListItem>1000</asp:ListItem>
         <asp:ListItem>5000</asp:ListItem>
        <asp:ListItem>All</asp:ListItem>
        </asp:DropDownList>
    </div>
     <br class="clear" />

    <asp:GridView ID="gvComputers" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" CssClass="Gridview" AllowSorting="True" OnSorting="gridView_Sorting" AlternatingRowStyle-CssClass="alt">
        <Columns>
            <asp:TemplateField>
                
                <ItemStyle CssClass="chkboxwidth"></ItemStyle>
                <HeaderTemplate>
                    <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelectAll_CheckedChanged"/>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="chkSelector" runat="server"/>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField>

                  <ItemStyle CssClass="chkboxwidth"></ItemStyle>
                <ItemTemplate>
                    <asp:LinkButton ID="btnDeploy" runat="server" OnClick="btnDeploy_Click" Text="Deploy"/>
                </ItemTemplate>
            </asp:TemplateField>
             
            <asp:TemplateField>
                 <ItemStyle CssClass="chkboxwidth"></ItemStyle>
                <ItemTemplate>
                    <asp:LinkButton ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="Upload"/>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Id" HeaderText="computerID" InsertVisible="False" ReadOnly="True" SortExpression="Id" Visible="False"/>
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-CssClass="width_200"/>
              <asp:TemplateField ItemStyle-CssClass="width_200 mobi-hide-smaller" HeaderStyle-CssClass="mobi-hide-smaller" HeaderText="Image">
                <ItemTemplate>
                    <asp:Label ID="lblImage" runat="server" Text='<%# Bind("Image.Name") %>'/>
                </ItemTemplate>
            </asp:TemplateField>
              <asp:TemplateField>

                  <ItemStyle CssClass="chkboxwidth"></ItemStyle>
                <ItemTemplate>
                    <asp:LinkButton ID="btnPermanentDeploy" runat="server" OnClick="btnPermanentDeploy_Click" Text="Permanent Deploy"/>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-CssClass="width_250">
                <ItemTemplate>
                </ItemTemplate>
            </asp:TemplateField>
           
        </Columns>
        <EmptyDataTemplate>
            No Computers Found
        </EmptyDataTemplate>
    </asp:GridView>
    <div id="confirmbox" class="confirm-box-outer">
        <div class="confirm-box-inner">
            <h4>
                <asp:Label ID="lblTitle" runat="server" CssClass="modaltitle"></asp:Label>
            </h4>
            <asp:GridView ID="gvConfirm" runat="server" CssClass="Gridview gv-confirm" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="Name" HeaderText="Name"/>
                    <asp:BoundField DataField="Mac" HeaderText="MAC" ItemStyle-CssClass="mobi-hide-smallest" HeaderStyle-CssClass="mobi-hide-smallest"/>
                      <asp:TemplateField HeaderText="Image">
                <ItemTemplate>
                    <asp:Label ID="lblImage" runat="server" Text='<%# Bind("Image.Name") %>'/>
                </ItemTemplate>
            </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <div class="confirm-box-btns">
                <asp:LinkButton ID="SingleOkButton" OnClick="SingleOkButton_Click" runat="server" Visible="False" Text="Yes" CssClass="confirm_yes"/>
                <asp:LinkButton ID="MultiOkButton" OnClick="MultiOkButton_Click" runat="server" Visible="False" Text="Yes" CssClass="confirm_yes"/>
                <asp:LinkButton ID="CancelButton" runat="server" Text="No" CssClass="confirm_no"/>
            </div>
        </div>
    </div>
   
</asp:Content>