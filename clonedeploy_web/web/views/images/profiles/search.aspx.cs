using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Global;
using Models;

public partial class views_images_profiles_search : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        new Utility().Msgbox(Utility.Message); //For Redirects

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
            new BLL.Computer().DeleteComputer(Convert.ToInt32(dataKey.Value));
        }

        PopulateGrid();
        new Utility().Msgbox(Utility.Message);
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
        IEnumerable<Models.LinuxProfile> listProfiles = (List<Models.LinuxProfile>)gvProfiles.DataSource;
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
        gvProfiles.DataSource = new BLL.LinuxProfile().SearchProfiles(Master.Image.Id);
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