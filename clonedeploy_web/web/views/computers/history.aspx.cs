using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Global;
using Models;

namespace views.hosts
{
    public partial class HostHistory : Page
    {
        public Computer Host { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Host = new Computer { Id = Convert.ToInt16(Request["hostid"] )};
            Host.Read();
            var subTitle = Master.Master.FindControl("SubNav").FindControl("labelSubTitle") as Label;
            if (subTitle != null) subTitle.Text = Host.Name + " | History";
            if (!IsPostBack) PopulateHistory();
        }

        protected void ddlLimit_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateHistory();
        }

        protected void PopulateHistory()
        {
            if (!IsPostBack) ddlLimit.SelectedValue = "10";
            var history = new History {Type = "Host", TypeId = Host.Id.ToString(), Limit = ddlLimit.Text};
            gvHistory.DataSource = history.Read();
            gvHistory.DataBind();
            Master.Msgbox(Utility.Message);
        }
    }
}