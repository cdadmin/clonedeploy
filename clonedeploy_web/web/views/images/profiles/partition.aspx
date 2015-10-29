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
    
    
    <div id="dynamicPartition" runat="server" >
     <div class="size-9 column">
        Force Dynamic Partition For Exact Hdd Match
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkDownForceDynamic" runat="server"></asp:CheckBox>
    </div>
    <br class="clear"/>
        
         <div class="size-9 column">
        Modify The Image Schema
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkModifySchema" runat="server" AutoPostBack="True" OnCheckedChanged="chkModifySchema_OnCheckedChanged"></asp:CheckBox>
    </div>
    <br class="clear"/>
        
        <div id="imageSchema" runat="server">
            <asp:GridView ID="gvHDs" runat="server" AutoGenerateColumns="false" CssClass="Gridview" AlternatingRowStyle-CssClass="alt">
    <Columns>

        <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="width_30" HeaderStyle-CssClass="">
            <ItemTemplate>
                <div style="width: 0">
                    <asp:LinkButton ID="btnHd" runat="server" CausesValidation="false" CommandName="" Text="+" OnClick="btnHd_Click"></asp:LinkButton>
                </div>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:HiddenField ID="HiddenActive" runat="server" Value='<%# Bind("active") %>'/>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField ItemStyle-CssClass="width_50" HeaderText="Active">
            <ItemTemplate>
                <asp:CheckBox ID="chkHDActive" runat="server"/>
            </ItemTemplate>
        </asp:TemplateField>


        <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-CssClass="width_100"></asp:BoundField>
        <asp:BoundField DataField="Size" HeaderText="Size (Reported / Usable)" ItemStyle-CssClass="width_200"></asp:BoundField>
        <asp:BoundField DataField="Table" HeaderText="Table" ItemStyle-CssClass="width_100"></asp:BoundField>
        <asp:BoundField DataField="Boot" HeaderText="Boot Flag" ItemStyle-CssClass="width_100"></asp:BoundField>
        <asp:BoundField DataField="Lbs" HeaderText="LBS" ItemStyle-CssClass="width_100"></asp:BoundField>
        <asp:BoundField DataField="Pbs" HeaderText="PBS" ItemStyle-CssClass="width_100"></asp:BoundField>
        <asp:BoundField DataField="Guid" HeaderText="GUID" ItemStyle-CssClass="width_100"></asp:BoundField>

        <asp:TemplateField>
            <ItemTemplate>
                <tr>
                    <td id="tdParts" runat="server" visible="false" colspan="900">
                        <asp:GridView ID="gvParts" AutoGenerateColumns="false" runat="server" CssClass="Gridview gv_parts" ShowHeader="true" Visible="false" AlternatingRowStyle-CssClass="alt">
                            <Columns>

                             
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:HiddenField ID="HiddenActivePart" runat="server" Value='<%# Bind("active") %>'/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="width_50" HeaderText="Active">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkPartActive" runat="server"/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Number" HeaderText="#" ItemStyle-CssClass="width_100"></asp:BoundField>
                                <asp:BoundField DataField="Start" HeaderText="Start" ItemStyle-CssClass="width_100"></asp:BoundField>
                                <asp:BoundField DataField="End" HeaderText="End" ItemStyle-CssClass="width_100"></asp:BoundField>
                                <asp:BoundField DataField="Size" HeaderText="Size" ItemStyle-CssClass="width_100"></asp:BoundField>
                                <asp:BoundField DataField="VolumeSize" HeaderText="Volume" ItemStyle-CssClass="width_100"></asp:BoundField>
                                <asp:BoundField DataField="Type" HeaderText="Type" ItemStyle-CssClass="width_100"></asp:BoundField>
                                <asp:BoundField DataField="FsType" HeaderText="FS" ItemStyle-CssClass="width_100"></asp:BoundField>
                                <asp:BoundField DataField="FsId" HeaderText="FSID" ItemStyle-CssClass="width_105"></asp:BoundField>
                                <asp:BoundField DataField="UsedMb" HeaderText="Used" ItemStyle-CssClass="width_100"></asp:BoundField>
                                <asp:TemplateField ItemStyle-CssClass="width_100" HeaderText="Custom Size (MB)">
                                    <ItemTemplate>
                                        <div id="settings">
                                            <asp:TextBox ID="txtCustomSize" runat="server" Text='<%# Bind("CustomSize") %>' CssClass="textbox_specs"/>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="width_100" HeaderText="Unit">
                                    <ItemTemplate>
                                        <div>
                                            <asp:DropDownList ID="chkUnit" runat="server" CssClass="ddlist"/>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        </td>
                                        <tr>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>

                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="UUID" Font-Bold="true"/>
                                            <asp:Label ID="lblUUID" runat="server" Text='<%# Bind("uuid") %>'/>

                                        </td>
                                        <td>
                                        <asp:Label ID="Label2" runat="server" Text="GUID" Font-Bold="true"/>
                                        <asp:Label ID="lblGUID" runat="server" Text='<%# Bind("guid") %>'/>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <tr>
                                            <td id="tdVG" runat="server" visible="false" colspan="900">
                                                <h4>
                                                    <asp:Label ID="LVM" runat="server" Text="Volume Group" style="margin-left: 30px;"></asp:Label>
                                                </h4>
                                                <asp:GridView ID="gvVG" AutoGenerateColumns="false" runat="server" CssClass="Gridview gv_vg" ShowHeader="true" Visible="false" AlternatingRowStyle-CssClass="alt">
                                                    <Columns>
                                                        <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="width_30" HeaderStyle-CssClass="">
                                                            <ItemTemplate>
                                                                <div style="width: 20px">
                                                                    <asp:LinkButton ID="vgClick" runat="server" CausesValidation="false" CommandName="" Text="+" OnClick="btnVG_Click"></asp:LinkButton>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-CssClass="width_100"/>
                                                        <asp:BoundField DataField="PhysicalVolume" HeaderText="PV" ItemStyle-CssClass="width_200"/>
                                                        <asp:BoundField DataField="Uuid" HeaderText="UUID" ItemStyle-CssClass="width_200"/>

                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td id="tdLVS" runat="server" visible="false" colspan="900">
                                                                        <asp:GridView ID="gvLVS" AutoGenerateColumns="false" runat="server" CssClass="Gridview gv_parts" ShowHeader="true" Visible="false" AlternatingRowStyle-CssClass="alt">
                                                                            <Columns>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:HiddenField ID="HiddenActivePart" runat="server" Value='<%# Bind("active") %>'/>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField ItemStyle-CssClass="width_50" HeaderText="Active">
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="chkPartActive" runat="server"/>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-CssClass="width_100"></asp:BoundField>

                                                                                <asp:BoundField DataField="Size" HeaderText="Size" ItemStyle-CssClass="width_100"></asp:BoundField>
                                                                                <asp:BoundField DataField="VolumeSize" HeaderText="Resize" ItemStyle-CssClass="width_100"></asp:BoundField>

                                                                                <asp:BoundField DataField="FsType" HeaderText="FS" ItemStyle-CssClass="width_100"></asp:BoundField>
                                                                                <asp:BoundField DataField="Uuid" HeaderText="UUID" ItemStyle-CssClass="width_100"></asp:BoundField>

                                                                                <asp:BoundField DataField="UsedMb" HeaderText="Used" ItemStyle-CssClass="width_100"></asp:BoundField>

                                                                                <asp:TemplateField ItemStyle-CssClass="width_100" HeaderText="Custom Size (MB)">
                                                                                    <ItemTemplate>
                                                                                        <div id="subsettings">
                                                                                            <asp:TextBox ID="txtCustomSize" runat="server" Text='<%# Bind("CustomSize") %>' CssClass="textbox_specs"/>
                                                                                        </div>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>

                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                    </Columns>


                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:TemplateField>

                               
                            </Columns>
                        </asp:GridView>


                    </td>


                </tr>
            </ItemTemplate>
        </asp:TemplateField>

    </Columns>
    <EmptyDataTemplate>
        No Image Schema Found
    </EmptyDataTemplate>
</asp:GridView>
        </div>
    </div>
 
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

