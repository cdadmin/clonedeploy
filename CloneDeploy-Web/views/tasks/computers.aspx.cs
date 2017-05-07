using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

namespace views.tasks
{
    public partial class TaskUnicast : Tasks
    {
        protected void btnDeploy_Click(object sender, EventArgs e)
        {
            SingleOkButton.Visible = true;
            var control = sender as Control;
            if (control != null)
            {
                var gvRow = (GridViewRow) control.Parent.Parent;
                var dataKey = gvComputers.DataKeys[gvRow.RowIndex];
                if (dataKey != null)
                {
                    var computer = Call.ComputerApi.Get(Convert.ToInt32(dataKey.Value));
                    Session["computerID"] = computer.Id;
                    Session["direction"] = "push";
                    lblTitle.Text = "Deploy The Selected Computer?";
                    gvConfirm.DataSource = new List<ComputerEntity> {computer};
                }
            }
            gvConfirm.DataBind();
            DisplayConfirm();
        }

        protected void btnListDeploy_Click(object sender, EventArgs e)
        {
            MultiOkButton.Visible = true;
            Session["action"] = "push";
            lblTitle.Text = "Deploy The Selected Computers?";
            DisplayConfirm();
        }

        protected void btnListPermanentDeploy_Click(object sender, EventArgs e)
        {
            MultiOkButton.Visible = true;
            Session["action"] = "permanent_push";
            lblTitle.Text = "Permanent Deploy The Selected Computers?";
            DisplayConfirm();
        }

        protected void btnListUpload_Click(object sender, EventArgs e)
        {
            MultiOkButton.Visible = true;
            Session["action"] = "pull";
            lblTitle.Text = "Upload The Selected Computers?";
            DisplayConfirm();
        }

        protected void btnPermanentDeploy_Click(object sender, EventArgs e)
        {
            SingleOkButton.Visible = true;
            var control = sender as Control;
            if (control != null)
            {
                var gvRow = (GridViewRow) control.Parent.Parent;
                var dataKey = gvComputers.DataKeys[gvRow.RowIndex];
                if (dataKey != null)
                {
                    var computer = Call.ComputerApi.Get(Convert.ToInt32(dataKey.Value));
                    Session["computerID"] = computer.Id;
                    Session["direction"] = "permanent_push";
                    lblTitle.Text = "Permanent Deploy The Selected Computer?";
                    gvConfirm.DataSource = new List<ComputerEntity> {computer};
                }
            }
            gvConfirm.DataBind();
            DisplayConfirm();
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            SingleOkButton.Visible = true;
            var control = sender as Control;
            if (control != null)
            {
                var gvRow = (GridViewRow) control.Parent.Parent;
                var dataKey = gvComputers.DataKeys[gvRow.RowIndex];
                if (dataKey != null)
                {
                    var computer = Call.ComputerApi.Get(Convert.ToInt32(dataKey.Value));
                    Session["computerID"] = computer.Id;
                    Session["direction"] = "pull";
                    lblTitle.Text = "Upload The Selected Computer?";
                    gvConfirm.DataSource = new List<ComputerEntity> {computer};
                }
            }
            gvConfirm.DataBind();
            DisplayConfirm();
        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            ChkAll(gvComputers);
        }

        protected void ddlLimit_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        private void DisplayConfirm()
        {
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

        protected void MultiOkButton_Click(object sender, EventArgs e)
        {
            var action = (string) Session["action"];
            Session.Remove("action");

            var selectedComputers = new List<int>();

            foreach (GridViewRow row in gvComputers.Rows)
            {
                var cb = (CheckBox) row.FindControl("chkSelector");
                if (cb == null || !cb.Checked) continue;
                var dataKey = gvComputers.DataKeys[row.RowIndex];
                if (dataKey == null) continue;
                selectedComputers.Add(Convert.ToInt32(dataKey.Value));
            }

            var sucessCount = 0;
            switch (action)
            {
                case "permanent_push":
                {
                    foreach (var computerId in selectedComputers)
                    {
                        RequiresAuthorizationOrManagedComputer(Authorizations.ImageDeployTask, computerId);
                        if (Call.ComputerApi.StartPermanentDeploy(computerId).Contains("Successfully"))
                            sucessCount++;
                        EndUserMessage = "Successfully Started " + sucessCount + " Tasks";
                    }
                }
                    break;
                case "push":
                {
                    foreach (var computerId in selectedComputers)
                    {
                        RequiresAuthorizationOrManagedComputer(Authorizations.ImageDeployTask, computerId);
                        if (Call.ComputerApi.StartDeploy(computerId).Contains("Successfully"))
                            sucessCount++;
                        EndUserMessage = "Successfully Started " + sucessCount + " Tasks";
                    }
                }
                    break;
                case "pull":
                {
                    foreach (var computerId in selectedComputers)
                    {
                        RequiresAuthorizationOrManagedComputer(Authorizations.ImageUploadTask, computerId);
                        if (Call.ComputerApi.StartUpload(computerId).Contains("Successfully"))
                            sucessCount++;
                        EndUserMessage = "Successfully Started " + sucessCount + " Tasks";
                    }
                }
                    break;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            if (Settings.DefaultComputerView == "all")
                PopulateGrid();
        }

        protected void PopulateGrid()
        {
            var limit = 0;
            limit = ddlLimit.Text == "All" ? int.MaxValue : Convert.ToInt32(ddlLimit.Text);
            var listOfComputers = Call.ComputerApi.Search(limit, txtSearch.Text);
            gvComputers.DataSource = listOfComputers.GroupBy(c => c.Id).Select(g => g.First()).ToList();
            gvComputers.DataBind();

            lblTotal.Text = gvComputers.Rows.Count + " Result(s) / " + Call.ComputerApi.GetCount() +
                            " Total Computer(s)";
        }

        protected void search_Changed(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        protected void SingleOkButton_Click(object sender, EventArgs e)
        {
            var computerId = Convert.ToInt32(Session["computerID"]);
            var direction = (string) Session["direction"];

            if (direction == "permanent_push")
            {
                RequiresAuthorizationOrManagedComputer(Authorizations.ImageDeployTask, computerId);
                EndUserMessage = Call.ComputerApi.StartPermanentDeploy(computerId);
            }
            else if (direction == "push")
            {
                RequiresAuthorizationOrManagedComputer(Authorizations.ImageDeployTask, computerId);
                EndUserMessage = Call.ComputerApi.StartDeploy(computerId);
            }
            else
            {
                RequiresAuthorizationOrManagedComputer(Authorizations.ImageUploadTask, computerId);
                EndUserMessage = Call.ComputerApi.StartUpload(computerId);
            }
            Session.Remove("computerID");
            Session.Remove("direction");
        }
    }
}