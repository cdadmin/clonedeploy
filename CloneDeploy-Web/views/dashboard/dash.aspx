<%@ Page Title="" Language="C#" MasterPageFile="~/views/site.master" AutoEventWireup="true" Inherits="CloneDeploy_Web.views.dashboard.Dashboard" ValidateRequest="false" Codebehind="dash.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="SubNav" runat="Server">
    &nbsp;
</asp:Content>

<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="Server">

    <p class="denied_text">
        <asp:Label ID="lblDenied" runat="server"></asp:Label>
    </p>

    <div class="column-left">
        <h4>Stats</h4>
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
        <br/><br/>
        <h4>Last Logins</h4>
    </div>

    <div class="column-center">
        <h4>Primary Distribution Point</h4>
        <p class="DashDistributionPoints">
            <asp:Label ID="lblTotalDP" runat="server"></asp:Label>
            <asp:Label ID="lblDPfree" runat="server"></asp:Label>
        </p>

    </div>

    <div class="column-right">
        <h4 class="right">Current User</h4>
        <br/>
        <asp:LinkButton ID="LogOut" runat="server" OnClick="LogOut_OnClick"></asp:LinkButton>
    </div>


</asp:Content>