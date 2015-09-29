using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Global;
using Helpers;
using Models;
using Tasks;
using Image = Models.Image;

namespace views.tasks
{
    public partial class TaskUnicast : BasePages.Tasks
    {
        BLL.Computer _bllComputer = new BLL.Computer();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            if (Settings.DefaultHostView == "all")
                PopulateGrid();
        }

        protected void btnDeploy_Click(object sender, EventArgs e)
        {
            var control = sender as Control;
            if (control != null)
            {
                var gvRow = (GridViewRow) control.Parent.Parent;
                var dataKey = gvHosts.DataKeys[gvRow.RowIndex];
                if (dataKey != null)
                {
                    var host = _bllComputer.GetComputer(Convert.ToInt32(dataKey.Value));
                    Session["hostID"] = host.Id;
                    Session["direction"] = "push";
                    lblTitle.Text = "Deploy The Selected Host?";
                    gvConfirm.DataSource = new List<Models.Computer> { host };
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
                var dataKey = gvHosts.DataKeys[gvRow.RowIndex];
                if (dataKey != null)
                {
                    var host = _bllComputer.GetComputer(Convert.ToInt32(dataKey.Value));
                    Session["hostID"] = host.Id;
                    Session["direction"] = "pull";
                    lblTitle.Text = "Upload The Selected Host?";
                    gvConfirm.DataSource = new List<Models.Computer> { host };
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

            var dataTable = gvHosts.DataSource as DataTable;

            if (dataTable == null) return;
            var dataView = new DataView(dataTable) {Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression)};
            gvHosts.DataSource = dataView;
            gvHosts.DataBind();
        }

        protected void OkButton_Click(object sender, EventArgs e)
        {
            var host = _bllComputer.GetComputer(Convert.ToInt32(Session["hostID"]));


            var direction = (string) (Session["direction"]);

            if (direction == "push")
            {
                var bllImage = new BLL.Image();
                var image = bllImage.GetImage(host.Image);
                Session["imageID"] = image.Id;

                if (bllImage.Check_Checksum(image))
                {
                    new BLL.Computer().StartUnicast(host,direction);
                   
                }
                else
                {
                    lblIncorrectChecksum.Text =
                        "This Image Has Not Been Confirmed And Cannot Be Deployed.  <br>Confirm It Now?";
                    ClientScript.RegisterStartupScript(GetType(), "modalscript",
                        "$(function() {  var menuTop = document.getElementById('incorrectChecksum'),body = document.body;classie.toggle(menuTop, 'confirm-box-outer-open'); });",
                        true);
                }
            }
            else
            {
                new BLL.Computer().StartUnicast(host, direction);
            }
            Session.Remove("hostID");
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

            gvHosts.DataSource = _bllComputer.SearchComputers(txtSearch.Text);

            gvHosts.DataBind();

            lblTotal.Text = gvHosts.Rows.Count + " Result(s) / " + _bllComputer.TotalCount() + " Total Host(s)";
        }

        protected void search_Changed(object sender, EventArgs e)
        {
            PopulateGrid();
        }
    }
}