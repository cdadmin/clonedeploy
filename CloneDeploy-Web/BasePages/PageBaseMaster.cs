using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using CloneDeploy_ApiCalls;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;

namespace CloneDeploy_Web.BasePages
{
    public class PageBaseMaster : Page
    {
        public APICall Call;
        public CloneDeployUserEntity CloneDeployCurrentUser;
        public List<string> CurrentUserRights;

        public static string EndUserMessage
        {
            get { return (string) HttpContext.Current.Session["Message"]; }
            set { HttpContext.Current.Session["Message"] = value; }
        }

        public void ChkAll(GridView gridview)
        {
            var hcb = (CheckBox) gridview.HeaderRow.FindControl("chkSelectAll");

            foreach (GridViewRow row in gridview.Rows)
            {
                var cb = (CheckBox) row.FindControl("chkSelector");
                if (cb != null)
                    cb.Checked = hcb.Checked;
            }
        }

        public void Export(string fileName, string contents)
        {
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.AppendHeader("Content-Disposition",
                "attachment; filename=" + fileName);
            HttpContext.Current.Response.Write(contents);
            HttpContext.Current.Response.End();
        }

        public string GetSortDirection(string sortExpression)
        {
            if (ViewState[sortExpression] == null)
                ViewState[sortExpression] = "Desc";
            else
                ViewState[sortExpression] = ViewState[sortExpression].ToString() == "Desc" ? "Asc" : "Desc";

            return ViewState[sortExpression].ToString();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Call = new APICall();
            var currentUser = Session["CloneDeployUser"];

            if (currentUser == null)
            {
                HttpContext.Current.Session.Abandon();
                FormsAuthentication.SignOut();
                Response.Redirect("~/?session=expired", true);
            }

            CloneDeployCurrentUser = (CloneDeployUserEntity) currentUser;
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

        protected void PopulateBootTemplatesDdl(DropDownList ddl)
        {
            ddl.DataSource = Call.BootTemplateApi.Get(int.MaxValue, "").Select(i => new {i.Id, i.Name});
            ddl.DataValueField = "Id";
            ddl.DataTextField = "Name";
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem("No Template", "-1"));
        }

        protected void PopulateBuildingsDdl(DropDownList ddl)
        {
            ddl.DataSource = Call.BuildingApi.Get(int.MaxValue, "").Select(i => new {i.Id, i.Name});
            ddl.DataValueField = "Id";
            ddl.DataTextField = "Name";
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem("Select Building", "-1"));
        }

        protected void PopulateClusterGroupsDdl(DropDownList ddlClusterGroup)
        {
            ddlClusterGroup.DataSource =
                Call.ClusterGroupApi.Get(int.MaxValue, "").Select(d => new {d.Id, d.Name});
            ddlClusterGroup.DataValueField = "Id";
            ddlClusterGroup.DataTextField = "Name";
            ddlClusterGroup.DataBind();
            ddlClusterGroup.Items.Insert(0, new ListItem("Default", "-1"));
        }

        protected void PopulateGroupsDdl(DropDownList ddlGroups)
        {
            ddlGroups.DataSource = Call.GroupApi.Get(int.MaxValue, "").Select(i => new {i.Id, i.Name});
            ddlGroups.DataValueField = "Id";
            ddlGroups.DataTextField = "Name";
            ddlGroups.DataBind();
            ddlGroups.Items.Insert(0, new ListItem("Select Group", "-1"));
        }

        protected void PopulateImageProfilesDdl(DropDownList ddlImageProfile, int value)
        {
            ddlImageProfile.DataSource = Call.ImageApi.GetImageProfiles(value).Select(i => new {i.Id, i.Name});
            ddlImageProfile.DataValueField = "Id";
            ddlImageProfile.DataTextField = "Name";
            ddlImageProfile.DataBind();
            ddlImageProfile.Items.Insert(0, new ListItem("Select Profile", "-1"));
        }

        protected void PopulateImagesDdl(DropDownList ddlImages)
        {
            ddlImages.DataSource =
                Call.ImageApi.Get(int.MaxValue, "").Select(i => new {i.Id, i.Name}).OrderBy(x => x.Name).ToList();
            ddlImages.DataValueField = "Id";
            ddlImages.DataTextField = "Name";
            ddlImages.DataBind();
            ddlImages.Items.Insert(0, new ListItem("Select Image", "-1"));
        }

        protected void PopulateImagesDdlForComputer(int computerId,DropDownList ddlImages)
        {
            var images = Call.ImageApi.Get(int.MaxValue, "");
            var filterDto = new FilterComputerClassificationDTO();
            filterDto.ComputerId = computerId;
            filterDto.ListImages = images;
            var filteredClassifications = Call.ComputerImageClassificationApi.FilterClassifications(filterDto);
            ddlImages.DataSource =
                filteredClassifications.Select(i => new { i.Id, i.Name }).OrderBy(x => x.Name).ToList();

            ddlImages.DataValueField = "Id";
            ddlImages.DataTextField = "Name";
            ddlImages.DataBind();
            ddlImages.Items.Insert(0, new ListItem("Select Image", "-1"));
        }

