using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Helpers;

namespace views.tasks
{
    public partial class TaskActive : BasePages.Tasks
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            ViewState["clickTracker"] = "1";
            gvTasks.DataSource = BLL.ActiveImagingTask.ReadAll();
            gvTasks.DataBind();
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
            gvTasks.DataSource = BLL.ActiveImagingTask.ReadAll();
            gvTasks.DataBind();
        }

        protected void cancelTasks_Click(object sender, EventArgs e)
        {
            BLL.ActiveImagingTask.CancelAll();     
            gvTasks.DataSource = BLL.ActiveImagingTask.ReadAll();
            gvTasks.DataBind();
        }

      
        protected void Timer_Tick(object sender, EventArgs e)
        {
            gvTasks.DataSource = BLL.ActiveImagingTask.ReadAll();
            gvTasks.DataBind();
            UpdatePanel1.Update();
        }

      
    }
}