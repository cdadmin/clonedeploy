<%@ Page Title="" Language="C#" MasterPageFile="~/views/site.master" AutoEventWireup="true" Inherits="CloneDeploy_Web.views.dashboard.Dashboard" ValidateRequest="false" Codebehind="dash.aspx.cs" %>

<asp:Content runat="server" ID="Breadcrumb" ContentPlaceHolderID="Breadcrumb">
    <li>
        <a href="<%= ResolveUrl("~/views/dashboard/dash.aspx") %>">Dashboard</a>
    </li>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="SubNav" runat="Server">
    &nbsp;
</asp:Content>

<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="Server">

    <p class="denied_text">
        <asp:Label ID="lblDenied" runat="server"></asp:Label>
    </p>

    <div class="column-left">
        <h4>Totals</h4>
        <br/>
        <div class="DashDiv">
            <a href="<%= ResolveUrl("~/views/computers/search.aspx") %>" class="icon-host">
                <span class="testlbl">
                <asp:Label ID="lblTotalComputers" runat="server"></asp:Label></span>
            </a>

            <br/>
            <br class="clear"/>

            <a class="icon-group" href="<%= ResolveUrl("~/views/groups/search.aspx") %>">
                <asp:Label ID="lblTotalGroups" runat="server"></asp:Label>
            </a>


            <p class="DashTotalImages">
                <a class="icon-image" href="<%= ResolveUrl("~/views/images/search.aspx") %>">
                    <asp:Label ID="lblTotalImages" runat="server"></asp:Label>
                </a>
            </p>
        </div>

    </div>

    <div class="column-center">
        <h4 class="right">Current User</h4>
        <br/>
        <asp:LinkButton ID="LogOut" runat="server" OnClick="LogOut_OnClick"></asp:LinkButton>


    </div>

    <div class="column-right">
        <h4>Primary Distribution Point</h4>
        <asp:Label runat="server" ID="lblDpPath"></asp:Label>
        <br/>

        <div class="clearfix">
            <table style="margin-left: 30px; margin-top: 15px;">
                <tr>
                    <td>
                        <div>Free</div>
                        <div class="c100 p<%= freePercent %> small">
                            <span><%= freePercent %>%</span>
                            <div class="slice">
                                <div class="bar"></div>
                                <div class="fill"></div>
                            </div>
                        </div>
                    </td>

                    <td>
                        <div>Used</div>
                        <div class="c100 p<%= usedPercent %> small">
                            <span><%= usedPercent %>%</span><div class="slice">
                                <div class="bar"></div>
                                <div class="fill"></div>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>


        <asp:Label ID="lblDPfree" runat="server"></asp:Label>
        <br/>
        <asp:Label ID="lblDpTotal" runat="server"></asp:Label>


    </div>

    <div class="column-left-half">
        <div class="dash_left_second">
            <h4>Recent Logins</h4>
            <asp:GridView ID="gvLogins" runat="server" DataKeyNames="Id" AutoGenerateColumns="False" CssClass="Gridview" AlternatingRowStyle-CssClass="alt">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="" Visible="False"/>
                    <asp:BoundField DataField="ObjectName" HeaderText="User Name" ItemStyle-CssClass="width_200"></asp:BoundField>
                    <asp:BoundField DataField="AuditType" HeaderText="Type" ItemStyle-CssClass="width_200"/>
                    <asp:BoundField DataField="DateTime" HeaderText="Date" ItemStyle-CssClass="width_200"/>
                </Columns>
                <EmptyDataTemplate>
                    No Logins Found
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
    </div>

    <div class="column-right-half">
        <div class="dash_left_second">
            <h4>Recent Tasks</h4>
            <asp:GridView ID="gvTasks" runat="server" DataKeyNames="Id" AutoGenerateColumns="False" CssClass="Gridview" AlternatingRowStyle-CssClass="alt">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="" Visible="False"/>
                    <asp:BoundField DataField="ObjectName" HeaderText="Computer Name" ItemStyle-CssClass="width_200"></asp:BoundField>
                    <asp:BoundField DataField="AuditType" HeaderText="Type" ItemStyle-CssClass="width_200"/>
                    <asp:BoundField DataField="DateTime" HeaderText="Date" ItemStyle-CssClass="width_200"/>
                </Columns>
                <EmptyDataTemplate>
                    No Tasks Found
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
    </div>

</asp:Content>