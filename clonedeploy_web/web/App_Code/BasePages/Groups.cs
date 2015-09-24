using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasePages
{
    public class Groups : PageBaseMaster
    {
        public Models.Group Group { get; set; }
        public BLL.Group BllGroup { get; set; }

        public Groups() 
        {
            BllGroup = new BLL.Group();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Group = !string.IsNullOrEmpty(Request["hostid"]) ? BllGroup.GetGroup(Convert.ToInt32(Request.QueryString["groupid"])) : null;
        }
    }
}