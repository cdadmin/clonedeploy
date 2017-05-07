using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using CloneDeploy_Web.BasePages;

public partial class views_tasks_activepermanent : Tasks
{
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        var control = sender as Control;
        if (control != null)
        {
            var gvRow = (GridViewRow) control.Parent.Parent;
            var dataKey = gvUcTasks.DataKeys[gvRow.RowIndex];
            if (dataKey != null)

                Call.ActiveImagingTaskApi.Delete(Convert.ToInt32(dataKey.Value));
        }
        PopulateGrid();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        ViewState["clickTracker"] = "1";
        PopulateGrid();
        lblTotal.Text = Call.ActiveImagingTaskApi.GetActiveUnicastCount("permanent_push") + " Total Permanent Task(s)";
    }

    private void PopulateGrid()
    {
        gvUcTasks.DataSource = Call.ActiveImagingTaskApi.GetUnicasts("permanent_push");
        gvUcTasks.DataBind();
    }

    protected void Timer_Tick(object sender, EventArgs e)
    {
        PopulateGrid();
        lblTotal.Text = Call.ActiveImagingTaskApi.GetActiveUnicastCount("permanent_push") + " Total Permanent Task(s)";
        UpdatePanel1.Update();
    }
}