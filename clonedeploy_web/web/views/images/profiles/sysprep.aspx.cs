using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using BasePages;

public partial class views_images_profiles_sysprep : Images
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;

        PopulateGrid();
    }

    protected void PopulateProfileScripts()
    {
        var profileSyspreps = BLL.ImageProfileSysprepTag.SearchImageProfileSysprepTags(ImageProfile.Id);
        foreach (GridViewRow row in gvSysprep.Rows)
        {
            var enabled = (CheckBox)row.FindControl("chkEnabled");
            var dataKey = gvSysprep.DataKeys[row.RowIndex];
            if (dataKey == null) continue;
            foreach (var profileSysprep in profileSyspreps)
            {
                if (profileSysprep.SysprepId == Convert.ToInt16(dataKey.Value))
                {

                    enabled.Checked = true;
                    var txtPriority = row.FindControl("txtPriority") as TextBox;
                    if (txtPriority != null)
                        txtPriority.Text = profileSysprep.Priority.ToString();
                }
            }
        }
    }

    protected void PopulateGrid()
    {
        gvSysprep.DataSource = BLL.SysprepTag.SearchSysprepTags("");
        gvSysprep.DataBind();
        PopulateProfileScripts();
    }


    protected void gridView_Sorting(object sender, GridViewSortEventArgs e)
    {
        PopulateGrid();
        List<Models.SysprepTag> listSysprepTags = (List<Models.SysprepTag>)gvSysprep.DataSource;
        switch (e.SortExpression)
        {
            case "Name":
                listSysprepTags = GetSortDirection(e.SortExpression) == "Asc" ? listSysprepTags.OrderBy(s => s.Name).ToList() : listSysprepTags.OrderByDescending(s => s.Name).ToList();
                break;
        }

        gvSysprep.DataSource = listSysprepTags;
        gvSysprep.DataBind();
        PopulateProfileScripts();
    }


    protected void btnUpdateSysprep_OnClick(object sender, EventArgs e)
    {
        BLL.ImageProfileSysprepTag.DeleteImageProfileSysprepTags(ImageProfile.Id);
        foreach (GridViewRow row in gvSysprep.Rows)
        {
            var enabled = (CheckBox)row.FindControl("chkEnabled");
            if (enabled == null ) continue;
            if (!enabled.Checked) continue;
            var dataKey = gvSysprep.DataKeys[row.RowIndex];
            if (dataKey == null) continue;

            var profileSysPrep = new Models.ImageProfileSysprepTag
            {
                SysprepId = Convert.ToInt16(dataKey.Value),
                ProfileId = ImageProfile.Id,
            };
            var txtPriority = row.FindControl("txtPriority") as TextBox;
            if (txtPriority != null)
                if (!string.IsNullOrEmpty(txtPriority.Text))
                    profileSysPrep.Priority = Convert.ToInt32(txtPriority.Text);
            BLL.ImageProfileSysprepTag.AddImageProfileSysprepTag(profileSysPrep);
        }
    }
}