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
            var user = new WdsUser { Id = Request.QueryString["userid"] };
            user.Read();
            var subTitle = Master.Master.FindControl("SubNav").FindControl("labelSubTitle") as Label;
            if (subTitle != null) subTitle.Text = user.Name + " | Edit";
            if (!IsPostBack) PopulateHistory();
        }

        protected void ddlLimit_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateHistory();
        }

        protected void PopulateHistory()
        {
            var user = new WdsUser {Id = Request.QueryString["userid"]};
            user.Read();

            if (!IsPostBack)
                ddlLimit.SelectedValue = "10";
            var history = new History
            {
                Type = "User",
                TypeId = user.Id,
                Limit = ddlLimit.Text,
                EventUser = user.Name
            };
            gvHistory.DataSource = history.ReadUser();
            gvHistory.DataBind();
            Master.Master.Msgbox(Utility.Message);
        }
    }
}