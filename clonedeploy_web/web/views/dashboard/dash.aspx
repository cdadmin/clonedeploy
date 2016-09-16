<%@ Page Title="" Language="C#" MasterPageFile="~/views/site.master" AutoEventWireup="true" Inherits="views.dashboard.Dashboard" CodeFile="dash.aspx.cs" ValidateRequest="false"%>



<asp:Content ID="Content1" ContentPlaceHolderID="SubNav" runat="Server">
   
    </asp:Content>

<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="Server">
    
    <p class="denied_text">
        <asp:Label ID="lblDenied" runat="server"></asp:Label>
    </p>
    
    <div class="DashDiv">
      
          <a class="icon-host" href="/clonedeploy/views/computers/search.aspx">
                
       <span class="testlbl"><asp:Label ID="lblTotalComputers" runat="server"></asp:Label></span>
          </a>
      
        
     
          <a class="icon-group" href="/clonedeploy/views/groups/search.aspx">
            <asp:Label ID="lblTotalGroups" runat="server"></asp:Label>
          </a>
     
        
        <p class="DashTotalImages">
          <a class="icon-image" href="/clonedeploy/views/images/search.aspx">
            <asp:Label ID="lblTotalImages" runat="server"></asp:Label>
          </a>
        </p>
        
        <p class="DashDistributionPoints">
            <asp:Label ID="lblTotalDP" runat="server"></asp:Label>
            <asp:Label ID="lblDPfree" runat="server"></asp:Label>
        </p>
    
    </div>

</asp:Content>
