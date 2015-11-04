using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Helpers;

public partial class views_tasks_activemulticast : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        ViewState["clickTracker"] = "1";
        gvMcTasks.DataSource = BLL.ActiveMulticastSession.GetAllMulticastSessions();
        gvMcTasks.DataBind();
        GetMcInfo();
    }

    protected void TimerMC_Tick(object sender, EventArgs e)
    {
        gvMcTasks.DataSource = BLL.ActiveMulticastSession.GetAllMulticastSessions();
        gvMcTasks.DataBind();
        GetMcInfo();
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
                BLL.ActiveMulticastSession.Delete(Convert.ToInt32(dataKey.Value));
            }
        }
        gvMcTasks.DataSource = BLL.ActiveMulticastSession.GetAllMulticastSessions();
        gvMcTasks.DataBind();
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

                var table = BLL.ActiveImagingTask.MulticastMemberStatus(Convert.ToInt32(gvRow.Cells[1].Text));
                gv.DataSource = table;
                gv.DataBind();
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
            try
            {
                var listActive = BLL.ActiveImagingTask.MulticastProgress(Convert.ToInt32(row.Cells[1].Text));
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
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
            }
        }
    }
}