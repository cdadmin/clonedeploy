using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BasePages;


public partial class views_images_profiles_search : Images
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;

        PopulateGrid();
    }

    protected void ButtonConfirmDelete_Click(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.DeleteProfile);
        var deleteCounter = 0;
        foreach (GridViewRow row in gvProfiles.Rows)
        {
            var cb = (CheckBox)row.FindControl("chkSelector");
            if (cb == null || !cb.Checked) continue;
            var dataKey = gvProfiles.DataKeys[row.RowIndex];
            if (dataKey == null) continue;
            if (BLL.ImageProfile.DeleteProfile(Convert.ToInt32(dataKey.Value)))
                deleteCounter++;
        }
        EndUserMessage = "Successfully Deleted " + deleteCounter + " Profiles";
        PopulateGrid();
    }

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        var hcb = (CheckBox)gvProfiles.HeaderRow.FindControl("chkSelectAll");

        ToggleCheckState(hcb.Checked);
    }

   
    protected void gridView_Sorting(object sender, GridViewSortEventArgs e)
    {
        PopulateGrid();
        var listProfiles = (List<ImageProfile>)gvProfiles.DataSource;
        switch (e.SortExpression)
        {
            case "Name":
                listProfiles = GetSortDirection(e.SortExpression) == "Asc" ? listProfiles.OrderBy(h => h.Name).ToList() : listProfiles.OrderByDescending(h => h.Name).ToList();
                break;

        }


        gvProfiles.DataSource = listProfiles;
        gvProfiles.DataBind();
    }

    protected void PopulateGrid()
    {
        gvProfiles.DataSource = BLL.ImageProfile.SearchProfiles(Image.Id);
        gvProfiles.DataBind();
            
        foreach (GridViewRow row in gvProfiles.Rows)
        {
            var lblClient = row.FindControl("lblSizeClient") as Label;
            if (lblClient != null)
            {
                var dataKey = gvProfiles.DataKeys[row.RowIndex];
                if (dataKey == null) continue;
                lblClient.Text = BLL.ImageSchema.MinimumClientSizeForGridView(Convert.ToInt32(dataKey.Value), 0);
            }
        }

    }

    protected void search_Changed(object sender, EventArgs e)
    {
        PopulateGrid();
    }

    private void ToggleCheckState(bool checkState)
    {
        foreach (GridViewRow row in gvProfiles.Rows)
        {
            var cb = (CheckBox)row.FindControl("chkSelector");
            if (cb != null)
                cb.Checked = checkState;
        }
    }

    protected void btnHds_Click(object sender, EventArgs e)
    {
        var control = sender as Control;
        if (control == null) return;
        var row = (GridViewRow)control.Parent.Parent;
        var gvHDs = (GridView)row.FindControl("gvHDs");
        var btn = (LinkButton)row.FindControl("btnHDs");

        if (gvHDs.Visible == false)
        {
            var td = row.FindControl("tdHds");
            td.Visible = true;
            gvHDs.Visible = true;

            gvHDs.DataSource = new BLL.ImageSchema(null, "deploy", BLL.Image.GetImage(Image.Id)).GetHardDrivesForGridView();
            gvHDs.DataBind();
            btn.Text = "-";
        }
        else
        {
            var td = row.FindControl("tdHds");
            td.Visible = false;
            gvHDs.Visible = false;
            btn.Text = "+";
        }

        foreach (GridViewRow hdrow in gvHDs.Rows)
        {
            var selectedHd = (hdrow.RowIndex);
            var lblClient = hdrow.FindControl("lblHDSizeClient") as Label;
            if (lblClient != null)
            {
                var dataKey = gvProfiles.DataKeys[row.RowIndex];
                if (dataKey == null) continue;
                lblClient.Text = BLL.ImageSchema.MinimumClientSizeForGridView(Convert.ToInt32(dataKey.Value), selectedHd);
            }
        }
    }

    protected void profileClone_OnClick(object sender, EventArgs e)
    {
        var control = sender as Control;
        if (control != null)
        {
            var gvRow = (GridViewRow)control.Parent.Parent;
            var dataKey = gvProfiles.DataKeys[gvRow.RowIndex];
            if (dataKey != null)
            {
                var imageProfile = BLL.ImageProfile.ReadProfile(Convert.ToInt32(dataKey.Value));
                BLL.ImageProfile.CloneProfile(imageProfile);
            }
        }
        PopulateGrid();
    }
}