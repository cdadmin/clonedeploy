using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

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

        protected void btnPermanentDeploy_Click(object sender, EventArgs e)
        {
            SingleOkButton.Visible = true;
            var control = sender as Control;
            if (control != null)
            {
                var gvRow = (GridViewRow)control.Parent.Parent;
                var dataKey = gvComputers.DataKeys[gvRow.RowIndex];
                if (dataKey != null)
                {
                    var computer = BLL.Computer.GetComputer(Convert.ToInt32(dataKey.Value));
                    Session["computerID"] = computer.Id;
                    Session["direction"] = "permanent_push";
                    lblTitle.Text = "Permanent Deploy The Selected Computer?";
                    gvConfirm.DataSource = new List<Computer> { computer };
                }
            }
            gvConfirm.DataBind();
           DisplayConfirm();
        }

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
                    var computer = BLL.Computer.GetComputer(Convert.ToInt32(dataKey.Value));
                    Session["computerID"] = computer.Id;
                    Session["direction"] = "push";
                    lblTitle.Text = "Deploy The Selected Computer?";
                    gvConfirm.DataSource = new List<Computer> { computer };
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
                    var computer = BLL.Computer.GetComputer(Convert.ToInt32(dataKey.Value));
                    Session["computerID"] = computer.Id;
                    Session["direction"] = "pull";
                    lblTitle.Text = "Upload The Selected Computer?";
                    gvConfirm.DataSource = new List<Computer> { computer };
                }
            }
            gvConfirm.DataBind();
            DisplayConfirm();
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

        protected void btnListDeploy_Click(object sender, EventArgs e)
        {
            MultiOkButton.Visible = true;
            Session["action"] = "push";
            lblTitle.Text = "Deploy The Selected Computers?";
            DisplayConfirm();
        }

        protected void btnListUpload_Click(object sender, EventArgs e)
        {
            MultiOkButton.Visible = true;
            Session["action"] = "pull";
            lblTitle.Text = "Upload The Selected Computers?";
            DisplayConfirm();
        }

        protected void btnListPermanentDeploy_Click(object sender, EventArgs e)
        {
            MultiOkButton.Visible = true;
            Session["action"] = "permanent_push";
            lblTitle.Text = "Permanent Deploy The Selected Computers?";
            DisplayConfirm();
        }

        protected void MultiOkButton_Click(object sender, EventArgs e)
        {
            var action = (string)(Session["action"]);
            Session.Remove("action");

            var selectedComputers = new List<int>();
            
            foreach (GridViewRow row in gvComputers.Rows)
            {
                var cb = (CheckBox)row.FindControl("chkSelector");
                if (cb == null || !cb.Checked) continue;
                var dataKey = gvComputers.DataKeys[row.RowIndex];
                if (dataKey == null) continue;
                selectedComputers.Add(Convert.ToInt32(dataKey.Value));
            }
           
            var sucessCount = 0;
            switch (action)
            {
                case "push": case "permanent_push":
                    {
                        foreach (var computerId in selectedComputers)
                        {
                            var computer = BLL.Computer.GetComputer(computerId);
                            RequiresAuthorizationOrManagedComputer(Authorizations.ImageDeployTask, computer.Id);
                            if (new BLL.Workflows.Unicast(computer, action, CloneDeployCurrentUser.Id)
                                .Start().Contains("Successfully"))
                                sucessCount++;
                            EndUserMessage = "Successfully Started " + sucessCount + " Tasks";
                        }
                    }
                    break;
                case "pull":
                    {
                        foreach (var computerId in selectedComputers)
                        {
                            var computer = BLL.Computer.GetComputer(computerId);
                            RequiresAuthorizationOrManagedComputer(Authorizations.ImageUploadTask, computer.Id);
                            if (new BLL.Workflows.Unicast(computer, action, CloneDeployCurrentUser.Id)
                                .Start().Contains("Successfully"))
                                sucessCount++;
                            EndUserMessage = "Successfully Started " + sucessCount + " Tasks";
                        } 
                    }
                    break;
            }
        }
        protected void SingleOkButton_Click(object sender, EventArgs e)
        {
            var computer = BLL.Computer.GetComputer(Convert.ToInt32(Session["computerID"]));
            var direction = (string) (Session["direction"]);

            if (direction == "push" || direction == "permanent_push")
            {
                RequiresAuthorizationOrManagedComputer(Authorizations.ImageDeployTask, computer.Id);    
                EndUserMessage = new BLL.Workflows.Unicast(computer,direction,CloneDeployCurrentUser.Id).Start();               
             
            }
            else
            {
                RequiresAuthorizationOrManagedComputer(Authorizations.ImageUploadTask, computer.Id);
                EndUserMessage = new BLL.Workflows.Unicast(computer,direction,CloneDeployCurrentUser.Id).Start();
            }
            Session.Remove("computerID");
            Session.Remove("direction");
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

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            ChkAll(gvComputers);
        }
    }
}