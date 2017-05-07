using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using CloneDeploy_Web.BasePages;

namespace views.tasks
{
    public partial class TaskActive : Tasks
    {
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            var control = sender as Control;
            if (control != null)
            {
                var gvRow = (GridViewRow) control.Parent.Parent;
                var dataKey = gvTasks.DataKeys[gvRow.RowIndex];
                if (dataKey != null)

                    Call.ActiveImagingTaskApi.Delete(Convert.ToInt32(dataKey.Value));
            }
            gvTasks.DataSource = Call.ActiveImagingTaskApi.GetActiveTasks();
            gvTasks.DataBind();
        }

        protected void cancelTasks_Click(object sender, EventArgs e)
        {
            Call.WorkflowApi.CancelAllImagingTasks();
            PopulateGrid();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Call.CloneDeployUserApi.IsAdmin(CloneDeployCurrentUser.Id))
            {
                lblTotalNotOwned.Visible = false;
                cancelTasks.Visible = true;
            }
            else
            {
                lblTotalNotOwned.Text = Call.ActiveImagingTaskApi.GetActiveNotOwned() + " Task(s) Not Visible";
                lblTotalNotOwned.Visible = true;
                cancelTasks.Visible = false;
            }

            if (IsPostBack) return;
            ViewState["clickTracker"] = "1";
            PopulateGrid();
            lblTotal.Text = Call.ActiveImagingTaskApi.GetAllActiveCount() + " Total Tasks(s)";
        }

        private void PopulateGrid()
        {
            gvTasks.DataSource = Call.ActiveImagingTaskApi.GetActiveTasks();
            gvTasks.DataBind();
        }


        protected void Timer_Tick(object sender, EventArgs e)
        {
            PopulateGrid();
            lblTotal.Text = Call.ActiveImagingTaskApi.GetAllActiveCount() + " Total Tasks(s)";
            UpdatePanel1.Update();
        }
    }
}