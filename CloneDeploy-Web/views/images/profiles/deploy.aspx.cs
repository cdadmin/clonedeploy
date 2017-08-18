using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CloneDeploy_Common;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Entities.DTOs.ImageSchemaBE;
using CloneDeploy_Web.BasePages;
using Newtonsoft.Json;
using VolumeGroup = CloneDeploy_Entities.DTOs.ImageSchemaFE.VolumeGroup;

namespace CloneDeploy_Web.views.images.profiles
{
    public partial class views_images_profiles_deploy : Images
    {
        private DropDownList ddlObject;

        protected void btnHd_Click(object sender, EventArgs e)
        {
            var control = sender as Control;
            if (control == null) return;
            var gvRow = (GridViewRow) control.Parent.Parent;
            var gv = (GridView) gvRow.FindControl("gvParts");

            var selectedHd = gvRow.Cells[2].Text;
            ViewState["selectedHD"] = gvRow.RowIndex.ToString();
            ViewState["selectedHDName"] = selectedHd;

            var schemaRequestOptions = new ImageSchemaRequestDTO();
            schemaRequestOptions.image = null;
            schemaRequestOptions.imageProfile = ImageProfile;
            schemaRequestOptions.schemaType = "deploy";
            schemaRequestOptions.selectedHd = selectedHd;
            var partitions = Call.ImageSchemaApi.GetPartitions(schemaRequestOptions);
            var btn = (LinkButton) gvRow.FindControl("btnHd");
            if (gv.Visible == false)
            {
                gv.Visible = true;

                var td = gvRow.FindControl("tdParts");
                td.Visible = true;
                gv.DataSource = partitions;
                gv.DataBind();

                btn.Text = "-";
            }
            else
            {
                gv.Visible = false;

                var td = gvRow.FindControl("tdParts");
                td.Visible = false;
                btn.Text = "+";
            }

            foreach (GridViewRow row in gv.Rows)
            {
                if (ddlObject.Text != "Dynamic")
                {
                    foreach (GridViewRow partRow in gv.Rows)
                    {
                        var txtCustomSize = partRow.FindControl("txtCustomSize") as TextBox;
                        if (txtCustomSize != null)
                            txtCustomSize.Enabled = false;

                        var ddlUnit = partRow.FindControl("ddlUnit") as DropDownList;
                        if (ddlUnit != null)
                            ddlUnit.Enabled = false;

                        var chkFixed = partRow.FindControl("chkFixed") as CheckBox;
                        if (chkFixed != null)
                            chkFixed.Enabled = false;
                    }
                }

                if (partitions[row.RowIndex].VolumeGroup == null) continue;
                if (partitions[row.RowIndex].VolumeGroup.Name == null) continue;
                var gvVg = (GridView) row.FindControl("gvVG");
                gvVg.DataSource = new List<VolumeGroup>
                {
                    partitions[row.RowIndex].VolumeGroup
                };
                gvVg.DataBind();

                gvVg.Visible = true;
                var td = row.FindControl("tdVG");
                td.Visible = true;
            }
        }

