using System;
using System.Linq;
using System.Web;
using BLL;
using BLL.Workflows;
using Helpers;
using Tasks;

namespace views.tasks
{
    public partial class TaskCustom : BasePages.Tasks
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Settings.OnDemand != "Enabled")
                Response.Redirect("~/views/dashboard/dash.aspx?access=denied");
            RequiresAuthorization(Authorizations.AllowOnd);
            if (!IsPostBack) PopulateForm();
        }

        protected void PopulateForm()
        {
            PopulateImagesDdl(ddlComputerImage);
        }

        protected void ddlComputerImage_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateImageProfilesDdl(ddlImageProfile, Convert.ToInt32(ddlComputerImage.SelectedValue));
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (ddlComputerImage.Text == "Select Image") return;
            var imageProfile = BLL.ImageProfile.ReadProfile(Convert.ToInt32(ddlImageProfile.SelectedValue));
            EndUserMessage = new BLL.Workflows.Multicast(imageProfile, txtClientCount.Text,CloneDeployCurrentUser.Id).Create();

        }
    }
}