<%@ Page Title="" Language="C#" MasterPageFile="~/views/images/profiles/profiles.master" AutoEventWireup="true" CodeFile="partition.aspx.cs" Inherits="views_images_profiles_partition" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
      <li><a href="<%= ResolveUrl("~/views/images/profiles/chooser.aspx") %>?imageid=<%= Image.Id %>&profileid=<%= ImageProfile.Id %>&cat=profiles"><%= ImageProfile.Name %></a></li>
    <li>Partitions</li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
     <script type="text/javascript">
        $(document).ready(function() {
            $('#partition').addClass("nav-current");
        });
    </script>
   
     <div class="size-9 column">
        Create Partitions Method
    </div>
    <div class="size-5 column">
        <asp:dropdownlist ID="ddlPartitionMethod" runat="server" CssClass="ddlist" OnSelectedIndexChanged="ddlPartitionMethod_OnSelectedIndexChanged" AutoPostBack="True">
            <asp:ListItem>Use Original MBR / GPT</asp:ListItem>
            <asp:ListItem>Dynamic</asp:ListItem>
            <asp:ListItem>Custom Script</asp:ListItem>
            <asp:ListItem>Custom Layout</asp:ListItem>
        </asp:dropdownlist>
    </div>
    <br class="clear"/>
    
        
     <div class="size-9 column">
        Force Dynamic Partition For Exact Hdd Match
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkDownForceDynamic" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>
    
 
    <div id="customScript" runat="server">
    <br/><br/>
    <h3 class="txt-left">Custom Partition Script</h3>
    <div class="full column">
        <pre id="editor" class="editor height_400"></pre>
     <asp:HiddenField ID="scriptEditorText" runat="server"/>

<script>

    var editor = ace.edit("editor");
    editor.session.setValue($('#<%= scriptEditorText.ClientID %>').val());

    editor.setTheme("ace/theme/idle_fingers");
    editor.getSession().setMode("ace/mode/sh");
    editor.setOption("showPrintMargin", false);
    editor.session.setUseWrapMode(true);
    editor.session.setWrapLimitRange(60, 60);


    function update_click() {
        var editor = ace.edit("editor");
        $('#<%= scriptEditorText.ClientID %>').val(editor.session.getValue());
    }

</script>
    <br class="clear"/>
       </div>
    </div>
    
    <div id="customLayout" runat="server">
        <asp:GridView ID="gvLayout" runat="server" AllowSorting="True" DataKeyNames="Id" OnSorting="gridView_Sorting" AutoGenerateColumns="False" CssClass="Gridview" AlternatingRowStyle-CssClass="alt">
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
            <asp:BoundField DataField="Id" HeaderText="hostID" SortExpression="hostID" Visible="False"/>
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-CssClass="width_200"></asp:BoundField>
            <asp:BoundField DataField="Table" HeaderText="Table" SortExpression="Table" ItemStyle-CssClass="width_200 mobi-hide-smallest" HeaderStyle-CssClass="mobi-hide-smallest"/>
            <asp:BoundField DataField="ImageEnvironment" HeaderText="Environment" SortExpression="ImageEnvironment" ItemStyle-CssClass="width_200 mobi-hide-smallest" HeaderStyle-CssClass="mobi-hide-smallest"/>
          
         
            <asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="~/views/global/partitions/edit.aspx?cat=sub1&layoutid={0}" Text="View"/>
        </Columns>
        <EmptyDataTemplate>
            No Hosts Found
        </EmptyDataTemplate>
    </asp:GridView>
    </div>
    <br class="clear"/>
      <div class="size-4 column">
        &nbsp;
    </div>
    <div class="size-5 column">
        <asp:LinkButton ID="btnUpdatePartitions" runat="server" OnClick="btnUpdatePartitions_OnClick" Text="Update Partition Options" CssClass="submits" OnClientClick="update_click();"/>
    </div>
</asp:Content>

