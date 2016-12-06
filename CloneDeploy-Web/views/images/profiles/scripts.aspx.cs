using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using BasePages;

public partial class views_images_profiles_scripts : Images
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;

        PopulateGrid();
    }

    protected void PopulateProfileScripts()
    {
        var profileScripts = BLL.ImageProfileScript.SearchImageProfileScripts(ImageProfile.Id);
        foreach (GridViewRow row in gvScripts.Rows)
        {
            var pre = (CheckBox)row.FindControl("chkPre");
            var post = (CheckBox)row.FindControl("chkPost");
            var dataKey = gvScripts.DataKeys[row.RowIndex];
            if (dataKey == null) continue;
            foreach (var profileScript in profileScripts)
            {
                if (profileScript.ScriptId == Convert.ToInt32(dataKey.Value))
                {
                    pre.Checked = Convert.ToBoolean(profileScript.RunPre);
                    post.Checked = Convert.ToBoolean(profileScript.RunPost);
                    var txtPriority = row.FindControl("txtPriority") as TextBox;
                    if (txtPriority != null)
                        txtPriority.Text = profileScript.Priority.ToString();
                }
            }
        }
    }

    protected void PopulateGrid()
    {
        gvScripts.DataSource = BLL.Script.SearchScripts("");
        gvScripts.DataBind();
        PopulateProfileScripts();
    }

   
    protected void gridView_Sorting(object sender, GridViewSortEventArgs e)
    {
        PopulateGrid();
        List<Script> listScripts = (List<Script>)gvScripts.DataSource;
        switch (e.SortExpression)
        {
            case "Name":
                listScripts = GetSortDirection(e.SortExpression) == "Asc" ? listScripts.OrderBy(s => s.Name).ToList() : listScripts.OrderByDescending(s => s.Name).ToList();
                break;
        }

        gvScripts.DataSource = listScripts;
        gvScripts.DataBind();
        PopulateProfileScripts();
    }
   

    protected void btnUpdateScripts_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorizationOrManagedImage(Authorizations.UpdateProfile, Image.Id);
        var deleteResult = BLL.ImageProfileScript.DeleteImageProfileScripts((ImageProfile.Id));
        var checkedCount = 0;
        foreach (GridViewRow row in gvScripts.Rows)
        {
            var pre = (CheckBox)row.FindControl("chkPre");
            var post = (CheckBox)row.FindControl("chkPost");
            if (pre == null || post == null) continue;
            if (!pre.Checked && !post.Checked) continue;
            checkedCount++;

            var dataKey = gvScripts.DataKeys[row.RowIndex];
            if (dataKey == null) continue;
           
            var profileScript = new ImageProfileScript()
            {
                ScriptId = Convert.ToInt32(dataKey.Value),
                ProfileId = ImageProfile.Id,
                RunPre = Convert.ToInt16(pre.Checked),
                RunPost = Convert.ToInt16(post.Checked)
            };
            var txtPriority = row.FindControl("txtPriority") as TextBox;
            if(txtPriority != null)
                if (!string.IsNullOrEmpty(txtPriority.Text))
                    profileScript.Priority = Convert.ToInt32(txtPriority.Text);
            EndUserMessage = BLL.ImageProfileScript.AddImageProfileScript(profileScript)
                ? "Successfully Updated Image Profile"
                : "Could Not Update Image Profile";
        }
        if (checkedCount == 0)
        {
            EndUserMessage = deleteResult ? "Successfully Updated Image Profile" : "Could Not Update Image Profile";
        }
    }
}