using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using BLL;
using Models;

namespace BasePages
{
    public class Computers : Global
    {
        public Models.Computer Computer { get; set; }
        public BLL.Computer BllComputer { get; set; }

        public Computers() 
        {
            BllComputer = new BLL.Computer();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!string.IsNullOrEmpty(Request["hostid"]))
            {
                Computer = BllComputer.GetComputer(Convert.ToInt32(Request.QueryString["hostid"]));
            }
        }

        protected void Test(DropDownList ddlImageProfile, int value)
        {
            ddlImageProfile.DataSource = new BLL.LinuxProfile().SearchProfiles(value).Select(i => new { i.Id, i.Name });
            ddlImageProfile.DataValueField = "Id";
            ddlImageProfile.DataTextField = "Name";
            ddlImageProfile.DataBind();
        }

        
    }
}