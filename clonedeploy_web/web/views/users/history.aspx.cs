using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Global;
using Models;

namespace views.users
{
    public partial class UserHistory : BasePages.Users
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) PopulateHistory();
        }

        protected void ddlLimit_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateHistory();
        }

        protected void PopulateHistory()
        {
         

            if (!IsPostBack)
                ddlLimit.SelectedValue = "10";
            var history = new History
            {
                Type = "User",
                TypeId = CloneDeployUser.Id.ToString(),
                Limit = ddlLimit.Text,
                EventUser = CloneDeployUser.Name
            };
            gvHistory.DataSource = history.ReadUser();
            gvHistory.DataBind();
        }
    }
}