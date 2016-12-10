using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Security;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

namespace views.dashboard
{
    public partial class Dashboard : PageBaseMaster
    {
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

        protected void LogOut_OnClick(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Response.Redirect("~/", true);
        }



        protected void PopulateStats()
        {
            //FixMe: get all the numbers in own function, don't slowly create full lists of unused stuff 

            List<ComputerEntity> computersList = Call.ComputerApi.GetAll(Int32.MaxValue, "");
            lblTotalComputers.Text = computersList.Count + " Total Computer(s)";

            List<ImageEntity> imagesList =
                Call.ImageApi.GetAll(Int32.MaxValue, "").OrderBy(x => x.Name).ToList();
            lblTotalImages.Text = imagesList.Count + " Total Image(s)";

            List<GroupEntity> groupsList = Call.GroupApi.GetAll(Int32.MaxValue, "");
            lblTotalGroups.Text = groupsList.Count + " Total Group(s)";

            var free = Call.FilesystemApi.GetDpFreeSpace();

            lblTotalDP.Text += "<br> Path: " + free.dPPath;

            string percentFreeCircleLightStart = @"<div class='clearfix'><table><tr>";
            string percentFreeCircleLight = @"<td><div>free</div><div class='c100 p" + free.freePercent +
                                            " small'><span>" + free.freePercent +
                                            "%</span><div class='slice'><div class='bar'></div><div class='fill'></div></div></div></td>";
            string percentUsedCircleLight = @"<td><div>used</div><div class='c100 p" + free.usedPercent +
                                            " small'><span>" + free.usedPercent +
                                            "%</span><div class='slice'><div class='bar'></div><div class='fill'></div></div></div></td>";
            string percentFreeCircleLightEnd = @"</tr></table></div>";

            lblDPfree.Text += percentFreeCircleLightStart;
            lblDPfree.Text += percentFreeCircleLight;
            lblDPfree.Text += percentUsedCircleLight;
            lblDPfree.Text += percentFreeCircleLightEnd;

            lblDPfree.Text += String.Format(" Free Space:      {0,15:D}",
                Utility.SizeSuffix(Convert.ToInt64(free.freespace)));
            lblDPfree.Text += String.Format(" || Total:     {0,15:D}", Utility.SizeSuffix(Convert.ToInt64(free.total)));


        }


    }





}


