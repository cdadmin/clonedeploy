using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace views.tasks
{
    public partial class TaskActive : BasePages.Tasks
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            cancelTasks.Visible = Call.CloneDeployUserApi.IsAdmin(CloneDeployCurrentUser.Id);
            if (IsPostBack) return;
            ViewState["clickTracker"] = "1";
            PopulateGrid();
            lblTotal.Text = Call.ActiveImagingTaskApi.GetAllActiveCount() + " Total Tasks(s)";
        }

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

      
        protected void Timer_Tick(object sender, EventArgs e)
        {
            PopulateGrid();
            lblTotal.Text = Call.ActiveImagingTaskApi.GetAllActiveCount() + " Total Tasks(s)";
            UpdatePanel1.Update();
        }

        private void PopulateGrid()
        {
            gvTasks.DataSource = Call.ActiveImagingTaskApi.GetActiveTasks();
            gvTasks.DataBind();
        }
      
    }
}