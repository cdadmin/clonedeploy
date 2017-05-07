<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/admin.master" AutoEventWireup="true" CodeBehind="kerneldownload.aspx.cs" Inherits="CloneDeploy_Web.views.admin.kerneldownload" Async="true" %>
<asp:Content ID="Breadcrumb" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li>Kernel Downloads</li>
</asp:Content>

<asp:Content runat="server" ID="Help" ContentPlaceHolderID="Help">
    <li role="separator" class="divider"></li>
    <li>
        <a href="<%= ResolveUrl("~/views/help/admin-kerneldownload.aspx") %>" target="_blank">Help</a>
    </li>
</asp:Content>

<asp:Content runat="server" ID="ActionsRight" ContentPlaceHolderID="SubPageActionsRight">
    <asp:LinkButton ID="btnUpdateSettings" runat="server" Text="Update Server Settings " OnClick="btnUpdateSettings_OnClick" CssClass="btn btn-default"/>
    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        <span class="caret"></span>
    </button>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="SubContent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#kerneldownload').addClass("nav-current");
            $("[id*=gvKernels] td").hover(function() {
                $("td", $(this).closest("tr")).addClass("hover_row");
            }, function() {
                $("td", $(this).closest("tr")).removeClass("hover_row");
            });
        });
    </script>


    <asp:GridView ID="gvKernels" runat="server" AutoGenerateColumns="False" CssClass="Gridview extraPad" AlternatingRowStyle-CssClass="alt">
        <Columns>


            <asp:BoundField DataField="FileName" HeaderText="File" ItemStyle-CssClass="width_200"/>
            <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-CssClass="width_200"></asp:BoundField>
            <asp:BoundField DataField="BaseVersion" HeaderText="Version" ItemStyle-CssClass="width_200"/>

            <asp:TemplateField>

                <ItemStyle></ItemStyle>
                <ItemTemplate>
                    <asp:LinkButton ID="btnDownload" runat="server" OnClick="btnDownload_OnClick" Text="Download"/>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField ShowHeader="True" HeaderText="Installed">
                <ItemTemplate>
                    <asp:Label ID="lblInstalled" runat="server" CausesValidation="false" CssClass="lbl_file" Text="No"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>


        </Columns>

    </asp:GridView>
</asp:Content>