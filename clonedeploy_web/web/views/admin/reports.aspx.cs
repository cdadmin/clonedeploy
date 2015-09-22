using System;
using System.Web.UI;
using Security;

namespace views.admin
{
    public partial class Reports : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /*
            if (!new Authorize().IsInMembership("Administrator"))
                Response.Redirect("~/views/dashboard/dash.aspx?access=denied");

            var reports = new Global.Reports();

            gvLastFiveUsers.DataSource = reports.LastUsers();
            gvLastFiveUsers.DataBind();

            gvLastFiveUnicasts.DataSource = reports.LastUnicasts();
            gvLastFiveUnicasts.DataBind();

            gvLastFiveMulticasts.DataSource = reports.LastMulticasts();
            gvLastFiveMulticasts.DataBind();

            gvTopFiveUnicasts.DataSource = reports.TopFiveUnicast();
            gvTopFiveUnicasts.DataBind();

            gvTopFiveMulticasts.DataSource = reports.TopFiveMulticast();
            gvTopFiveMulticasts.DataBind();

            gvUserStats.DataSource = reports.UserStats();
            gvUserStats.DataBind();
             * */
        }
    }
}