        protected void btnUpdateDeploy_OnClick(object sender, EventArgs e)
        {
            RequiresAuthorizationOrManagedImage(AuthorizationStrings.UpdateProfile, Image.Id);
            var imageProfile = ImageProfile;
            imageProfile.ChangeName = Convert.ToInt16(chkChangeName.Checked);
            imageProfile.SkipExpandVolumes = Convert.ToInt16(chkDownNoExpand.Checked);
            imageProfile.FixBcd = Convert.ToInt16(chkAlignBCD.Checked);
            imageProfile.FixBootloader = Convert.ToInt16(chkRunFixBoot.Checked);
            imageProfile.OsxInstallMunki = Convert.ToInt16(chkInstallMunki.Checked);
            imageProfile.MunkiRepoUrl = txtMunkiRepoUrl.Text;
            imageProfile.OsxTargetVolume = txtTargetVolume.Text;
            if (Image.Environment == "macOS")
                imageProfile.PartitionMethod = ddlPartitionMethodMac.Text;
            else if (Image.Environment == "winpe")
                imageProfile.PartitionMethod = ddlPartitionMethodWin.Text;
            else
                imageProfile.PartitionMethod = ddlPartitionMethodLin.Text;
            imageProfile.ForceDynamicPartitions = Convert.ToInt16(chkDownForceDynamic.Checked);
            imageProfile.MunkiAuthUsername = txtMunkiUsername.Text;
            if (!string.IsNullOrEmpty(txtMunkiPassword.Text))
                imageProfile.MunkiAuthPassword = txtMunkiPassword.Text;

            switch (ddlObject.Text)
            {
                case "Use Original MBR / GPT":
                    imageProfile.CustomSchema = chkModifySchema.Checked ? SetCustomSchema() : "";
                    break;
                case "Dynamic":

                    imageProfile.CustomSchema = chkModifySchema.Checked ? SetCustomSchema() : "";
                    break;
                case "Custom Script":
                    var fixedLineEnding = scriptEditorText.Value.Replace("\r\n", "\n");
                    imageProfile.CustomPartitionScript = fixedLineEnding;
                    imageProfile.CustomSchema = chkModifySchema.Checked ? SetCustomSchema() : "";
                    break;
                case "Standard Core Storage":
                    imageProfile.CustomSchema = chkModifySchema.Checked ? SetCustomSchema() : "";
                    break;
                case "Standard":
                    if (Image.Environment == "winpe")
                    {
                        imageProfile.CustomSchema = SetCustomSchema();
                    }
                    else
                    {
                        imageProfile.CustomSchema = chkModifySchema.Checked ? SetCustomSchema() : "";
                    }
                    break;
                default:
                    imageProfile.CustomPartitionScript = "";
                    break;
            }

            var isSchemaError = false;
            if (imageProfile.PartitionMethod == "Standard" && Image.Environment == "winpe")
            {
                var customSchema = JsonConvert.DeserializeObject<ImageSchema>(imageProfile.CustomSchema);

                foreach (var hd in customSchema.HardDrives)
                {
                    var activePartCounter = hd.Partitions.Count(part => part.Active);
                    if (activePartCounter == 0)
                    {
                        EndUserMessage =
                            "When Using A Standard Partition Layout One Partition With The Operating System Must Be Active.";
                        isSchemaError = true;
                        break;
                    }
                    if (activePartCounter > 1)
                    {
                        EndUserMessage =
                            "When Using A Standard Partition Layout Only One Partition With The Operating System Can Be Active.";
                        isSchemaError = true;
                        break;
                    }
                }
            }

            if (!isSchemaError)
            {
                var result = Call.ImageProfileApi.Put(imageProfile.Id, imageProfile);
                EndUserMessage = result.Success ? "Successfully Updated Image Profile" : result.ErrorMessage;
            }
        }

        protected void btnVG_Click(object sender, EventArgs e)
        {
            var control = sender as Control;
            if (control == null) return;
            var gvRow = (GridViewRow) control.Parent.Parent;
            var gv = (GridView) gvRow.FindControl("gvLVS");

            var selectedHd = (string) ViewState["selectedHD"];

            var btn = (LinkButton) gvRow.FindControl("vgClick");
            if (gv.Visible == false)
            {
                gv.Visible = true;

                var td = gvRow.FindControl("tdLVS");
                td.Visible = true;
                var schemaRequestOptions = new ImageSchemaRequestDTO();
                schemaRequestOptions.image = null;
                schemaRequestOptions.imageProfile = ImageProfile;
                schemaRequestOptions.schemaType = "deploy";
                schemaRequestOptions.selectedHd = selectedHd;
                gv.DataSource = Call.ImageSchemaApi.GetLogicalVolumes(schemaRequestOptions);
                gv.DataBind();
                btn.Text = "-";
            }

            else
            {
                gv.Visible = false;
                var td = gvRow.FindControl("tdLVS");
                td.Visible = false;
                btn.Text = "+";
            }

            if (ddlObject.Text != "Dynamic")
            {
                foreach (GridViewRow lv in gv.Rows)
                {
                    var lvTxtCustomSize = lv.FindControl("txtCustomSize") as TextBox;
                    if (lvTxtCustomSize != null)
                        lvTxtCustomSize.Enabled = false;

                    var lvDdlUnit = lv.FindControl("ddlUnit") as DropDownList;
                    if (lvDdlUnit != null)
                        lvDdlUnit.Enabled = false;

                    var lvChkFixed = lv.FindControl("chkFixed") as CheckBox;
                    if (lvChkFixed != null)
                        lvChkFixed.Enabled = false;
                }
            }
        }

