using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Security;
//for DllImport, move to another file later
#if __MonoCS__  
using Mono.Unix; // requires reference to  Mono.Posix.dll
#endif
using BasePages;
using CloneDeploy_Entities;

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
        // if running on Mono
        #if __MonoCS__     
        public static bool DriveFreeBytes(string folderName, out ulong freespace, out ulong total)
        {
            freespace = 0;
            total = 0;
            
            UnixDriveInfo[] drives = UnixDriveInfo.GetDrives();
            int idx = -1, count = -1;
            for (int i = 0; i < drives.Length; ++i)
            {
              if (folderName.StartsWith (drives[i].Name) && drives[i].Name.Length > count)
              {
                count = drives[i].Name.Length;
                idx = i;
              }
            }
            
            // Drive for path is: drives[idx].Name
            freespace = (ulong)drives[idx].AvailableFreeSpace;
            total = (ulong)drives[idx].TotalSize; 
            return true;
        }   
        #else
        
        // using GetDiskFreeSpaceEx because this handles mountpoints, quota and UNC 
        // there are reports that old CIFS doesn't support unc-share to a mountpoint, needs Windows 2008/SMB2        
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetDiskFreeSpaceEx(string lpDirectoryName,
        out ulong lpFreeBytesAvailable,
        out ulong lpTotalNumberOfBytes,
        out ulong lpTotalNumberOfFreeBytes);
    
        public static bool DriveFreeBytes(string folderName, out ulong freespace, out ulong total)
        {
            freespace = 0;
            total = 0;
            if (string.IsNullOrEmpty(folderName))
            {
                throw new ArgumentNullException("folderName");
            }
    
            if (!folderName.EndsWith("\\"))
            {
                folderName += '\\';
            }
    
            ulong free = 0, tot = 0, dummy2 = 0;
    
            if (GetDiskFreeSpaceEx(folderName, out free, out tot, out dummy2))
            {
                freespace = free;
                total = tot;
                return true;
            }
            else
            {
                return false;
            }
        }   
        #endif


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

            var primaryDp = Call.DistributionPointApi.GetPrimary();
            
            lblTotalDP.Text += "<br> Path: " + primaryDp.PhysicalPath;

            try
            {
                if (System.IO.Directory.Exists(primaryDp.PhysicalPath))
                {
                    ulong freespace = 0;
                    ulong total = 0;
                    bool success = DriveFreeBytes(primaryDp.PhysicalPath, out freespace, out total);

                    if (!success)
                    {
                    }
                    else
                    {
                        Int64 freePercent = 0;
                        Int64 usedPercent = 0;

                        if (total > 0 && freespace > 0)
                        {
                            freePercent = (Int64) (0.5f + ((100f*Convert.ToInt64(freespace))/Convert.ToInt64(total)));
                            usedPercent =
                                (Int64) (0.5f + ((100f*Convert.ToInt64(total - freespace))/Convert.ToInt64(total)));
                        }

                        string percentFreeCircleLightStart = @"<div class='clearfix'><table><tr>";
                        string percentFreeCircleLight = @"<td><div>free</div><div class='c100 p" + freePercent +
                                                        " small'><span>" + freePercent +
                                                        "%</span><div class='slice'><div class='bar'></div><div class='fill'></div></div></div></td>";
                        string percentUsedCircleLight = @"<td><div>used</div><div class='c100 p" + usedPercent +
                                                        " small'><span>" + usedPercent +
                                                        "%</span><div class='slice'><div class='bar'></div><div class='fill'></div></div></div></td>";
                        string percentFreeCircleLightEnd = @"</tr></table></div>";

                        lblDPfree.Text += percentFreeCircleLightStart;
                        lblDPfree.Text += percentFreeCircleLight;
                        lblDPfree.Text += percentUsedCircleLight;
                        lblDPfree.Text += percentFreeCircleLightEnd;

                        lblDPfree.Text += String.Format(" Free Space:      {0,15:D}",
                            SizeSuffix(Convert.ToInt64(freespace)));
                        lblDPfree.Text += String.Format(" || Total:     {0,15:D}", SizeSuffix(Convert.ToInt64(total)));

                    }
                }
            }
            catch
            {
                //ignore
            }
        }

        static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        static string SizeSuffix(Int64 value)
        {
            if (value < 0) { return "-" + SizeSuffix(-value); } 
            if (value == 0) { return "0.0 bytes"; }
        
            int mag = (int)Math.Log(value, 1024);
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));
        
            return string.Format("{0:n1} {1}", adjustedSize, SizeSuffixes[mag]);
        }


       
    }
}
