using System;
using System.Web.UI.WebControls;
using CloneDeploy_Web;

public partial class views_global_profileupdater_kernel : BasePages.Global
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           
            PopulateGrid();
        }
    }

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        ChkAll(gvProfiles);
    }

    protected void PopulateGrid()
    {
        ddlKernel.DataSource = Call.FilesystemApi.GetKernels();
        ddlKernel.DataBind();
        ddlKernel.SelectedValue = Settings.DefaultKernel64;
        gvProfiles.DataSource = Call.ImageProfileApi.GetAll(Int32.MaxValue, "");
        gvProfiles.DataBind();
    }

    protected void btnUpdateKernel_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.UpdateGlobal);
        var updateCount = 0;
        foreach (GridViewRow row in gvProfiles.Rows)
        {
            var cb = (CheckBox)row.FindControl("chkSelector");
            if (cb == null || !cb.Checked) continue;
            var dataKey = gvProfiles.DataKeys[row.RowIndex];
            if (dataKey == null) continue;

            var imageProfile = Call.ImageProfileApi.Get(Convert.ToInt32(dataKey.Value));
            imageProfile.Kernel = ddlKernel.Text;
            if (Call.ImageProfileApi.Put(imageProfile.Id,imageProfile).Success)
            {
                updateCount++;
            }
        }
        EndUserMessage = "Updated " + updateCount + " Image Profile(s)";
        PopulateGrid();
    }
}