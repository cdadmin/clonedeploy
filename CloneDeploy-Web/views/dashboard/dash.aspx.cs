using System;
using System.Linq;
using System.Web;
using System.Web.Security;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.dashboard
{
    public partial class Dashboard : PageBaseMaster
    {
        public int freePercent;
        public int usedPercent;

        protected void LogOut_OnClick(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Response.Redirect("~/", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Call.CdVersionApi.IsFirstRunCompleted())
                Response.Redirect("~/views/login/firstrun.aspx");

            if (Request.QueryString["access"] == "denied")
            {
                lblDenied.Text = "You Are Not Authorized For That Action<br><br>";
            }
            LogOut.Text = HttpContext.Current.User.Identity.Name;
            PopulateStats();
        }

        protected void PopulateStats()
        {
            //FixMe: get all the numbers in own function, don't slowly create full lists of unused stuff 

            var computersList = Call.ComputerApi.GridViewSearch(int.MaxValue, "");
            lblTotalComputers.Text = computersList.Count + " Total Computer(s)";

            var imagesList =
                Call.ImageApi.Get(int.MaxValue, "").OrderBy(x => x.Name).ToList();
            lblTotalImages.Text = imagesList.Count + " Total Image(s)";

            var groupsList = Call.GroupApi.Get(int.MaxValue, "");
            lblTotalGroups.Text = groupsList.Count + " Total Group(s)";

            var free = Call.FilesystemApi.GetDpFreeSpace();

            lblDpPath.Text = free.dPPath;
            freePercent = free.freePercent;
            usedPercent = free.usedPercent;

            lblDPfree.Text = string.Format("<b>Free Space:</b>{0,15:D}", SizeSuffix(Convert.ToInt64(free.freespace)));
            lblDpTotal.Text = string.Format("<b>Total Space:</b>{0,15:D}", SizeSuffix(Convert.ToInt64(free.total)));

            gvLogins.DataSource = Call.CloneDeployUserApi.GetUserLoginsDashboard();
            gvLogins.DataBind();

            gvTasks.DataSource = Call.CloneDeployUserApi.GetUserTaskAuditLogs(CloneDeployCurrentUser.Id, 25);
            gvTasks.DataBind();
        }

        private string SizeSuffix(long value)
        {
            string[] SizeSuffixes = {"bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"};

            if (value < 0)
            {
                return "-" + SizeSuffix(-value);
            }
            if (value == 0)
            {
                return "0.0 bytes";
            }

            var mag = (int) Math.Log(value, 1024);
            var adjustedSize = (decimal) value/(1L << (mag*10));

            return string.Format("{0:n1} {1}", adjustedSize, SizeSuffixes[mag]);
        }
    }
}