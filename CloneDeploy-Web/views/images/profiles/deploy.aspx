﻿<%@ Page Title="" Language="C#" MasterPageFile="~/views/images/profiles/profiles.master" AutoEventWireup="true" Inherits="CloneDeploy_Web.views.images.profiles.views_images_profiles_deploy" Codebehind="deploy.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
    <li>
        <a href="<%= ResolveUrl("~/views/images/profiles/general.aspx") %>?imageid=<%= Image.Id %>&profileid=<%= ImageProfile.Id %>&cat=profiles"><%= ImageProfile.Name %></a>
    </li>
    <li>Deploy Options</li>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="SubHelp">
    <li role="separator" class="divider"></li>
    <li>
        <a href="<%= ResolveUrl("~/views/help/images-deployoptions.aspx") %>" target="_blank">Help</a>
    </li>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ActionsRightSub">
    <asp:LinkButton ID="btnUpdateDeploy" runat="server" OnClick="btnUpdateDeploy_OnClick" Text="Update Deploy Options " OnClientClick="update_click()" CssClass="btn btn-default"/>
    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        <span class="caret"></span>
    </button>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
<script type="text/javascript">
    $(document).ready(function() {
        $('#deploy').addClass("nav-current");
    });
</script>

<div class="size-9 column">
    Change Computer Name
</div>
<div class="size-8 column">
    <asp:CheckBox ID="chkChangeName" runat="server" CssClass="textbox"></asp:CheckBox>
</div>
<br class="clear"/>

<div id="divExpandVol" runat="server">
    <div class="size-9 column">
        Don't Expand Volumes
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkDownNoExpand" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>
</div>

<div id="divBoot" runat="server">
    <div class="size-9 column">
        Update BCD
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkAlignBCD" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>
    
     <div class="size-9 column">
        Randomize GUIDs
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkRandomize" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>

    <div class="size-9 column">
        Fix Boot Sector
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkRunFixBoot" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>
       <div class="size-9 column">
       Don't Update NVRAM
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkNvram" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>
</div>
   

<div id="DivPartDdlLin" runat="server" Visible="False">
    <div class="size-9 column">
        Create Partitions Method
    </div>
    <div class="size-5 column">
        <asp:dropdownlist ID="ddlPartitionMethodLin" runat="server" CssClass="ddlist" OnSelectedIndexChanged="ddlPartitionMethod_OnSelectedIndexChanged" AutoPostBack="True">
            <asp:ListItem>Use Original MBR / GPT</asp:ListItem>
            <asp:ListItem>Standard</asp:ListItem>
            <asp:ListItem>Dynamic</asp:ListItem>
            <asp:ListItem>Custom Script</asp:ListItem>
        </asp:dropdownlist>
    </div>
    <br class="clear"/>
</div>

<div id="DivPartDdlWin" runat="server" Visible="False">
    <div class="size-9 column">
        Create Partitions Method
    </div>
    <div class="size-5 column">
        <asp:dropdownlist ID="ddlPartitionMethodWin" runat="server" CssClass="ddlist" OnSelectedIndexChanged="ddlPartitionMethod_OnSelectedIndexChanged" AutoPostBack="True">
            <asp:ListItem>Standard</asp:ListItem>
            <asp:ListItem>Dynamic</asp:ListItem>
            <asp:ListItem>Custom Script</asp:ListItem>
        </asp:dropdownlist>
    </div>
    <br class="clear"/>
</div>


<div id="DivStandardOptions" runat="server" Visible="False">
     <div class="size-9 column">
        Force Standard EFI Partitions
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkForceEfi" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>
     <div class="size-9 column">
        Force Standard Legacy Partitions
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkForceLegacy" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>
</div>

<div id="ForceDiv" runat="server" Visible="False">
    <div class="size-9 column">
        Force Dynamic Partition For Exact Hdd Match
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkDownForceDynamic" runat="server" AutoPostBack="True" OnCheckedChanged="chkForceDynamic_OnCheckedChanged"></asp:CheckBox>
    </div>
    <br class="clear"/>
</div>
<div class="size-9 column">
    Modify The Image Schema
</div>
<div class="size-8 column">
    <asp:CheckBox ID="chkModifySchema" runat="server" AutoPostBack="True" OnCheckedChanged="chkModifySchema_OnCheckedChanged"></asp:CheckBox>
</div>
<br class="clear"/>

<div id="imageSchema" runat="server" visible="false">
<div class="size-4 column">
    <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_OnClick" Text="Export Schema" CssClass="submits"/>
</div>
<br class="clear"/>
<asp:GridView ID="gvHDs" runat="server" AutoGenerateColumns="false" CssClass="Gridview" AlternatingRowStyle-CssClass="alt">
<Columns>

