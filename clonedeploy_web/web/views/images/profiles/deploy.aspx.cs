using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BasePages;
using BLL;
using Newtonsoft.Json;

public partial class views_images_profiles_deploy : Images
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            chkDownNoExpand.Checked = Convert.ToBoolean(ImageProfile.SkipExpandVolumes);
            chkAlignBCD.Checked = Convert.ToBoolean(ImageProfile.FixBcd);
            chkRunFixBoot.Checked = Convert.ToBoolean(ImageProfile.FixBootloader);
            ddlPartitionMethod.Text = ImageProfile.PartitionMethod;
            DisplayLayout();
        }

    }

    protected void btnUpdateDeploy_OnClick(object sender, EventArgs e)
    {
        var imageProfile = ImageProfile;
        imageProfile.SkipExpandVolumes = Convert.ToInt16(chkDownNoExpand.Checked);
        imageProfile.FixBcd = Convert.ToInt16(chkAlignBCD.Checked);
        imageProfile.FixBootloader = Convert.ToInt16(chkRunFixBoot.Checked);
        BLL.LinuxProfile.UpdateProfile(imageProfile);
    }


    protected void ddlPartitionMethod_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        DisplayLayout();
    }

    protected void DisplayLayout()
    {
        switch (ddlPartitionMethod.SelectedIndex)
        {
            case 1:
                dynamicPartition.Visible = true;
                customScript.Visible = false;
                customLayout.Visible = false;

                if (!string.IsNullOrEmpty(ImageProfile.CustomSchema))
                {
                    chkModifySchema.Checked = true;
                    imageSchema.Visible = true;
                    PopulateHardDrives();
                }
                break;
            case 2:
                customScript.Visible = true;
                customLayout.Visible = false;
                dynamicPartition.Visible = false;
                scriptEditorText.Value = ImageProfile.CustomPartitionScript;
                break;
            case 3:
                customScript.Visible = false;
                customLayout.Visible = true;
                dynamicPartition.Visible = false;
                PopulateGrid();
                var profilePartitionLayouts = BLL.ImageProfilePartition.SearchImageProfilePartitions(ImageProfile.Id);
                foreach (GridViewRow row in gvLayout.Rows)
                {
                    var cb = (CheckBox)row.FindControl("chkSelector");
                    var dataKey = gvLayout.DataKeys[row.RowIndex];
                    if (dataKey == null) continue;
                    foreach (var profilePartitionLayout in profilePartitionLayouts)
                    {
                        if (profilePartitionLayout.LayoutId == Convert.ToInt16(dataKey.Value))
                            cb.Checked = true;
                    }
                }
                break;
            default:
                customScript.Visible = false;
                customLayout.Visible = false;
                dynamicPartition.Visible = false;
                break;
        }
    }

    protected void PopulateGrid()
    {
        gvLayout.DataSource = BLL.PartitionLayout.SearchPartitionLayouts("");
        gvLayout.DataBind();
    }

    protected void gridView_Sorting(object sender, GridViewSortEventArgs e)
    {
        PopulateGrid();
        List<Models.PartitionLayout> listLayouts = (List<Models.PartitionLayout>)gvLayout.DataSource;
        switch (e.SortExpression)
        {
            case "Name":
                listLayouts = GetSortDirection(e.SortExpression) == "Asc" ? listLayouts.OrderBy(l => l.Name).ToList() : listLayouts.OrderByDescending(l => l.Name).ToList();
                break;
            case "Table":
                listLayouts = GetSortDirection(e.SortExpression) == "Asc" ? listLayouts.OrderBy(l => l.Table).ToList() : listLayouts.OrderByDescending(l => l.Table).ToList();
                break;
            case "ImageEnvironment":
                listLayouts = GetSortDirection(e.SortExpression) == "Asc" ? listLayouts.OrderBy(l => l.ImageEnvironment).ToList() : listLayouts.OrderByDescending(l => l.ImageEnvironment).ToList();
                break;

        }


        gvLayout.DataSource = listLayouts;
        gvLayout.DataBind();
    }
   
    protected void btnHd_Click(object sender, EventArgs e)
    {
        var control = sender as Control;
        if (control == null) return;
        var gvRow = (GridViewRow)control.Parent.Parent;
        var gv = (GridView)gvRow.FindControl("gvParts");

        var selectedHd = gvRow.Cells[3].Text;
        ViewState["selectedHD"] = gvRow.RowIndex.ToString();
        ViewState["selectedHDName"] = selectedHd;


        var partitions = new ImageSchema(ImageProfile).GetPartitionsForGridView(selectedHd);
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
            var isActive = Convert.ToBoolean(((HiddenField)row.FindControl("HiddenActivePart")).Value);
            if (!isActive) continue;
            var box = row.FindControl("chkPartActive") as CheckBox;
            if (box != null) box.Checked = true;

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
            gv.DataSource = new ImageSchema(ImageProfile).GetLogicalVolumesForGridView(selectedHd);
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

        foreach (var box in (from GridViewRow row in gv.Rows
                             let isActive = Convert.ToBoolean(((HiddenField)row.FindControl("HiddenActivePart")).Value)
                             where isActive
                             select row.FindControl("chkPartActive")).OfType<CheckBox>())
        {
            box.Checked = true;
        }
    }

    protected void PopulateHardDrives()
    {
        gvHDs.DataSource = new ImageSchema(ImageProfile).GetHardDrivesForGridView();
        gvHDs.DataBind();


        foreach (var box in (from GridViewRow row in gvHDs.Rows
                             let isActive = Convert.ToBoolean(((HiddenField)row.FindControl("HiddenActive")).Value)
                             where isActive
                             select row.FindControl("chkHDActive")).OfType<CheckBox>())
        {
            box.Checked = true;
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

    protected void btnUpdateImageSpecs_Click(object sender, EventArgs e)
    {
        var schema = new BLL.ImageSchema(ImageProfile).GetImageSchema();

        var rowCounter = 0;
        foreach (GridViewRow row in gvHDs.Rows)
        {
            var box = row.FindControl("chkHDActive") as CheckBox;
            schema.HardDrives[rowCounter].Active = box != null && box.Checked;

            var gvParts = (GridView)row.FindControl("gvParts");

            var partCounter = 0;
            foreach (GridViewRow partRow in gvParts.Rows)
            {
                var boxPart = partRow.FindControl("chkPartActive") as CheckBox;
                if (boxPart != null && boxPart.Checked)
                    schema.HardDrives[rowCounter].Partitions[partCounter].Active = true;
                else
                    schema.HardDrives[rowCounter].Partitions[partCounter].Active = false;

                var txtCustomSize = partRow.FindControl("txtCustomSize") as TextBox;
                if (txtCustomSize != null && !string.IsNullOrEmpty(txtCustomSize.Text))
                {
                    var customSizeBlk =
                        (Convert.ToInt64(txtCustomSize.Text) * 1024 * 1024 / Convert.ToInt32(schema.HardDrives[rowCounter].Lbs))
                            .ToString
                            ();
                    schema.HardDrives[rowCounter].Partitions[partCounter].CustomSize = customSizeBlk;
                }


                var gvVg = (GridView)partRow.FindControl("gvVG");
                foreach (GridViewRow vg in gvVg.Rows)
                {
                    var gvLvs = (GridView)vg.FindControl("gvLVS");
                    var lvCounter = 0;
                    foreach (GridViewRow lv in gvLvs.Rows)
                    {
                        var boxLv = lv.FindControl("chkPartActive") as CheckBox;
                        if (boxLv != null && boxLv.Checked)
                            schema.HardDrives[rowCounter].Partitions[partCounter].VolumeGroup.LogicalVolumes[lvCounter].Active = true;
                        else
                            schema.HardDrives[rowCounter].Partitions[partCounter].VolumeGroup.LogicalVolumes[lvCounter].Active = false;

                        var txtCustomSizeLv = lv.FindControl("txtCustomSize") as TextBox;
                        if (txtCustomSizeLv != null && !string.IsNullOrEmpty(txtCustomSizeLv.Text))
                        {
                            var customSizeBlk =
                                (Convert.ToInt64(txtCustomSizeLv.Text) * 1024 * 1024 /
                                 Convert.ToInt32(schema.HardDrives[rowCounter].Lbs))
                                    .ToString();
                            schema.HardDrives[rowCounter].Partitions[partCounter].VolumeGroup.LogicalVolumes[lvCounter].CustomSize =
                                customSizeBlk;
                        }
                        lvCounter++;
                    }
                }
                partCounter++;
            }
            rowCounter++;
        }
        ImageProfile.CustomPartitionScript = JsonConvert.SerializeObject(schema);
       
    }
}