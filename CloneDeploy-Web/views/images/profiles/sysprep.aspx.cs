using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.images.profiles
{
    public partial class views_images_profiles_sysprep : Images
    {
        protected void btnUpdateSysprep_OnClick(object sender, EventArgs e)
        {
            RequiresAuthorizationOrManagedImage(AuthorizationStrings.UpdateProfile, Image.Id);
            var deleteResult = Call.ImageProfileApi.RemoveProfileSysprepTags(ImageProfile.Id);
            var checkedCount = 0;
            foreach (GridViewRow row in gvSysprep.Rows)
            {
                var enabled = (CheckBox) row.FindControl("chkEnabled");
                if (enabled == null) continue;
                if (!enabled.Checked) continue;
                checkedCount++;
                var dataKey = gvSysprep.DataKeys[row.RowIndex];
                if (dataKey == null) continue;

                var profileSysPrep = new ImageProfileSysprepTagEntity
                {
                    SysprepId = Convert.ToInt32(dataKey.Value),
                    ProfileId = ImageProfile.Id
                };
                var txtPriority = row.FindControl("txtPriority") as TextBox;
                if (txtPriority != null)
                    if (!string.IsNullOrEmpty(txtPriority.Text))
                        profileSysPrep.Priority = Convert.ToInt32(txtPriority.Text);

                EndUserMessage = Call.ImageProfileSysprepTagApi.Post(profileSysPrep).Success
                    ? "Successfully Updated Image Profile"
                    : "Could Not Update Image Profile";
            }
            if (checkedCount == 0)
            {
                EndUserMessage = deleteResult ? "Successfully Updated Image Profile" : "Could Not Update Image Profile";
            }
        }


        protected void gridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            PopulateGrid();
            var listSysprepTags = (List<SysprepTagEntity>) gvSysprep.DataSource;
            switch (e.SortExpression)
            {
                case "Name":
                    listSysprepTags = GetSortDirection(e.SortExpression) == "Asc"
                        ? listSysprepTags.OrderBy(s => s.Name).ToList()
                        : listSysprepTags.OrderByDescending(s => s.Name).ToList();
                    break;
            }

            gvSysprep.DataSource = listSysprepTags;
            gvSysprep.DataBind();
            PopulateProfileScripts();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            PopulateGrid();
        }

        protected void PopulateGrid()
        {
            gvSysprep.DataSource = Call.SysprepTagApi.Get(int.MaxValue, "");
            gvSysprep.DataBind();
            PopulateProfileScripts();
        }

        protected void PopulateProfileScripts()
        {
            var profileSyspreps = Call.ImageProfileApi.GetSysprepTags(ImageProfile.Id);
            foreach (GridViewRow row in gvSysprep.Rows)
            {
                var enabled = (CheckBox) row.FindControl("chkEnabled");
                var dataKey = gvSysprep.DataKeys[row.RowIndex];
                if (dataKey == null) continue;
                foreach (var profileSysprep in profileSyspreps)
                {
                    if (profileSysprep.SysprepId == Convert.ToInt32(dataKey.Value))
                    {
                        enabled.Checked = true;
                        var txtPriority = row.FindControl("txtPriority") as TextBox;
                        if (txtPriority != null)
                            txtPriority.Text = profileSysprep.Priority.ToString();
                    }
                }
            }
        }
    }
}