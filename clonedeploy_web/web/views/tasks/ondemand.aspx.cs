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
            RequiresAuthorization(Authorizations.AllowOnd);   
        
            if (IsPostBack) return;
            PopulateForm();
        }

        protected void PopulateForm()
        {
            PopulateImagesDdl(ddlHostImage);
           
        }

        protected void ddlHostImage_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateImageProfilesDdl(ddlImageProfile, Convert.ToInt32(ddlHostImage.SelectedValue));
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (ddlHostImage.Text != "Select Image")
            {
                var imageProfile = BLL.ImageProfile.ReadProfile(Convert.ToInt32(ddlImageProfile.SelectedValue));
                var multicast = new BLL.Workflows.Multicast(null, true, imageProfile).StartMulticastSender();              
            }
            else
                EndUserMessage = "Select An Image";
        }
    }
}