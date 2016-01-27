using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BasePages
{
    public class PageBaseMaster : Page
    {
        public Models.WdsUser CloneDeployCurrentUser;
        public List<string> CurrentUserRights;

        public static string EndUserMessage
        {
            get { return (string)HttpContext.Current.Session["Message"]; }
            set { HttpContext.Current.Session["Message"] = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            object currentUser = Session["CloneDeployUser"];

            if (currentUser == null )
            {
                HttpContext.Current.Session.Abandon();
                FormsAuthentication.SignOut();
                Response.Redirect("~/views/login/login.aspx?session=expired", true);
            }

            CloneDeployCurrentUser = (Models.WdsUser)currentUser;


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
            ddlImages.DataSource = BLL.Image.SearchImagesForUser(CloneDeployCurrentUser.Id).Select(i => new { i.Id, i.Name });
            ddlImages.DataValueField = "Id";
            ddlImages.DataTextField = "Name";
            ddlImages.DataBind();
            ddlImages.Items.Insert(0, new ListItem("Select Image", "-1"));
        }

        protected void PopulateImageProfilesDdl(DropDownList ddlImageProfile, int value)
        {
            ddlImageProfile.DataSource = BLL.ImageProfile.SearchProfiles(value).Select(i => new { i.Id, i.Name });
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
                ViewState[sortExpression] = "Asc";
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

        public void RequiresAuthorization(string requiredRight)
        {
            if(!new BLL.Authorize(CloneDeployCurrentUser, requiredRight).IsAuthorized())
                Response.Redirect("~/views/dashboard/dash.aspx?access=denied",true);
        }

        public void RequiresAuthorizationOrManagedComputer(string requiredRight, int computerId)
        {
            if (!new BLL.Authorize(CloneDeployCurrentUser, requiredRight).ComputerManagement(computerId))
            Response.Redirect("~/views/dashboard/dash.aspx?access=denied");
        }

        public void RequiresAuthorizationOrManagedGroup(string requiredRight, int groupId)
        {
            if (!new BLL.Authorize(CloneDeployCurrentUser, requiredRight).GroupManagement(groupId))
                Response.Redirect("~/views/dashboard/dash.aspx?access=denied");
        }

        public void RequiresAuthorizationOrManagedImage(string requiredRight, int imageId)
        {
            if (!new BLL.Authorize(CloneDeployCurrentUser, requiredRight).ImageManagement(imageId))
                Response.Redirect("~/views/dashboard/dash.aspx?access=denied");
        }
    }
}