        protected void PopulateImagesDdlForGroup(int groupId, DropDownList ddlImages)
        {
            var images = Call.ImageApi.Get(int.MaxValue, "");
            var filterDto = new FilterGroupClassificationDTO();
            filterDto.GroupId = groupId;
            filterDto.ListImages = images;
            var filteredClassifications = Call.GroupImageClassificationApi.FilterClassifications(filterDto);
            ddlImages.DataSource =
                filteredClassifications.Select(i => new { i.Id, i.Name }).OrderBy(x => x.Name).ToList();

            ddlImages.DataValueField = "Id";
            ddlImages.DataTextField = "Name";
            ddlImages.DataBind();
            ddlImages.Items.Insert(0, new ListItem("Select Image", "-1"));
        }

        protected void PopulateRoomsDdl(DropDownList ddl)
        {
            ddl.DataSource = Call.RoomApi.Get(int.MaxValue, "").Select(i => new {i.Id, i.Name});
            ddl.DataValueField = "Id";
            ddl.DataTextField = "Name";
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem("Select Room", "-1"));
        }

        protected void PopulateSitesDdl(DropDownList ddl)
        {
            ddl.DataSource = Call.SiteApi.Get(int.MaxValue, "").Select(i => new {i.Id, i.Name});
            ddl.DataValueField = "Id";
            ddl.DataTextField = "Name";
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem("Select Site", "-1"));
        }

        protected void PopulateAltServerIps(DropDownList ddl)
        {
            ddl.DataSource = Call.AlternateServerIpApi.Get().Select(i => new { i.Id, i.Ip });
            ddl.DataValueField = "Id";
            ddl.DataTextField = "Ip";
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem("Select Alternate Ip", "-1"));
        }

        protected void PopulateImageClassifications(DropDownList ddl)
        {
            ddl.DataSource = Call.ImageClassificationApi.Get().Select(i => new { i.Id, i.Name });
            ddl.DataValueField = "Id";
            ddl.DataTextField = "Name";
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem("Select Classification", "-1"));
        }

        public void RequiresAuthorization(string requiredRight)
        {
            if (!Call.AuthorizationApi.IsAuthorized(requiredRight))
                Response.Redirect("~/views/dashboard/dash.aspx?access=denied", true);
        }

        public void RequiresAuthorizationOrManagedComputer(string requiredRight, int computerId)
        {
            if (!Call.AuthorizationApi.ComputerManagement(requiredRight, computerId))
                Response.Redirect("~/views/dashboard/dash.aspx?access=denied");
        }

        public void RequiresAuthorizationOrManagedGroup(string requiredRight, int groupId)
        {
            if (!Call.AuthorizationApi.GroupManagement(requiredRight, groupId))
                Response.Redirect("~/views/dashboard/dash.aspx?access=denied");
        }

        public void RequiresAuthorizationOrManagedImage(string requiredRight, int imageId)
        {
            if (!Call.AuthorizationApi.ImageManagement(requiredRight, imageId))
                Response.Redirect("~/views/dashboard/dash.aspx?access=denied");
        }

        public static string PlaceHolderReplace(string parameter)
        {
            if (string.IsNullOrEmpty(parameter)) return parameter;
            var start = parameter.IndexOf("[", StringComparison.Ordinal);
            var to = parameter.IndexOf("]", start + "[".Length, StringComparison.Ordinal);
            if (start < 0 || to < 0) return parameter;
            var s = parameter.Substring(
                start + "[".Length,
                to - start - "[".Length);
            if (s == "server-ip")
            {
                return parameter.Replace("[server-ip]", GetSetting(SettingStrings.ServerIp));
            }
            if (s == "tftp-server-ip")
            {
                return parameter.Replace("[tftp-server-ip]", GetSetting(SettingStrings.TftpServerIp));
            }
            return s;
        }

        public static List<string> GetFeLogs()
        {
            var logPath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "private" +
                          Path.DirectorySeparatorChar + "logs" + Path.DirectorySeparatorChar;

            var logFiles = Directory.GetFiles(logPath, "*.*");
            var result = new List<string>();
            for (var x = 0; x < logFiles.Length; x++)
                result.Add(Path.GetFileName(logFiles[x]));

            return result;
        }

        public static List<string> GetLogContents(string name, int limit)
        {
            var logPath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "private" +
                          Path.DirectorySeparatorChar + "logs" + Path.DirectorySeparatorChar + name;
            return File.ReadLines(logPath).Reverse().Take(limit).Reverse().ToList();
        }

        public static string GetSetting(string settingName)
        {
            var setting = new APICall().SettingApi.GetSetting(settingName);
            return setting != null ? setting.Value : string.Empty;
        }
    }
}