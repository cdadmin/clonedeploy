using System;
using System.Linq;
using System.Web.UI.WebControls;
using CloneDeploy_Common;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.tasks
{
    public partial class TaskCustom : Tasks
    {
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            RequiresAuthorization(AuthorizationStrings.ImageMulticastTask);
            if (ddlComputerImage.Text == "Select Image") return;
            EndUserMessage = Call.WorkflowApi.StartOnDemandMulticast(Convert.ToInt32(ddlImageProfile.SelectedValue),
                txtClientCount.Text);
        }

        protected void ddlComputerImage_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateImageProfilesDdl(ddlImageProfile, Convert.ToInt32(ddlComputerImage.SelectedValue));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (GetSetting(SettingStrings.OnDemand) != "Enabled")
                Response.Redirect("~/views/dashboard/dash.aspx?access=denied");
            RequiresAuthorization(AuthorizationStrings.AllowOnd);
            if (!IsPostBack) PopulateForm();
        }

        protected void PopulateForm()
        {
            //PopulateImagesDdl(ddlComputerImage);

            ddlComputerImage.DataSource =
                Call.ImageApi.Get(int.MaxValue, "")
                    .Where(
                        x => x.Environment == "linux" || x.Environment == "winpe" || string.IsNullOrEmpty(x.Environment))
                    .Select(i => new {i.Id, i.Name})
                    .OrderBy(x => x.Name)
                    .ToList();
            ddlComputerImage.DataValueField = "Id";
            ddlComputerImage.DataTextField = "Name";
            ddlComputerImage.DataBind();
            ddlComputerImage.Items.Insert(0, new ListItem("Select Image", "-1"));
        }
    }
}