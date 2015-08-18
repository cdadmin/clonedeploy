<%@ Page Title="" Language="C#" MasterPageFile="~/views/masters/Admin.master" AutoEventWireup="true" Inherits="views.admin.AdminChooser" CodeFile="chooser.aspx.cs" ValidateRequest="false" %>

<%@ MasterType VirtualPath="~/views/masters/Admin.master" %>
<%@ Reference VirtualPath="~/views/masters/Site.master" %>
<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">
    <div class="full column margin-top-min20">
        <h3 class="txt-left no-margin">Global</h3>
     <div class="nav-btn-square nav-btn-square-large">
         
        <a id="editoption" href="<%= ResolveUrl("~/views/admin/settings.aspx") %>" class="icon global global-large">
             <span class="chooser-text">Server</span>
        </a>
         
          <a id="editoption" href="<%= ResolveUrl("~/views/admin/settings.aspx") %>" class="icon global global-large">
             <span class="chooser-text">Client</span>
        </a>
         
           
          <a id="editoption" href="<%= ResolveUrl("~/views/admin/settings.aspx") %>" class="icon global global-large">
             <span class="chooser-text">PXE</span>
        </a>
         
           
          <a id="editoption" href="<%= ResolveUrl("~/views/admin/settings.aspx") %>" class="icon global global-large">
             <span class="chooser-text">Security</span>
        </a>
         
           
          <a id="editoption" href="<%= ResolveUrl("~/views/admin/settings.aspx") %>" class="icon global global-large">
             <span class="chooser-text">Multicast</span>
        </a>
         
           
          <a id="editoption" href="<%= ResolveUrl("~/views/admin/settings.aspx") %>" class="icon global global-large">
             <span class="chooser-text">E-mail</span>
        </a>
         </div>
        </div>
    <br class="clear"/>
         <div class="full column margin-top-min20">
             <h3 class="txt-left no-margin">Editors</h3>
     <div class="nav-btn-square nav-btn-square-large">
         
        <a id="editoption" href="<%= ResolveUrl("~/views/admin/settings.aspx") %>" class="icon global global-large">
             <span class="chooser-text">Boot Menu</span>
        </a>
         
          <a id="editoption" href="<%= ResolveUrl("~/views/admin/settings.aspx") %>" class="icon global global-large">
             <span class="chooser-text">Script Editor</span>
        </a>
         
           
          <a id="editoption" href="<%= ResolveUrl("~/views/admin/settings.aspx") %>" class="icon global global-large">
             <span class="chooser-text">Sysprep Editor</span>
        </a>
         
           </div>

             </div>

    <br class="clear" />
     <div class="full column margin-top-min20">
           <h3 class="txt-left">Util</h3>
     <div class="nav-btn-square nav-btn-square-large">
        
        <a id="editoption" href="<%= ResolveUrl("~/views/admin/settings.aspx") %>" class="icon global global-large">
             <span class="chooser-text">Logs</span>
        </a>
         
          <a id="editoption" href="<%= ResolveUrl("~/views/admin/settings.aspx") %>" class="icon global global-large">
             <span class="chooser-text">Export Database</span>
        </a>
         
           
          <a id="editoption" href="<%= ResolveUrl("~/views/admin/settings.aspx") %>" class="icon global global-large">
             <span class="chooser-text">Reports</span>
        </a>
         </div>
         </div>
           <br class="clear" />
         
     <div class="full column margin-top-min20">
           <h3 class="txt-left">Organization</h3>
     <div class="nav-btn-square nav-btn-square-large">
        
          <a id="editoption" href="<%= ResolveUrl("~/views/admin/settings.aspx") %>" class="icon global global-large">
             <span class="chooser-text">Sites</span>
        </a>
          <a id="editoption" href="<%= ResolveUrl("~/views/admin/settings.aspx") %>" class="icon global global-large">
             <span class="chooser-text">Buildings</span>
        </a>
          <a id="editoption" href="<%= ResolveUrl("~/views/admin/settings.aspx") %>" class="icon global global-large">
             <span class="chooser-text">Departments</span>
        </a>
          <a id="editoption" href="<%= ResolveUrl("~/views/admin/settings.aspx") %>" class="icon global global-large">
             <span class="chooser-text">Rooms</span>
        </a>

    </div>
         </div>

</asp:Content>
