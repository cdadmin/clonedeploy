<%@ Page Title="" Language="C#" MasterPageFile="~/views/Site.master" AutoEventWireup="true" Inherits="views.dashboard.Dashboard" CodeFile="dash.aspx.cs" ValidateRequest="false"%>


<asp:Content ID="Header" ContentPlaceHolderID="HeaderNav" runat="Server">
    <div class="test" style="float: right; margin-top: 40px; text-align: right;">
        <h4>CloneDeploy 1.0.0</h4>
    </div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="SubNav" runat="Server">
    </asp:Content>

<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="Server">
    <p class="denied_text">
        <asp:Label ID="lblDenied" runat="server"></asp:Label>
    </p>
    
   
    

</asp:Content>