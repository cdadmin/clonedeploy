/*  
    CrucibleWDS A Windows Deployment Solution
    Copyright (C) 2011  Jon Dolny

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/.
 */

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

        public string GetSortDirection(string sortExpression)
        {
            if (ViewState[sortExpression] == null)
                ViewState[sortExpression] = "Desc";
            else
                ViewState[sortExpression] = ViewState[sortExpression].ToString() == "Desc" ? "Asc" : "Desc";

            return ViewState[sortExpression].ToString();
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
                    var unicast = new Unicast {Host = host, Direction = direction};
                    unicast.Create();
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
                var unicast = new Unicast {Host = host, Direction = direction};
                unicast.Create();
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