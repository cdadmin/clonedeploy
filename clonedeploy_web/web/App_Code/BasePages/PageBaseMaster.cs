using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace BasePages
{
    public class PageBaseMaster : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            new Message().Show();
        }

        protected void PopulateImagesDdl(DropDownList ddlImages, int value)
        {
            ddlHostImage.DataSource = new BLL.Image().SearchImages("").Select(i => new { i.Id, i.Name });
            ddlHostImage.DataValueField = "Id";
            ddlHostImage.DataTextField = "Name";
            ddlHostImage.DataBind();
            ddlHostImage.Items.Insert(0, new ListItem("Select Image", "0"));
        }

        protected void PopulateImageProfilesDdl(DropDownList ddlImageProfile, int value)
        {
            ddlImageProfile.DataSource = new BLL.LinuxProfile().SearchProfiles(value).Select(i => new { i.Id, i.Name });
            ddlImageProfile.DataValueField = "Id";
            ddlImageProfile.DataTextField = "Name";
            ddlImageProfile.DataBind();
        }
    }
}