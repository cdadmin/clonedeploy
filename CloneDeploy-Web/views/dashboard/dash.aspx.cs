using System;
using System.Linq;
using System.Web;
using System.Web.Security;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.dashboard
{
    public partial class Dashboard : PageBaseMaster
    {
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

            var computersList = Call.ComputerApi.Search(int.MaxValue, "");
            lblTotalComputers.Text = computersList.Count + " Total Computer(s)";

            var imagesList =
                Call.ImageApi.GetAll(int.MaxValue, "").OrderBy(x => x.Name).ToList();
            lblTotalImages.Text = imagesList.Count + " Total Image(s)";

            var groupsList = Call.GroupApi.GetAll(int.MaxValue, "");
            lblTotalGroups.Text = groupsList.Count + " Total Group(s)";

            var free = Call.FilesystemApi.GetDpFreeSpace();

            lblTotalDP.Text += "<br> Path: " + free.dPPath;

            var percentFreeCircleLightStart = @"<div class='clearfix'><table><tr>";
            var percentFreeCircleLight = @"<td><div>free</div><div class='c100 p" + free.freePercent +
                                         " small'><span>" + free.freePercent +
                                         "%</span><div class='slice'><div class='bar'></div><div class='fill'></div></div></div></td>";
            var percentUsedCircleLight = @"<td><div>used</div><div class='c100 p" + free.usedPercent +
                                         " small'><span>" + free.usedPercent +
                                         "%</span><div class='slice'><div class='bar'></div><div class='fill'></div></div></div></td>";
            var percentFreeCircleLightEnd = @"</tr></table></div>";

            lblDPfree.Text += percentFreeCircleLightStart;
            lblDPfree.Text += percentFreeCircleLight;
            lblDPfree.Text += percentUsedCircleLight;
            lblDPfree.Text += percentFreeCircleLightEnd;

            lblDPfree.Text += string.Format(" Free Space:      {0,15:D}",
                SizeSuffix(Convert.ToInt64(free.freespace)));
            lblDPfree.Text += string.Format(" || Total:     {0,15:D}", SizeSuffix(Convert.ToInt64(free.total)));
        }

        private string SizeSuffix(long value)
        {
            string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

            if (value < 0)
            {
                return "-" + SizeSuffix(-value);
            }
            if (value == 0)
            {
                return "0.0 bytes";
            }

            var mag = (int)Math.Log(value, 1024);
            var adjustedSize = (decimal)value / (1L << (mag * 10));

            return string.Format("{0:n1} {1}", adjustedSize, SizeSuffixes[mag]);
        }
    }
}