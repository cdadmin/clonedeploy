using System;
using Models;

namespace BasePages
{
    public class Groups : PageBaseMaster
    {
        public Group Group { get; set; }
        public BLL.Group BllGroup { get; set; }

        public Groups() 
        {
            BllGroup = new BLL.Group();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Group = !string.IsNullOrEmpty(Request["groupid"]) ? BllGroup.GetGroup(Convert.ToInt32(Request.QueryString["groupid"])) : null;
        }
    }
}