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

        protected void PopulateImagesDdl(DropDownList ddlImages)
        {
            ddlImages.DataSource = new BLL.Image().SearchImages("").Select(i => new { i.Id, i.Name });
            ddlImages.DataValueField = "Id";
            ddlImages.DataTextField = "Name";
            ddlImages.DataBind();
            ddlImages.Items.Insert(0, new ListItem("Select Image", "0"));
        }

        protected void PopulateImageProfilesDdl(DropDownList ddlImageProfile, int value)
        {
            ddlImageProfile.DataSource = new BLL.LinuxProfile().SearchProfiles(value).Select(i => new { i.Id, i.Name });
            ddlImageProfile.DataValueField = "Id";
            ddlImageProfile.DataTextField = "Name";
            ddlImageProfile.DataBind();
        }


        public void ChkAll(GridView gridview)
        {
            var hcb = (CheckBox)gridview.HeaderRow.FindControl("chkSelectAll");

            foreach (GridViewRow row in gridview.Rows)
            {
                var cb = (CheckBox)row.FindControl("chkSelector");
                if (cb != null)
                    cb.Checked = hcb.Checked;
            }
        }

        public string GetSortDirection(string sortExpression)
        {
            if (ViewState[sortExpression] == null)
                ViewState[sortExpression] = "Desc";
            else
                ViewState[sortExpression] = ViewState[sortExpression].ToString() == "Desc" ? "Asc" : "Desc";

            return ViewState[sortExpression].ToString();
        }

        
    }
}