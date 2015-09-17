using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Global;
using Models;

namespace views.groups
{
    public partial class GroupHistory : Page
    {
        protected void ddlLimit_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateHistory();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var group = new Group {Id = Convert.ToInt32(Request.QueryString["groupid"])};
            group.Read();
            var subTitle = Master.Master.FindControl("SubNav").FindControl("labelSubTitle") as Label;
            if (subTitle != null) subTitle.Text = group.Name + " | History";
            if (!IsPostBack) PopulateHistory();
        }

        protected void PopulateHistory()
        {
            var group = new Group {Id = Convert.ToInt32(Request.QueryString["groupid"])};
            group.Read();

            if (!IsPostBack)
                ddlLimit.SelectedValue = "10";
            var history = new History
            {
                Type = "Group",
                TypeId = group.Id.ToString(),
                Limit = ddlLimit.Text
            };
            gvHistory.DataSource = history.Read();
            gvHistory.DataBind();
            Master.Master.Msgbox(Utility.Message);
        }
    }
}