using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Global;
using Models;

namespace views.users
{
    public partial class UserHistory : Page
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
                TypeId = Master.User.Id.ToString(),
                Limit = ddlLimit.Text,
                EventUser = Master.User.Name
            };
            gvHistory.DataSource = history.ReadUser();
            gvHistory.DataBind();
        }
    }
}