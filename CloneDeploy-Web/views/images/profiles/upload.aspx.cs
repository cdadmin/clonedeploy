﻿using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using CloneDeploy_Common;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Entities.DTOs.ImageSchemaFE;
using CloneDeploy_Web.BasePages;
using Newtonsoft.Json;

namespace CloneDeploy_Web.views.images.profiles
{
    public partial class views_images_profiles_upload : Images
    {
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
            schemaRequestOptions.schemaType = "upload";
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

        protected void btnUpdateUpload_OnClick(object sender, EventArgs e)
        {
            RequiresAuthorizationOrManagedImage(AuthorizationStrings.UpdateProfile, Image.Id);
            if (chkCustomUpload.Checked && chkSchemaOnly.Checked)
            {
                EndUserMessage = "Custom Schema And Upload Schema Only Cannot Both Be Checked";
                return;
            }
            var imageProfile = ImageProfile;
            imageProfile.RemoveGPT = Convert.ToInt16(chkRemoveGpt.Checked);
            imageProfile.SkipShrinkVolumes = Convert.ToInt16(chkUpNoShrink.Checked);
            imageProfile.SkipShrinkLvm = Convert.ToInt16(chkUpNoShrinkLVM.Checked);
            imageProfile.CustomUploadSchema = chkCustomUpload.Checked ? SetCustomUploadSchema() : "";
            imageProfile.Compression = ddlCompAlg.Text;
            imageProfile.CompressionLevel = ddlCompLevel.Text;
            imageProfile.UploadSchemaOnly = Convert.ToInt16(chkSchemaOnly.Checked);
            imageProfile.WimMulticastEnabled = Convert.ToInt16(chkWimMulticast.Checked);
            imageProfile.SimpleUploadSchema = Convert.ToInt16(chkSimpleSchema.Checked);
            imageProfile.SkipHibernationCheck = Convert.ToInt16(chkSkipHibernation.Checked);
            imageProfile.SkipBitlockerCheck = Convert.ToInt16(chkSkipBitlocker.Checked);
            var result = Call.ImageProfileApi.Put(imageProfile.Id, imageProfile);
            EndUserMessage = result.Success ? "Successfully Updated Image Profile" : result.ErrorMessage;
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
                schemaRequestOptions.schemaType = "upload";
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
        }

        protected void chkCustomUpload_OnCheckedChanged(object sender, EventArgs e)
        {
            if (chkCustomUpload.Checked)
            {
                imageSchema.Visible = true;
                PopulateHardDrives();
            }
            else
            {
                imageSchema.Visible = false;
            }
        }

        protected void DisplayLayout()
        {
          
            if (Image.Environment == "winpe")
            {
                divCompression.Visible = false;
                divGpt.Visible = false;
                divShrink.Visible = false;
                DivSimple.Visible = false;
            }
            else if (Image.Environment == "linux" && Image.Type == "Block" ||
                     Image.Environment == "" && Image.Type == "Block")
            {
                divWimMulticast.Visible = false;
                DivSimple.Visible = false;
            }
            else if (Image.Environment == "linux" && Image.Type == "File" ||
                     Image.Environment == "" && Image.Type == "File")
            {
                divCompression.Visible = false;
                divShrink.Visible = false;
                DivSimple.Visible = false;
            }
            chkRemoveGpt.Checked = Convert.ToBoolean(ImageProfile.RemoveGPT);
            chkUpNoShrink.Checked = Convert.ToBoolean(ImageProfile.SkipShrinkVolumes);
            chkUpNoShrinkLVM.Checked = Convert.ToBoolean(ImageProfile.SkipShrinkLvm);
            chkSimpleSchema.Checked = Convert.ToBoolean(ImageProfile.SimpleUploadSchema);
            ddlCompAlg.Text = ImageProfile.Compression;
            ddlCompLevel.Text = ImageProfile.CompressionLevel;
            chkSchemaOnly.Checked = Convert.ToBoolean(ImageProfile.UploadSchemaOnly);
            chkWimMulticast.Checked = Convert.ToBoolean(ImageProfile.WimMulticastEnabled);
            chkSkipHibernation.Checked = Convert.ToBoolean(ImageProfile.SkipHibernationCheck);
            chkSkipBitlocker.Checked = Convert.ToBoolean(ImageProfile.SkipBitlockerCheck);
            if (!string.IsNullOrEmpty(ImageProfile.CustomUploadSchema))
            {
                chkCustomUpload.Checked = true;
                imageSchema.Visible = true;
                PopulateHardDrives();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) DisplayLayout();
        }

        protected void PopulateHardDrives()
        {
            var schemaRequestOptions = new ImageSchemaRequestDTO();
            schemaRequestOptions.image = null;
            schemaRequestOptions.imageProfile = ImageProfile;
            schemaRequestOptions.schemaType = "upload";
            gvHDs.DataSource = Call.ImageSchemaApi.GetHardDrives(schemaRequestOptions);
            gvHDs.DataBind();
        }

        protected string SetCustomUploadSchema()
        {
            var schemaRequestOptions = new ImageSchemaRequestDTO();
            schemaRequestOptions.image = null;
            schemaRequestOptions.imageProfile = ImageProfile;
            schemaRequestOptions.schemaType = "upload";
            var schema = Call.ImageSchemaApi.GetSchema(schemaRequestOptions);

            var rowCounter = 0;
            foreach (GridViewRow row in gvHDs.Rows)
            {
                var box = row.FindControl("chkHDActive") as CheckBox;
                if (box != null)
                    schema.HardDrives[rowCounter].Active = box.Checked;

                var gvParts = (GridView) row.FindControl("gvParts");

                var partCounter = 0;
                foreach (GridViewRow partRow in gvParts.Rows)
                {
                    var boxPart = partRow.FindControl("chkPartActive") as CheckBox;
                    if (boxPart != null)
                        schema.HardDrives[rowCounter].Partitions[partCounter].Active = boxPart.Checked;

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
            return JsonConvert.SerializeObject(schema);
        }
    }
}