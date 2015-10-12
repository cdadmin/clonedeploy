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
        public static string EndUserMessage
        {
            get { return (string)HttpContext.Current.Session["Message"]; }
            set { HttpContext.Current.Session["Message"] = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            if (string.IsNullOrEmpty(EndUserMessage)) return;
            const string msgType = "showSuccessToast";
            var page = HttpContext.Current.CurrentHandler as Page;

            if (page != null)
                page.ClientScript.RegisterStartupScript(GetType(), "msgBox",
                    "$(function() { $().toastmessage('" + msgType + "', " + "\"" + EndUserMessage + "\"); });", true);
            HttpContext.Current.Session.Remove("Message");
        }

        protected void PopulateImagesDdl(DropDownList ddlImages)
        {
            ddlImages.DataSource = BLL.Image.SearchImages("").Select(i => new { i.Id, i.Name });
            ddlImages.DataValueField = "Id";
            ddlImages.DataTextField = "Name";
            ddlImages.DataBind();
            ddlImages.Items.Insert(0, new ListItem("Select Image", "-1"));
        }

        protected void PopulateImageProfilesDdl(DropDownList ddlImageProfile, int value)
        {
            ddlImageProfile.DataSource = BLL.LinuxProfile.SearchProfiles(value).Select(i => new { i.Id, i.Name });
            ddlImageProfile.DataValueField = "Id";
            ddlImageProfile.DataTextField = "Name";
            ddlImageProfile.DataBind();
            ddlImageProfile.Items.Insert(0, new ListItem("Select Profile", "-1"));
        }

        protected void PopulateDistributionPointsDdl(DropDownList ddlDp)
        {
            ddlDp.DataSource =
                BLL.DistributionPoint.SearchDistributionPoints("").Select(d => new {d.Id, d.DisplayName});
            ddlDp.DataValueField = "Id";
            ddlDp.DataTextField = "DisplayName";
            ddlDp.DataBind();
        }

        protected void PopulateSitesDdl(DropDownList ddl)
        {
            ddl.DataSource = BLL.Site.SearchSites().Select(i => new { i.Id, i.Name });
            ddl.DataValueField = "Id";
            ddl.DataTextField = "Name";
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem("Select Site", "-1"));
        }

        protected void PopulateBuildingsDdl(DropDownList ddl)
        {
            ddl.DataSource = BLL.Building.SearchBuildings().Select(i => new { i.Id, i.Name });
            ddl.DataValueField = "Id";
            ddl.DataTextField = "Name";
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem("Select Building", "-1"));
        }

        protected void PopulateRoomsDdl(DropDownList ddl)
        {
            ddl.DataSource = BLL.Room.SearchRooms().Select(i => new { i.Id, i.Name });
            ddl.DataValueField = "Id";
            ddl.DataTextField = "Name";
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem("Select Room", "-1"));
        }

        protected void PopulateBootTemplatesDdl(DropDownList ddl)
        {
            ddl.DataSource = BLL.BootTemplate.SearchBootTemplates().Select(i => new { i.Id, i.Name });
            ddl.DataValueField = "Id";
            ddl.DataTextField = "Name";
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem("No Template", "-1"));
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

        public void Export(string fileName, string contents)
        {
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.AppendHeader("Content-Disposition",
                "attachment; filename=" + fileName);
            HttpContext.Current.Response.Write(contents);
            HttpContext.Current.Response.End();
        }
    }
}