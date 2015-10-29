using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BasePages;
using BLL;
using Helpers;

public partial class views_images_profiles_partition : Images
{
    private readonly ImageProfilePartition _bllImageProfilePartition = new ImageProfilePartition();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddlPartitionMethod.Text = ImageProfile.PartitionMethod;
            chkDownForceDynamic.Checked = Convert.ToBoolean(ImageProfile.ForceDynamicPartitions);
          
            DisplayLayout();
        }
    }

    protected void btnUpdatePartitions_OnClick(object sender, EventArgs e)
    {
        var imageProfile = ImageProfile;
        imageProfile.PartitionMethod = ddlPartitionMethod.Text;
        
        switch (ddlPartitionMethod.SelectedIndex)
        {
            case 1:
                imageProfile.ForceDynamicPartitions = Convert.ToInt16(chkDownForceDynamic.Checked);             
                break;
            case 2:
                var fixedLineEnding = scriptEditorText.Value.Replace("\r\n", "\n");
                imageProfile.CustomPartitionScript = fixedLineEnding;
                break;
            case 3:
                BLL.ImageProfilePartition.DeleteImageProfilePartitions(ImageProfile.Id);
                imageProfile.CustomPartitionScript = "";
                foreach (GridViewRow row in gvLayout.Rows)
                {
                    var cb = (CheckBox)row.FindControl("chkSelector");
                    if (cb == null || !cb.Checked) continue;
                    var dataKey = gvLayout.DataKeys[row.RowIndex];
                    if (dataKey == null) continue;
                    var profilePartitionLayout = new Models.ImageProfilePartition()
                    {
                        LayoutId = Convert.ToInt16(dataKey.Value),
                        ProfileId = ImageProfile.Id            
                    };
                    BLL.ImageProfilePartition.AddImageProfilePartition(profilePartitionLayout);
                }
                break;
            default:
                imageProfile.CustomPartitionScript = "";
                break;
        }
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
                    var cb = (CheckBox) row.FindControl("chkSelector");
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

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        var hcb = (CheckBox)gvLayout.HeaderRow.FindControl("chkSelectAll");

        ToggleCheckState(hcb.Checked);
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
    private void ToggleCheckState(bool checkState)
    {
        foreach (GridViewRow row in gvLayout.Rows)
        {
            var cb = (CheckBox)row.FindControl("chkSelector");
            if (cb != null)
                cb.Checked = checkState;
        }
    }

    protected void btnPart_Click(object sender, EventArgs e)
    {
        var selectedHd = (string)(ViewState["selectedHD"]);
        var control = sender as Control;
        if (control == null) return;
        var gvRow = (GridViewRow)control.Parent.Parent;
        var gv = (GridView)gvRow.FindControl("gvFiles");
        var selectedPartition = gvRow.Cells[3].Text;

        var btn = (LinkButton)gvRow.FindControl("partClick");

        if (gv.Visible == false)
        {
            gv.Visible = true;
            var td = gvRow.FindControl("tdFile");
            td.Visible = true;
            gv.DataSource = ImageSchema.GetPartitionImageFileInfoForGridView(Image, selectedHd,
                selectedPartition);
            gv.DataBind();
            btn.Text = "-";
        }
        else
        {
            gv.Visible = false;
            var td = gvRow.FindControl("tdFile");
            td.Visible = false;
            btn.Text = "+";
        }
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

            var isActive = ((HiddenField)row.FindControl("HiddenActivePart")).Value;
            if (isActive != "1") continue;
            var box = row.FindControl("chkPartActive") as CheckBox;
            if (box != null) box.Checked = true;
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
                             let isActive = ((HiddenField)row.FindControl("HiddenActivePart")).Value
                             where isActive == "1"
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
                             let isActive = ((HiddenField)row.FindControl("HiddenActive")).Value
                             where isActive == "1"
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
}