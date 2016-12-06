using System;
using System.Web.UI.WebControls;

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
            //PopulateImagesDdl(ddlComputerImage);

            ddlComputerImage.DataSource = BLL.Image.SearchImagesForUser(CloneDeployCurrentUser.Id).Where(x => x.Environment == "linux" || x.Environment == "winpe" || string.IsNullOrEmpty(x.Environment)).Select(i => new { i.Id, i.Name }).OrderBy(x => x.Name).ToList();
            ddlComputerImage.DataValueField = "Id";
            ddlComputerImage.DataTextField = "Name";
            ddlComputerImage.DataBind();
            ddlComputerImage.Items.Insert(0, new ListItem("Select Image", "-1"));   
        }

        protected void ddlComputerImage_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateImageProfilesDdl(ddlImageProfile, Convert.ToInt32(ddlComputerImage.SelectedValue));
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            RequiresAuthorization(Authorizations.ImageMulticastTask);
            if (ddlComputerImage.Text == "Select Image") return;
            var imageProfile = BLL.ImageProfile.ReadProfile(Convert.ToInt32(ddlImageProfile.SelectedValue));
            EndUserMessage = new BLL.Workflows.Multicast(imageProfile, txtClientCount.Text,CloneDeployCurrentUser.Id).Create();

        }
    }
}