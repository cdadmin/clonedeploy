using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using BasePages;
using BLL;

public partial class views_images_profiles_scripts : Images
{
    private readonly ImageProfileScript _bllImageProfileScript = new ImageProfileScript();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;

        PopulateGrid();
    }

    protected void PopulateGrid()
    {

        gvScripts.DataSource = new Script().SearchScripts("");
        gvScripts.DataBind();

        var profileScripts = _bllImageProfileScript.SearchImageProfileScripts(ImageProfile.Id);
        foreach (GridViewRow row in gvScripts.Rows)
        {
            var pre = (CheckBox)row.FindControl("chkPre");
            var post = (CheckBox)row.FindControl("chkPost");
            var dataKey = gvScripts.DataKeys[row.RowIndex];
            if (dataKey == null) continue;
            foreach (var profileScript in profileScripts)
            {
                if (profileScript.ScriptId == Convert.ToInt16(dataKey.Value))
                {
                    pre.Checked = Convert.ToBoolean(profileScript.RunPre);
                    post.Checked = Convert.ToBoolean(profileScript.RunPost);
                }
            }
        }

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
        List<Models.Script> listScripts = (List<Models.Script>)gvScripts.DataSource;
        switch (e.SortExpression)
        {
            case "Name":
                listScripts = GetSortDirection(e.SortExpression) == "Asc" ? listScripts.OrderBy(s => s.Name).ToList() : listScripts.OrderByDescending(s => s.Name).ToList();
                break;
            case "Priority":
                listScripts = GetSortDirection(e.SortExpression) == "Asc" ? listScripts.OrderBy(s => s.Priority).ToList() : listScripts.OrderByDescending(s => s.Priority).ToList();
                break;

        }


        gvScripts.DataSource = listScripts;
        gvScripts.DataBind();
    }
   

    protected void btnUpdateScripts_OnClick(object sender, EventArgs e)
    {
        _bllImageProfileScript.DeleteImageProfileScripts((ImageProfile.Id));
        foreach (GridViewRow row in gvScripts.Rows)
        {
            var pre = (CheckBox)row.FindControl("chkPre");
            var post = (CheckBox)row.FindControl("chkPost");
            if (pre == null || post == null) continue;
            if(!pre.Checked && !post.Checked) continue;
            var dataKey = gvScripts.DataKeys[row.RowIndex];
            if (dataKey == null) continue;

            var profileScript = new Models.ImageProfileScript()
            {
                ScriptId = Convert.ToInt16(dataKey.Value),
                ProfileId = ImageProfile.Id,
                RunPre = Convert.ToInt16(pre.Checked),
                RunPost = Convert.ToInt16(post.Checked)
            };
            _bllImageProfileScript.AddImageProfileScript(profileScript);
        }
    }
}