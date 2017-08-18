using System;
using System.Web.UI.WebControls;
using CloneDeploy_Common;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.global.profileupdater
{
    public partial class views_global_profileupdater_kernel : Admin
    {
        protected void btnUpdateKernel_OnClick(object sender, EventArgs e)
        {
            RequiresAuthorization(AuthorizationStrings.UpdateAdmin);
            var updateCount = 0;
            foreach (GridViewRow row in gvProfiles.Rows)
            {
                var cb = (CheckBox) row.FindControl("chkSelector");
                if (cb == null || !cb.Checked) continue;
                var dataKey = gvProfiles.DataKeys[row.RowIndex];
                if (dataKey == null) continue;

                var imageProfile = Call.ImageProfileApi.Get(Convert.ToInt32(dataKey.Value));
                imageProfile.Kernel = ddlKernel.Text;
                if (Call.ImageProfileApi.Put(imageProfile.Id, imageProfile).Success)
                {
                    updateCount++;
                }
            }
            EndUserMessage = "Updated " + updateCount + " Image Profile(s)";
            PopulateGrid();
        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            ChkAll(gvProfiles);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateGrid();
            }
        }

        protected void PopulateGrid()
        {
            ddlKernel.DataSource = Call.FilesystemApi.GetKernels();
            ddlKernel.DataBind();
            ddlKernel.SelectedValue = SettingStrings.DefaultKernel64;
            gvProfiles.DataSource = Call.ImageProfileApi.Get(int.MaxValue, "");
            gvProfiles.DataBind();
        }
    }
}