<asp:TemplateField ShowHeader="False" ItemStyle-CssClass="width_30" HeaderStyle-CssClass="">
    <ItemTemplate>
        <div style="width: 0">
            <asp:LinkButton ID="btnHd" runat="server" CausesValidation="false" CommandName="" Text="+" OnClick="btnHd_Click"></asp:LinkButton>
        </div>
    </ItemTemplate>
</asp:TemplateField>

<asp:TemplateField ItemStyle-CssClass="width_50" HeaderText="Active">
    <ItemTemplate>
        <asp:CheckBox ID="chkHDActive" runat="server" Checked='<%# Bind("Active") %>'/>
    </ItemTemplate>
</asp:TemplateField>


<asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-CssClass="width_100"></asp:BoundField>
<asp:TemplateField ItemStyle-CssClass="width_50" HeaderText="Destination">
    <ItemTemplate>
        <asp:TextBox ID="txtDestination" runat="server" Text='<%# Bind("Destination") %>'/>
    </ItemTemplate>
</asp:TemplateField>
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


                        <asp:TemplateField ItemStyle-CssClass="width_50" HeaderText="Active">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkPartActive" runat="server" Checked='<%# Bind("Active") %>'/>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="Number" HeaderText="#" ItemStyle-CssClass="width_100"></asp:BoundField>

                        <asp:BoundField DataField="Size" HeaderText="Size" ItemStyle-CssClass="width_100"></asp:BoundField>
                        <asp:BoundField DataField="VolumeSize" HeaderText="Volume" ItemStyle-CssClass="width_100"></asp:BoundField>
                        <asp:BoundField DataField="Type" HeaderText="Type" ItemStyle-CssClass="width_100"></asp:BoundField>
                        <asp:BoundField DataField="FsType" HeaderText="FS" ItemStyle-CssClass="width_100"></asp:BoundField>
                        <asp:BoundField DataField="FsId" HeaderText="FSID" ItemStyle-CssClass="width_105"></asp:BoundField>
                        <asp:BoundField DataField="UsedMb" HeaderText="Used" ItemStyle-CssClass="width_100"></asp:BoundField>
                        <asp:TemplateField ItemStyle-CssClass="width_100" HeaderText="Custom Size">
                            <ItemTemplate>
                                <div id="settings">
                                    <asp:TextBox ID="txtCustomSize" runat="server" Text='<%# Bind("CustomSize") %>' CssClass="textbox_specs"/>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-CssClass="width_150" HeaderText="Unit">
                            <ItemTemplate>

                                <asp:DropDownList ID="ddlUnit" runat="server" CssClass="ddlist" Text='<%# Bind("CustomSizeUnit") %>'>
                                    <asp:ListItem></asp:ListItem>
                                    <asp:ListItem>MB</asp:ListItem>
                                    <asp:ListItem>GB</asp:ListItem>
                                    <asp:ListItem>%</asp:ListItem>

                                </asp:DropDownList>

                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-CssClass="width_150" HeaderText="Fixed">
                            <ItemTemplate>

                                <asp:CheckBox runat="server" id="chkFixed" Checked='<%# Bind("ForceFixedSize") %>'/>

                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>

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


                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="UUID" Font-Bold="true"/>
                                        <asp:Label ID="lblUUID" runat="server" Text='<%# Bind("uuid") %>'/>

                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="GUID" Font-Bold="true"/>
                                        <asp:Label ID="lblGUID" runat="server" Text='<%# Bind("guid") %>'/>
                                    </td>
                                </tr>
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

                                                                        <asp:TemplateField ItemStyle-CssClass="width_50" HeaderText="Active">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkPartActive" runat="server" Checked='<%# Bind("Active") %>'/>
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
                                                                        <asp:TemplateField ItemStyle-CssClass="width_150" HeaderText="Unit">
                                                                            <ItemTemplate>

                                                                                <asp:DropDownList ID="ddlUnit" runat="server" CssClass="ddlist" Text='<%# Bind("CustomSizeUnit") %>'>
                                                                                    <asp:ListItem></asp:ListItem>
                                                                                    <asp:ListItem>MB</asp:ListItem>
                                                                                    <asp:ListItem>GB</asp:ListItem>
                                                                                    <asp:ListItem>%</asp:ListItem>

                                                                                </asp:DropDownList>

                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ItemStyle-CssClass="width_150" HeaderText="Fixed">
                                                                            <ItemTemplate>

                                                                                <asp:CheckBox runat="server" id="chkFixed" Checked='<%# Bind("ForceFixedSize") %>'/>

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


</asp:Content>