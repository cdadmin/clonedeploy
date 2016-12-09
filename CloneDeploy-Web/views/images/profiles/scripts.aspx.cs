using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CloneDeploy_Entities;
using CloneDeploy_Web;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

public partial class views_images_profiles_scripts : Images
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;

        PopulateGrid();
    }

    protected void PopulateProfileScripts()
    {
        var profileScripts = Call.ImageProfileApi.GetScripts(ImageProfile.Id);
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
        gvScripts.DataSource = Call.ScriptApi.GetAll(Int32.MaxValue, "");
        gvScripts.DataBind();
        PopulateProfileScripts();
    }

   
    protected void gridView_Sorting(object sender, GridViewSortEventArgs e)
    {
        PopulateGrid();
        List<ScriptEntity> listScripts = (List<ScriptEntity>)gvScripts.DataSource;
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
        var deleteResult = Call.ImageProfileApi.RemoveProfileScripts(ImageProfile.Id);
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
           
            var profileScript = new ImageProfileScriptEntity()
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
            EndUserMessage = Call.ImageProfileScriptApi.Post(profileScript).Success
                ? "Successfully Updated Image Profile"
                : "Could Not Update Image Profile";
        }
        if (checkedCount == 0)
        {
            EndUserMessage = deleteResult.Success ? "Successfully Updated Image Profile" : "Could Not Update Image Profile";
        }
    }
}