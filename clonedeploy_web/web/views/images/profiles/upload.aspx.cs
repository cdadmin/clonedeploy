using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BasePages;
using BLL;
using Helpers;
using Newtonsoft.Json;

public partial class views_images_profiles_upload : Images
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) DisplayLayout();
    }

    protected void btnUpdateUpload_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorizationOrManagedImage(Authorizations.UpdateProfile, Image.Id);
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
        var result = BLL.ImageProfile.UpdateProfile(imageProfile);
        EndUserMessage = result.IsValid ? "Successfully Updated Image Profile" : result.Message;
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

    protected void btnHd_Click(object sender, EventArgs e)
    {
        var control = sender as Control;
        if (control == null) return;
        var gvRow = (GridViewRow)control.Parent.Parent;
        var gv = (GridView)gvRow.FindControl("gvParts");

        var selectedHd = gvRow.Cells[2].Text;
        ViewState["selectedHD"] = gvRow.RowIndex.ToString();
        ViewState["selectedHDName"] = selectedHd;


        var partitions = new ImageSchema(ImageProfile,"upload").GetPartitionsForGridView(selectedHd);
        var btn = (LinkButton)gvRow.FindControl("btnHd");
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
            var gvVg = (GridView)row.FindControl("gvVG");
            gvVg.DataSource = new List<Models.ImageSchema.GridView.VolumeGroup>
                {
                    partitions[row.RowIndex].VolumeGroup
                };
            gvVg.DataBind();

            gvVg.Visible = true;
            var td = row.FindControl("tdVG");
            td.Visible = true;


        }
    }


    protected void btnVG_Click(object sender, EventArgs e)
    {
        var control = sender as Control;
        if (control == null) return;
        var gvRow = (GridViewRow)control.Parent.Parent;
        var gv = (GridView)gvRow.FindControl("gvLVS");

        var selectedHd = (string)(ViewState["selectedHD"]);


        var btn = (LinkButton)gvRow.FindControl("vgClick");
        if (gv.Visible == false)
        {
            gv.Visible = true;

            var td = gvRow.FindControl("tdLVS");
            td.Visible = true;
            gv.DataSource = new ImageSchema(ImageProfile,"upload").GetLogicalVolumesForGridView(selectedHd);
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

    protected void PopulateHardDrives()
    {
        gvHDs.DataSource = new ImageSchema(ImageProfile,"upload").GetHardDrivesForGridView();
        gvHDs.DataBind();
    }

    protected void DisplayLayout()
    {
        chkRemoveGpt.Checked = Convert.ToBoolean(ImageProfile.RemoveGPT);
        chkUpNoShrink.Checked = Convert.ToBoolean(ImageProfile.SkipShrinkVolumes);
        chkUpNoShrinkLVM.Checked = Convert.ToBoolean(ImageProfile.SkipShrinkLvm);
        ddlCompAlg.Text = ImageProfile.Compression;
        ddlCompLevel.Text = ImageProfile.CompressionLevel;
        chkSchemaOnly.Checked = Convert.ToBoolean(ImageProfile.UploadSchemaOnly);
        if (!string.IsNullOrEmpty(ImageProfile.CustomUploadSchema))
        {
            chkCustomUpload.Checked = true;
            imageSchema.Visible = true;
            PopulateHardDrives();
        }
    }

    protected string SetCustomUploadSchema()
    {
        var schema = new BLL.ImageSchema(ImageProfile,"upload").GetImageSchema();

        var rowCounter = 0;
        foreach (GridViewRow row in gvHDs.Rows)
        {
            var box = row.FindControl("chkHDActive") as CheckBox;
            if (box != null)
                schema.HardDrives[rowCounter].Active = box.Checked;

            var gvParts = (GridView)row.FindControl("gvParts");

            var partCounter = 0;
            foreach (GridViewRow partRow in gvParts.Rows)
            {
                var boxPart = partRow.FindControl("chkPartActive") as CheckBox;
                if (boxPart != null)
                    schema.HardDrives[rowCounter].Partitions[partCounter].Active = boxPart.Checked;

                var chkFixed = partRow.FindControl("chkFixed") as CheckBox;
                if (chkFixed != null)
                    schema.HardDrives[rowCounter].Partitions[partCounter].ForceFixedSize = chkFixed.Checked;

                var gvVg = (GridView)partRow.FindControl("gvVG");
                foreach (GridViewRow vg in gvVg.Rows)
                {
                    var gvLvs = (GridView)vg.FindControl("gvLVS");
                    var lvCounter = 0;
                    foreach (GridViewRow lv in gvLvs.Rows)
                    {
                        var lvBoxPart = lv.FindControl("chkPartActive") as CheckBox;
                        if (lvBoxPart != null)
                            schema.HardDrives[rowCounter].Partitions[partCounter].VolumeGroup.LogicalVolumes[lvCounter].Active = lvBoxPart.Checked;

                        var lvChkFixed = lv.FindControl("chkFixed") as CheckBox;
                        if (lvChkFixed != null)
                            schema.HardDrives[rowCounter].Partitions[partCounter].VolumeGroup.LogicalVolumes[lvCounter].ForceFixedSize = lvChkFixed.Checked;
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