        protected void chkForceDynamic_OnCheckedChanged(object sender, EventArgs e)
        {
            if (chkDownForceDynamic.Checked)
            {
                ddlPartitionMethodLin.Enabled = false;
                ddlPartitionMethodLin.Text = "Dynamic";
            }
            else
            {
                ddlPartitionMethodLin.Enabled = true;
            }
        }

        protected void chkModifySchema_OnCheckedChanged(object sender, EventArgs e)
        {
            if (chkModifySchema.Checked)
            {
                imageSchema.Visible = true;
                PopulateHardDrives();
            }
            else
            {
                imageSchema.Visible = false;
            }
        }

        protected void ddlPartitionMethod_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (Image.Environment == "winpe")
            {
                ForceDiv.Visible = false;
                if (ddlObject.Text == "Standard" || ddlObject.Text == "Standard Core Storage")
                {
                    chkModifySchema.Checked = true;
                    chkModifySchema.Enabled = false;
                }
            }
            else
            {
                ForceDiv.Visible = ddlPartitionMethodLin.Text == "Dynamic";
            }
            DisplayLayout();
        }

        protected void DisplayLayout()
        {
            switch (ddlObject.Text)
            {
                case "Custom Script":
                    customScript.Visible = true;
                    scriptEditorText.Value = ImageProfile.CustomPartitionScript;
                    if (!string.IsNullOrEmpty(ImageProfile.CustomSchema) || chkModifySchema.Checked)
                    {
                        chkModifySchema.Checked = true;
                        imageSchema.Visible = true;
                        PopulateHardDrives();
                    }
                    break;

                default:
                    customScript.Visible = false;

                    if (!string.IsNullOrEmpty(ImageProfile.CustomSchema) || chkModifySchema.Checked)
                    {
                        chkModifySchema.Checked = true;
                        imageSchema.Visible = true;
                        PopulateHardDrives();
                    }

                    break;
            }
        }

