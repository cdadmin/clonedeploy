<%@ Page Title="" Language="C#" MasterPageFile="~/views/images/images.master" AutoEventWireup="true" CodeFile="specs.aspx.cs" Inherits="views.images.ImageSpecs" %>
<%@ MasterType VirtualPath="~/views/images/images.master" %>
<%@ Reference virtualPath="~/views/masters/Site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="SubContent" Runat="Server">
<script type="text/javascript">
    $(document).ready(function() {
        $('#logoption').addClass("nav-current");
    });
</script>

<asp:Label ID="lblSpecsUnavailable" Visible="false" runat="server"></asp:Label>

<br class="clear"/>
<div class="column size-1" style="border: 1px solid red; padding: 5px;" id="incorrectChecksum" runat="server" visible="false">
    <p>The Image Checksum Does Not Match What Was Previously Reported. If You Have Recently Uploaded This Image, You Must Confirm It Before It Can Be Deployed. Otherwise, It May Have Been Tampered With and Should Be Deleted.</p>
    <asp:LinkButton ID="btnConfirmChecksum" runat="server" OnClick="btnConfirmChecksum_Click" Text="Confirm" CssClass="submits"/>
    <br class="clear"/>
</div>
<br class="clear"/>
<asp:GridView ID="gvHDs" runat="server" AutoGenerateColumns="false" CssClass="Gridview" AlternatingRowStyle-CssClass="alt">
    <Columns>

        <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="width_30" HeaderStyle-CssClass="">
            <ItemTemplate>
                <div style="width: 0">
                    <asp:LinkButton ID="btnParts" runat="server" CausesValidation="false" CommandName="" Text="+" OnClick="btnParts_Click"></asp:LinkButton>
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
        <asp:BoundField DataField="Size" HeaderText="Size(Reported / Usable)" ItemStyle-CssClass="width_200"></asp:BoundField>
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

                                <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="width_30" HeaderStyle-CssClass="">
                                    <ItemTemplate>
                                        <div style="width: 20px">
                                            <asp:LinkButton ID="partClick" runat="server" CausesValidation="false" CommandName="" Text="+" OnClick="btnPart_Click"></asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
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
                                <asp:BoundField DataField="Resize" HeaderText="Volume" ItemStyle-CssClass="width_100"></asp:BoundField>
                                <asp:BoundField DataField="Type" HeaderText="Type" ItemStyle-CssClass="width_100"></asp:BoundField>
                                <asp:BoundField DataField="FsType" HeaderText="FS" ItemStyle-CssClass="width_100"></asp:BoundField>
                                <asp:BoundField DataField="FsId" HeaderText="FSID" ItemStyle-CssClass="width_105"></asp:BoundField>
                                <asp:BoundField DataField="Used_Mb" HeaderText="Used" ItemStyle-CssClass="width_100"></asp:BoundField>
                                <asp:TemplateField ItemStyle-CssClass="width_100" HeaderText="Custom Size (MB)">
                                    <ItemTemplate>
                                        <div id="settings">
                                            <asp:TextBox ID="txtCustomSize" runat="server" Text='<%# Bind("Size_Override") %>' CssClass="textbox_specs"/>
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
                                                        <asp:BoundField DataField="Pv" HeaderText="PV" ItemStyle-CssClass="width_200"/>
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
                                                                                <asp:BoundField DataField="Resize" HeaderText="Resize" ItemStyle-CssClass="width_100"></asp:BoundField>

                                                                                <asp:BoundField DataField="FsType" HeaderText="FS" ItemStyle-CssClass="width_100"></asp:BoundField>
                                                                                <asp:BoundField DataField="Uuid" HeaderText="UUID" ItemStyle-CssClass="width_100"></asp:BoundField>

                                                                                <asp:BoundField DataField="Used_Mb" HeaderText="Used" ItemStyle-CssClass="width_100"></asp:BoundField>

                                                                                <asp:TemplateField ItemStyle-CssClass="width_100" HeaderText="Custom Size (MB)">
                                                                                    <ItemTemplate>
                                                                                        <div id="subsettings">
                                                                                            <asp:TextBox ID="txtCustomSize" runat="server" Text='<%# Bind("Size_Override") %>' CssClass="textbox_specs"/>
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

                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <tr>
                                            <td id="tdFile" runat="server" visible="false" colspan="900">
                                                <asp:GridView ID="gvFiles" AutoGenerateColumns="false" runat="server" CssClass="Gridview gv_parts" ShowHeader="true" Visible="false" AlternatingRowStyle-CssClass="alt">
                                                    <Columns>

                                                        <asp:BoundField DataField="fileName" HeaderText="File Name" ItemStyle-CssClass="width_100"/>
                                                        <asp:BoundField DataField="serverSize" HeaderText="Server Size" ItemStyle-CssClass="width_200"/>

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
<div class="full column">
    <asp:LinkButton ID="btnUpdateSpecs" runat="server" OnClick="btnUpdateImageSpecs_Click" Text="Update Image Specs" CssClass="submits"/>
    <br class="clear"/>
    <asp:LinkButton ID="btnRestoreSpecs" runat="server" OnClick="btnRestoreImageSpecs_Click" Text="Restore Image Specs" CssClass="submits"/>
</div>
<br class="clear"/>
</asp:Content>