using System;
using System.Linq;
using System.Web;
using BLL;
using Helpers;
using Tasks;

namespace views.tasks
{
    public partial class TaskCustom : BasePages.Tasks
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var user = BLL.User.GetUser(HttpContext.Current.User.Identity.Name);

            if (Settings.OnDemand == "Disabled")
            {
                secure.Visible = false;
                secureMsg.Text = "On Demand Mode Has Been Globally Disabled";
                secureMsg.Visible = true;
            }
                //FIX ME
                /*
            else if (user.OndAccess == "0")
            {
                secure.Visible = false;
                secureMsg.Text = "On Demand Mode Has Been Disabled For This Account";
                secureMsg.Visible = true;
            }*/
            else
            {
                secure.Visible = true;
                secureMsg.Visible = false;
            }
            if (IsPostBack) return;
            ddlImage.DataSource = BLL.Image.SearchImages("").Select(i => i.Name);
            ddlImage.DataBind();
            ddlImage.Items.Insert(0, "Select Image");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (ddlImage.Text != "Select Image")
            {
                var multicast = new Multicast
                {
                    ActiveMcTask = {Image = ddlImage.Text},
                    IsCustom = true
                };
                multicast.StartMulticastSender();

            }
            else
                EndUserMessage = "Select An Image";
        }
    }
}