        protected void lnkExport_OnClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ImageProfile.CustomSchema))
                EndUserMessage = "You Must Update The Schema First";
            else
            {
                Response.ContentType = "text/plain";
                Response.AppendHeader("Content-Disposition", "attachment; filename=schema.txt");
                Response.Write(ImageProfile.CustomSchema);
                Response.End();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Image.Environment == "macOS")
                ddlObject = ddlPartitionMethodMac;
            else if (Image.Environment == "winpe")
                ddlObject = ddlPartitionMethodWin;
            else
                ddlObject = ddlPartitionMethodLin;

            if (IsPostBack) return;
            chkDownNoExpand.Checked = Convert.ToBoolean(ImageProfile.SkipExpandVolumes);
            chkChangeName.Checked = Convert.ToBoolean(ImageProfile.ChangeName);
            chkAlignBCD.Checked = Convert.ToBoolean(ImageProfile.FixBcd);
            chkRunFixBoot.Checked = Convert.ToBoolean(ImageProfile.FixBootloader);

            if (Image.Environment == "macOS")
            {
                divBoot.Visible = false;
                divExpandVol.Visible = false;
                ForceDiv.Visible = false;
                DivPartDdlMac.Visible = true;
                ddlPartitionMethodMac.Text = ImageProfile.PartitionMethod;
            }
            else if (Image.Environment == "winpe")
            {
                divExpandVol.Visible = false;
                divBoot.Visible = false;
                ForceDiv.Visible = false;
                divOsx.Visible = false;
                DivPartDdlWin.Visible = true;
                ddlPartitionMethodWin.Text = ImageProfile.PartitionMethod;
            }
            else if (Image.Environment == "linux" || Image.Environment == "")
            {
                if (Image.Type == "File")
                    divExpandVol.Visible = false;
                divOsx.Visible = false;
                DivPartDdlLin.Visible = true;
                ddlPartitionMethodLin.Text = ImageProfile.PartitionMethod;
                if (chkDownForceDynamic.Checked) ddlPartitionMethodLin.Enabled = false;
                ForceDiv.Visible = ddlPartitionMethodLin.Text == "Dynamic";
            }
            chkDownForceDynamic.Checked = Convert.ToBoolean(ImageProfile.ForceDynamicPartitions);
            chkInstallMunki.Checked = Convert.ToBoolean(ImageProfile.OsxInstallMunki);
            txtMunkiRepoUrl.Text = ImageProfile.MunkiRepoUrl;
            txtTargetVolume.Text = ImageProfile.OsxTargetVolume;
            txtMunkiUsername.Text = ImageProfile.MunkiAuthUsername;

            DisplayLayout();
        }

        protected void PopulateHardDrives()
        {
            var schemaRequestOptions = new ImageSchemaRequestDTO();
            schemaRequestOptions.image = null;
            schemaRequestOptions.imageProfile = ImageProfile;
            schemaRequestOptions.schemaType = "deploy";

            gvHDs.DataSource = Call.ImageSchemaApi.GetHardDrives(schemaRequestOptions);
            gvHDs.DataBind();
        }

        protected string SetCustomSchema()
        {
            var schemaRequestOptions = new ImageSchemaRequestDTO();
            schemaRequestOptions.image = null;
            schemaRequestOptions.imageProfile = ImageProfile;
            schemaRequestOptions.schemaType = "deploy";

            var schema = Call.ImageSchemaApi.GetSchema(schemaRequestOptions);

            var rowCounter = 0;
            foreach (GridViewRow row in gvHDs.Rows)
            {
                var box = row.FindControl("chkHDActive") as CheckBox;
                if (box != null)
                    schema.HardDrives[rowCounter].Active = box.Checked;

                var txtDestination = row.FindControl("txtDestination") as TextBox;
                if (txtDestination != null)
                    schema.HardDrives[rowCounter].Destination = txtDestination.Text;

                var gvParts = (GridView) row.FindControl("gvParts");
                var partCounter = 0;
                foreach (GridViewRow partRow in gvParts.Rows)
                {
                    var boxPart = partRow.FindControl("chkPartActive") as CheckBox;
                    if (boxPart != null)
                        schema.HardDrives[rowCounter].Partitions[partCounter].Active = boxPart.Checked;

                    var txtCustomSize = partRow.FindControl("txtCustomSize") as TextBox;
                    if (txtCustomSize != null)
                        schema.HardDrives[rowCounter].Partitions[partCounter].CustomSize = txtCustomSize.Text;

                    var ddlUnit = partRow.FindControl("ddlUnit") as DropDownList;
                    if (ddlUnit != null)
                        schema.HardDrives[rowCounter].Partitions[partCounter].CustomSizeUnit = ddlUnit.Text;

                    var chkFixed = partRow.FindControl("chkFixed") as CheckBox;
                    if (chkFixed != null)
                        schema.HardDrives[rowCounter].Partitions[partCounter].ForceFixedSize = chkFixed.Checked;

                    var gvVg = (GridView) partRow.FindControl("gvVG");

                    foreach (GridViewRow vg in gvVg.Rows)
                    {
                        var gvLvs = (GridView) vg.FindControl("gvLVS");
                        var lvCounter = 0;
                        foreach (GridViewRow lv in gvLvs.Rows)
                        {
                            var lvBoxPart = lv.FindControl("chkPartActive") as CheckBox;
                            if (lvBoxPart != null)
                                schema.HardDrives[rowCounter].Partitions[partCounter].VolumeGroup.LogicalVolumes[
                                    lvCounter]
                                    .Active = lvBoxPart.Checked;

                            var lvTxtCustomSize = lv.FindControl("txtCustomSize") as TextBox;
                            if (lvTxtCustomSize != null)
                                schema.HardDrives[rowCounter].Partitions[partCounter].VolumeGroup.LogicalVolumes[
                                    lvCounter]
                                    .CustomSize = lvTxtCustomSize.Text;

                            var lvDdlUnit = lv.FindControl("ddlUnit") as DropDownList;
                            if (lvDdlUnit != null)
                                schema.HardDrives[rowCounter].Partitions[partCounter].VolumeGroup.LogicalVolumes[
                                    lvCounter]
                                    .CustomSizeUnit = lvDdlUnit.Text;

                            var lvChkFixed = lv.FindControl("chkFixed") as CheckBox;
                            if (lvChkFixed != null)
                                schema.HardDrives[rowCounter].Partitions[partCounter].VolumeGroup.LogicalVolumes[
                                    lvCounter]
                                    .ForceFixedSize = lvChkFixed.Checked;
                            lvCounter++;
                        }
                    }
                    partCounter++;
                }
                rowCounter++;
            }
            return JsonConvert.SerializeObject(schema,
                new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
        }
    }
}