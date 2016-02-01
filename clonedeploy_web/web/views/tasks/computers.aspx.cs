using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Helpers;
using Image = BLL.Image;

namespace views.tasks
{
    public partial class TaskUnicast : BasePages.Tasks
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            if (Settings.DefaultComputerView == "all")
                PopulateGrid();
        }

        protected void btnDeploy_Click(object sender, EventArgs e)
        {
            var control = sender as Control;
            if (control != null)
            {
                var gvRow = (GridViewRow) control.Parent.Parent;
                var dataKey = gvComputers.DataKeys[gvRow.RowIndex];
                if (dataKey != null)
                {
                    var computer = BLL.Computer.GetComputer(Convert.ToInt32(dataKey.Value));
                    Session["computerID"] = computer.Id;
                    Session["direction"] = "push";
                    lblTitle.Text = "Deploy The Selected Computer?";
                    gvConfirm.DataSource = new List<Models.Computer> { computer };
                }
            }
            gvConfirm.DataBind();
            ClientScript.RegisterStartupScript(GetType(), "modalscript",
                "$(function() {  var menuTop = document.getElementById('confirmbox'),body = document.body;classie.toggle(menuTop, 'confirm-box-outer-open'); });",
                true);
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            var control = sender as Control;
            if (control != null)
            {
                var gvRow = (GridViewRow) control.Parent.Parent;
                var dataKey = gvComputers.DataKeys[gvRow.RowIndex];
                if (dataKey != null)
                {
                    var computer = BLL.Computer.GetComputer(Convert.ToInt32(dataKey.Value));
                    Session["computerID"] = computer.Id;
                    Session["direction"] = "pull";
                    lblTitle.Text = "Upload The Selected Computer?";
                    gvConfirm.DataSource = new List<Models.Computer> { computer };
                }
            }
            gvConfirm.DataBind();
            ClientScript.RegisterStartupScript(GetType(), "modalscript",
                "$(function() {  var menuTop = document.getElementById('confirmbox'),body = document.body;classie.toggle(menuTop, 'confirm-box-outer-open'); });;",
                true);
        }

        protected void gridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            PopulateGrid();

            var dataTable = gvComputers.DataSource as DataTable;

            if (dataTable == null) return;
            var dataView = new DataView(dataTable) {Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression)};
            gvComputers.DataSource = dataView;
            gvComputers.DataBind();
        }

        protected void OkButton_Click(object sender, EventArgs e)
        {
            var computer = BLL.Computer.GetComputer(Convert.ToInt32(Session["computerID"]));


            var direction = (string) (Session["direction"]);

            if (direction == "push")
            {
                var image = BLL.Image.GetImage(computer.ImageId);
                Session["imageID"] = image.Id;        
                EndUserMessage = new BLL.Workflows.Unicast(computer,direction,CloneDeployCurrentUser.Id).Start();               
             
            }
            else
            {
                EndUserMessage = new BLL.Workflows.Unicast(computer,direction,CloneDeployCurrentUser.Id).Start();
            }
            Session.Remove("computerID");
            Session.Remove("direction");
        }

        protected void OkButtonChecksum_Click(object sender, EventArgs e)
        {
            var imageId = (string) (Session["imageID"]);
            Response.Redirect("~/views/images/specs.aspx?imageid=" + imageId, false);
            Session.Remove("imageID");
        }

        protected void PopulateGrid()
        {
            var limit = 0;
            limit = ddlLimit.Text == "All" ? Int32.MaxValue : Convert.ToInt32(ddlLimit.Text);
            var listOfComputers = BLL.Computer.SearchComputersForUser(CloneDeployCurrentUser.Id,limit, txtSearch.Text);
            gvComputers.DataSource = listOfComputers.GroupBy(c => c.Id).Select(g => g.First()).ToList();
            gvComputers.DataBind();

            lblTotal.Text = gvComputers.Rows.Count + " Result(s) / " + BLL.Computer.ComputerCountUser(CloneDeployCurrentUser.Id) + " Total Computer(s)";
        }

        protected void search_Changed(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        protected void ddlLimit_OnSelectedIndexChanged(object sender, EventArgs e)
        {
           PopulateGrid();
        }
    }
}