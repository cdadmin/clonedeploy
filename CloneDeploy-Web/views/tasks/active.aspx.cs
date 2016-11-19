using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL.Workflows;

namespace views.tasks
{
    public partial class TaskActive : BasePages.Tasks
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            cancelTasks.Visible = BLL.User.IsAdmin(CloneDeployCurrentUser.Id);
            if (IsPostBack) return;
            ViewState["clickTracker"] = "1";
            PopulateGrid();
            lblTotal.Text = BLL.ActiveImagingTask.AllActiveCount(CloneDeployCurrentUser.Id) + " Total Tasks(s)";
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            var control = sender as Control;
            if (control != null)
            {
                var gvRow = (GridViewRow) control.Parent.Parent;
                var dataKey = gvTasks.DataKeys[gvRow.RowIndex];
                if (dataKey != null)

                    BLL.ActiveImagingTask.DeleteActiveImagingTask(Convert.ToInt32(dataKey.Value));

            }
            gvTasks.DataSource = BLL.ActiveImagingTask.ReadAll(CloneDeployCurrentUser.Id);
            gvTasks.DataBind();
        }

        protected void cancelTasks_Click(object sender, EventArgs e)
        {
            CancelAllImagingTasks.Run();
            PopulateGrid();
        }

      
        protected void Timer_Tick(object sender, EventArgs e)
        {
            PopulateGrid();
            lblTotal.Text = BLL.ActiveImagingTask.AllActiveCount(CloneDeployCurrentUser.Id) + " Total Tasks(s)";
            UpdatePanel1.Update();
        }

        private void PopulateGrid()
        {
            gvTasks.DataSource = BLL.ActiveImagingTask.ReadAll(CloneDeployCurrentUser.Id);
            gvTasks.DataBind();
        }
      
    }
}