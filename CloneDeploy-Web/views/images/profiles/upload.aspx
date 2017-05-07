<%@ Page Title="" Language="C#" MasterPageFile="~/views/images/profiles/profiles.master" AutoEventWireup="true" Inherits="views_images_profiles_upload" Codebehind="upload.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
    <li>
        <a href="<%= ResolveUrl("~/views/images/profiles/general.aspx") %>?imageid=<%= Image.Id %>&profileid=<%= ImageProfile.Id %>&cat=profiles"><%= ImageProfile.Name %></a>
    </li>
    <li>Upload Options</li>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="SubHelp">
    <li role="separator" class="divider"></li>
    <li>
        <a href="<%= ResolveUrl("~/views/help/images-uploadoptions.aspx") %>" target="_blank">Help</a>
    </li>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ActionsRightSub">
    <asp:LinkButton ID="btnUpdateUpload" runat="server" OnClick="btnUpdateUpload_OnClick" Text="Update Upload Options" CssClass="btn btn-default"/>
    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        <span class="caret"></span>
    </button>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
<script type="text/javascript">
    $(document).ready(function() {
        $('#upload').addClass("nav-current");
    });
</script>

<div id="divGpt" runat="server">
    <div class="size-9 column">
        Remove GPT Structures
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkRemoveGpt" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>
</div>

<div id="divShrink" runat="server">
    <div class="size-9 column">
        Don't Shrink Volumes
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkUpNoShrink" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>

    <div class="size-9 column">
        Don't Shrink LVM Volumes
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkUpNoShrinkLVM" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>
</div>

<div id="divCompression" runat="server">
    <div class="size-9 column">
        Compression Algorithm:
    </div>
    <div class="size-8 column">
        <asp:DropDownList ID="ddlCompAlg" runat="server" CssClass="ddlist">
            <asp:ListItem>gzip</asp:ListItem>
            <asp:ListItem>lz4</asp:ListItem>
            <asp:ListItem>none</asp:ListItem>
        </asp:DropDownList>
    </div>
    <br class="clear"/>
    <div class="size-9 column">
        Compression Level:
    </div>

    <div class="size-8 column">
        <asp:DropDownList ID="ddlCompLevel" runat="server" CssClass="ddlist">
            <asp:ListItem>1</asp:ListItem>
            <asp:ListItem>2</asp:ListItem>
            <asp:ListItem>3</asp:ListItem>
            <asp:ListItem>4</asp:ListItem>
            <asp:ListItem>5</asp:ListItem>
            <asp:ListItem>6</asp:ListItem>
            <asp:ListItem>7</asp:ListItem>
            <asp:ListItem>8</asp:ListItem>
            <asp:ListItem>9</asp:ListItem>
        </asp:DropDownList>
    </div>
</div>
<br class="clear"/>

<div id="divWimMulticast" runat="server">
    <div class="size-9 column">
        Enable Multicast Support:
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkWimMulticast" runat="server"/>
    </div>
    <br class="clear"/>
</div>

<div class="size-9 column">
    Only Upload Schema
</div>
<div class="size-8 column">
    <asp:CheckBox ID="chkSchemaOnly" runat="server" CssClass="textbox"></asp:CheckBox>
</div>

<br class="clear"/>
<div class="size-9 column">
    Use Custom Upload Schema
</div>
<div class="size-8 column">
    <asp:CheckBox ID="chkCustomUpload" runat="server" CssClass="textbox" AutoPostBack="True" OnCheckedChanged="chkCustomUpload_OnCheckedChanged"></asp:CheckBox>
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

            <asp:TemplateField ItemStyle-CssClass="width_50" HeaderText="Upload">
                <ItemTemplate>
                    <asp:CheckBox ID="chkHDActive" runat="server" Checked='<%# Bind("Active") %>'/>
                </ItemTemplate>
            </asp:TemplateField>


            <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-CssClass="width_100"></asp:BoundField>
            <asp:BoundField DataField="Size" HeaderText="Size (Reported / Usable)" ItemStyle-CssClass="width_200"></asp:BoundField>
            <asp:BoundField DataField="Table" HeaderText="Table" ItemStyle-CssClass="width_100"></asp:BoundField>
            <asp:BoundField DataField="Boot" HeaderText="Boot Flag" ItemStyle-CssClass="width_100"></asp:BoundField>

            <asp:TemplateField>
                <ItemTemplate>
                    <tr>
                        <td id="tdParts" runat="server" visible="false" colspan="900">
                            <asp:GridView ID="gvParts" AutoGenerateColumns="false" runat="server" CssClass="Gridview gv_parts" ShowHeader="true" Visible="false" AlternatingRowStyle-CssClass="alt">
                                <Columns>
                                    <asp:TemplateField ItemStyle-CssClass="width_50" HeaderText="Upload">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkPartActive" runat="server" Checked='<%# Bind("Active") %>'/>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="Number" HeaderText="#" ItemStyle-CssClass="width_100"></asp:BoundField>

                                    <asp:BoundField DataField="Size" HeaderText="Size" ItemStyle-CssClass="width_100"></asp:BoundField>
                                    <asp:BoundField DataField="VolumeSize" HeaderText="Volume" ItemStyle-CssClass="width_100"></asp:BoundField>

                                    <asp:BoundField DataField="FsType" HeaderText="FS" ItemStyle-CssClass="width_100"></asp:BoundField>

                                    <asp:BoundField DataField="UsedMb" HeaderText="Used" ItemStyle-CssClass="width_100"></asp:BoundField>

                                    <asp:TemplateField ItemStyle-CssClass="width_150" HeaderText="Fixed Size">
                                        <ItemTemplate>

                                            <asp:CheckBox runat="server" id="chkFixed" Checked='<%# Bind("ForceFixedSize") %>'/>

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

                                                                                    <asp:TemplateField ItemStyle-CssClass="width_50" HeaderText="Upload">
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

</asp:Content>