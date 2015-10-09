using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using BasePages;
using BLL;
using LinuxProfile = Models.LinuxProfile;

public partial class views_images_profiles_search : Images
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;

        PopulateGrid();
    }

    protected void ButtonConfirmDelete_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gvProfiles.Rows)
        {
            var cb = (CheckBox)row.FindControl("chkSelector");
            if (cb == null || !cb.Checked) continue;
            var dataKey = gvProfiles.DataKeys[row.RowIndex];
            if (dataKey == null) continue;
            BLL.Computer.DeleteComputer(Convert.ToInt32(dataKey.Value));
        }

        PopulateGrid();
    }

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        var hcb = (CheckBox)gvProfiles.HeaderRow.FindControl("chkSelectAll");

        ToggleCheckState(hcb.Checked);
    }

    public string GetSortDirection(string sortExpression)
    {
        if (ViewState[sortExpression] == null)
            ViewState[sortExpression] = "Desc";
        else
            ViewState[sortExpression] = ViewState[sortExpression].ToString() == "Desc" ? "Asc" : "Desc";

        return ViewState[sortExpression].ToString();
    }

    protected void gridView_Sorting(object sender, GridViewSortEventArgs e)
    {
        PopulateGrid();
        IEnumerable<LinuxProfile> listProfiles = (List<LinuxProfile>)gvProfiles.DataSource;
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
        gvProfiles.DataSource = BLL.LinuxProfile.SearchProfiles(Image.Id);
        gvProfiles.DataBind();

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
}