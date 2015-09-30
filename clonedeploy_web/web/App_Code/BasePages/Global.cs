using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasePages
{
    public class Global : PageBaseMaster
    {
        public Models.SysprepTag SysprepTag { get; set; }
        public BLL.SysprepTag BllSysprepTag { get; set; }
        public BLL.Site BllSite { get; set; }
        public BLL.Room BllRoom { get; set; }
        public BLL.Building BllBuilding { get; set; }

        public Global() 
        {
            BllSysprepTag = new BLL.SysprepTag();
            BllSite =new BLL.Site();
            BllRoom = new BLL.Room();
            BllBuilding = new BLL.Building();

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            SysprepTag = !string.IsNullOrEmpty(Request["syspreptagid"]) ? BllSysprepTag.GetSysprepTag(Convert.ToInt32(Request.QueryString["syspreptagid"])) : null;
           
        }
    }
}