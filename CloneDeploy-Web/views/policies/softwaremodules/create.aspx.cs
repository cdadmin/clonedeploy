using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CloneDeploy_Web.views.policies.softwaremodules
{
    public partial class create : System.Web.UI.Page
    {
        protected string token { get; set; }
        protected string baseurl { get; set; }
        protected string softwareGuid { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                token = Request.Cookies["cdtoken"].Value;
                baseurl = Request.Cookies["cdBaseUrl"].Value;
                softwareGuid = Guid.NewGuid().ToString();
            }

        }

        protected void btnAdd_OnClick(object sender, EventArgs e)
        {
            
        }

        protected void ddlType_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}