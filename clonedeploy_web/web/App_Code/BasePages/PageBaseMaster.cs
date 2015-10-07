using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Helpers;
using Image = BLL.Image;

namespace BasePages
{
    public class PageBaseMaster : Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            if (string.IsNullOrEmpty(Message.Text)) return;
            const string msgType = "showSuccessToast";
            var page = HttpContext.Current.CurrentHandler as Page;

            if (page != null)
                page.ClientScript.RegisterStartupScript(GetType(), "msgBox",
                    "$(function() { $().toastmessage('" + msgType + "', " + "\"" + Message.Text + "\"); });", true);
            HttpContext.Current.Session.Remove("Message");
        }

        protected void PopulateImagesDdl(DropDownList ddlImages)
        {
            ddlImages.DataSource = new Image().SearchImages("").Select(i => new { i.Id, i.Name });
            ddlImages.DataValueField = "Id";
            ddlImages.DataTextField = "Name";
            ddlImages.DataBind();
            ddlImages.Items.Insert(0, new ListItem("Select Image", "0"));
        }

        protected void PopulateImageProfilesDdl(DropDownList ddlImageProfile, int value)
        {
            ddlImageProfile.DataSource = new LinuxProfile().SearchProfiles(value).Select(i => new { i.Id, i.Name });
            ddlImageProfile.DataValueField = "Id";
            ddlImageProfile.DataTextField = "Name";
            ddlImageProfile.DataBind();
        }

        protected void PopulateDistributionPointsDdl(DropDownList ddlDp)
        {
            ddlDp.DataSource =
                new DistributionPoint().SearchDistributionPoints("").Select(d => new {d.Id, d.DisplayName});
            ddlDp.DataValueField = "Id";
            ddlDp.DataTextField = "DisplayName";
            ddlDp.DataBind();
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