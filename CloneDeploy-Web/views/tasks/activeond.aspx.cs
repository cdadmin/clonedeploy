using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.tasks
{
    public partial class activeond : Tasks
    {
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            var control = sender as Control;
            if (control != null)
            {
                var gvRow = (GridViewRow) control.Parent.Parent;
                var dataKey = gvTasks.DataKeys[gvRow.RowIndex];
                if (dataKey != null)

                    Call.ActiveImagingTaskApi.DeleteOnDemand(Convert.ToInt32(dataKey.Value));
            }
            PopulateGrid();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            ViewState["clickTracker"] = "1";
            PopulateGrid();
            lblTotal.Text = Call.ActiveImagingTaskApi.OnDemandCount() + " Total Tasks(s)";
        }

        private void PopulateGrid()
        {
            gvTasks.DataSource = Call.ActiveImagingTaskApi.GetAllOnDemandUnregistered();
            gvTasks.DataBind();
        }

        protected void Timer_Tick(object sender, EventArgs e)
        {
            PopulateGrid();
            lblTotal.Text = Call.ActiveImagingTaskApi.OnDemandCount() + " Total Tasks(s)";
            UpdatePanel1.Update();
        }
    }
}