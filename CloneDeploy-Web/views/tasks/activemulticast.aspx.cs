using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using CloneDeploy_Web.BasePages;

public partial class views_tasks_activemulticast : Tasks
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        ViewState["clickTracker"] = "1";
        PopulateGrid();
             
    }

    private void PopulateGrid()
    {
        gvMcTasks.DataSource = Call.ActiveMulticastSessionApi.GetAll(Int32.MaxValue,"");
        gvMcTasks.DataBind();
        lblTotal.Text = Call.ActiveMulticastSessionApi.GetCount() + " Total Multicast(s)";
        GetMcInfo();
    }

    protected void TimerMC_Tick(object sender, EventArgs e)
    {
        PopulateGrid();
    }

    protected void btnCancelMc_Click(object sender, EventArgs e)
    {

        var control = sender as Control;
        if (control != null)
        {
            var gvRow = (GridViewRow)control.Parent.Parent;
            var dataKey = gvMcTasks.DataKeys[gvRow.RowIndex];
            if (dataKey != null)
            {
                Call.ActiveMulticastSessionApi.Delete(Convert.ToInt32(dataKey.Value));
            }
        }
        PopulateGrid();
    }

    protected void btnMembers_Click(object sender, EventArgs e)
    {
        int cTracker = Convert.ToInt16(ViewState["clickTracker"]);
        TimerMC.Enabled = cTracker % 2 == 0;
        ViewState["clickTracker"] = cTracker + 1;

        var control = sender as Control;
        if (control != null)
        {
            var gvRow = (GridViewRow)control.Parent.Parent;
            var gv = (GridView)gvRow.FindControl("gvMembers");

            if (gv.Visible == false)
            {
                var td = gvRow.FindControl("tdMembers");
                td.Visible = true;
                gv.Visible = true;

                var dataKey = gvMcTasks.DataKeys[gvRow.RowIndex];
                if (dataKey != null)
                {
                    var table = Call.ActiveMulticastSessionApi.GetMemberStatus(Convert.ToInt32(dataKey.Value));
                    gv.DataSource = table;
                    gv.DataBind();
                }

            }
            else
            {
                gv.Visible = false;
                var td = gvRow.FindControl("tdMembers");
                td.Visible = false;
            }
        }
    }

    protected void GetMcInfo()
    {
        foreach (GridViewRow row in gvMcTasks.Rows)
        {
            var dataKey = gvMcTasks.DataKeys[row.RowIndex];
            var mcId = 0;
            if (dataKey != null)
            {
                mcId = Convert.ToInt32(dataKey.Value);
            }
            try
            {
                var listActive = Call.ActiveMulticastSessionApi.GetProgress(mcId);
                var lblPartition = row.FindControl("lblPartition") as Label;
                var lblElapsed = row.FindControl("lblElapsed") as Label;
                var lblRemaining = row.FindControl("lblRemaining") as Label;
                var lblCompleted = row.FindControl("lblCompleted") as Label;
                var lblRate = row.FindControl("lblRate") as Label;
                foreach (var activeTask in listActive)
                {
                    if (lblPartition != null) lblPartition.Text = activeTask.Partition;
                    lblElapsed.Text = activeTask.Elapsed;
                    lblRemaining.Text = activeTask.Remaining;
                    lblCompleted.Text = activeTask.Completed;
                    lblRate.Text = activeTask.Rate;
                }
            }
            catch
            {
                // ignored
            }
        }
    }
}