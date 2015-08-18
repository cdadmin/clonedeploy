/*  
    CrucibleWDS A Windows Deployment Solution
    Copyright (C) 2011  Jon Dolny

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/.
 */

using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Global;
using Models;

namespace views.tasks
{
    public partial class TaskActive : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            ViewState["clickTracker"] = "1";
            gvTasks.DataSource = ActiveTask.ReadAll();
            gvTasks.DataBind();
            gvUcTasks.DataSource = ActiveTask.ReadUnicasts();
            gvUcTasks.DataBind();
            gvMcTasks.DataSource = ActiveMcTask.ReadMulticasts();
            gvMcTasks.DataBind();
            GetMcInfo();
            Master.Master.Msgbox(Utility.Message);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            var activeTask = new ActiveTask();
            var control = sender as Control;
            if (control != null)
            {
                var gvRow = (GridViewRow) control.Parent.Parent;
                var dataKey = gvTasks.DataKeys[gvRow.RowIndex];
                if (dataKey != null)
                {
                    activeTask.Id = dataKey.Value.ToString();
                    activeTask.Delete();
                }
            }
            Master.Master.Msgbox(Utility.Message);
            gvTasks.DataSource = ActiveTask.ReadAll();
            gvTasks.DataBind();
            gvUcTasks.DataSource = ActiveTask.ReadUnicasts();
            gvUcTasks.DataBind();
        }

        protected void btnCancelMc_Click(object sender, EventArgs e)
        {
            var mcTask = new ActiveMcTask();
            var control = sender as Control;
            if (control != null)
            {
                var gvRow = (GridViewRow) control.Parent.Parent;
                var dataKey = gvMcTasks.DataKeys[gvRow.RowIndex];
                if (dataKey != null)
                {
                    mcTask.Id = dataKey.Value.ToString();
                    mcTask.Name = gvRow.Cells[2].Text;
                    mcTask.Pid = (gvRow.Cells[3].Text);
                    mcTask.Delete();
                }
            }
            Master.Master.Msgbox(Utility.Message);
            gvMcTasks.DataSource = ActiveMcTask.ReadMulticasts();
            gvMcTasks.DataBind();
            gvTasks.DataSource = ActiveTask.ReadAll();
            gvTasks.DataBind();
        }

        protected void btnMembers_Click(object sender, EventArgs e)
        {
            int cTracker = Convert.ToInt16(ViewState["clickTracker"]);
            TimerMC.Enabled = cTracker%2 == 0;
            ViewState["clickTracker"] = cTracker + 1;
            var task = new ActiveTask();
            var control = sender as Control;
            if (control != null)
            {
                var gvRow = (GridViewRow) control.Parent.Parent;
                var gv = (GridView) gvRow.FindControl("gvMembers");

                task.MulticastName = gvRow.Cells[2].Text;
                if (gv.Visible == false)
                {
                    var td = gvRow.FindControl("tdMembers");
                    td.Visible = true;
                    gv.Visible = true;

                    var table = task.MulticastMemberStatus();
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
            Master.Master.Msgbox(Utility.Message);
        }

        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            gvTasks.Visible = !gvTasks.Visible;
        }

        protected void cancelTasks_Click(object sender, EventArgs e)
        {
            ActiveTask.CancelAll();
            gvMcTasks.DataSource = ActiveMcTask.ReadMulticasts();
            gvMcTasks.DataBind();
            gvUcTasks.DataSource = ActiveTask.ReadUnicasts();
            gvUcTasks.DataBind();
            gvTasks.DataSource = ActiveTask.ReadAll();
            gvTasks.DataBind();
            Master.Master.Msgbox(Utility.Message);
        }

        protected void GetMcInfo()
        {
            foreach (GridViewRow row in gvMcTasks.Rows)
            {
                try
                {
                    var task = new ActiveTask {MulticastName = row.Cells[2].Text};
                    var listActive = task.MulticastProgress();
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

        protected void Timer_Tick(object sender, EventArgs e)
        {
            gvTasks.DataSource = ActiveTask.ReadAll();
            gvTasks.DataBind();
            gvUcTasks.DataSource = ActiveTask.ReadUnicasts();
            gvUcTasks.DataBind();
            UpdatePanel1.Update();
        }

        protected void TimerMC_Tick(object sender, EventArgs e)
        {
            gvMcTasks.DataSource = ActiveMcTask.ReadMulticasts();
            gvMcTasks.DataBind();
            GetMcInfo();
        }
    }
}