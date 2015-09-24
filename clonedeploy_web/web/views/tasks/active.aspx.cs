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
        private readonly BLL.ActiveImagingTask _bllActiveImagingTask = new BLL.ActiveImagingTask();
        private readonly BLL.ActiveMulticastSession _bllActiveMulticastSession = new BLL.ActiveMulticastSession();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            ViewState["clickTracker"] = "1";
            gvTasks.DataSource = _bllActiveImagingTask.ReadAll();
            gvTasks.DataBind();
            gvUcTasks.DataSource = _bllActiveImagingTask.ReadUnicasts();
            gvUcTasks.DataBind();
            gvMcTasks.DataSource = _bllActiveMulticastSession.GetAllMulticastSessions();
            gvMcTasks.DataBind();
            GetMcInfo();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            var control = sender as Control;
            if (control != null)
            {
                var gvRow = (GridViewRow) control.Parent.Parent;
                var dataKey = gvTasks.DataKeys[gvRow.RowIndex];
                if (dataKey != null)

                    _bllActiveImagingTask.DeleteActiveImagingTask(Convert.ToInt32(dataKey.Value));

            }
            gvTasks.DataSource = _bllActiveImagingTask.ReadAll();
            gvTasks.DataBind();
            gvUcTasks.DataSource = _bllActiveImagingTask.ReadUnicasts();
            gvUcTasks.DataBind();
        }

        protected void btnCancelMc_Click(object sender, EventArgs e)
        {

            var control = sender as Control;
            if (control != null)
            {
                var gvRow = (GridViewRow) control.Parent.Parent;
                var dataKey = gvMcTasks.DataKeys[gvRow.RowIndex];
                if (dataKey != null)
                {
                    _bllActiveMulticastSession.Delete(Convert.ToInt32(dataKey.Value));
                }
            }
            gvMcTasks.DataSource = _bllActiveMulticastSession.GetAllMulticastSessions();
            gvMcTasks.DataBind();
            gvTasks.DataSource = _bllActiveImagingTask.ReadAll();
            gvTasks.DataBind();
        }

        protected void btnMembers_Click(object sender, EventArgs e)
        {
            int cTracker = Convert.ToInt16(ViewState["clickTracker"]);
            TimerMC.Enabled = cTracker%2 == 0;
            ViewState["clickTracker"] = cTracker + 1;

            var control = sender as Control;
            if (control != null)
            {
                var gvRow = (GridViewRow) control.Parent.Parent;
                var gv = (GridView) gvRow.FindControl("gvMembers");

                if (gv.Visible == false)
                {
                    var td = gvRow.FindControl("tdMembers");
                    td.Visible = true;
                    gv.Visible = true;

                    var table = _bllActiveImagingTask.MulticastMemberStatus(Convert.ToInt32(gvRow.Cells[1].Text));
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

        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            gvTasks.Visible = !gvTasks.Visible;
        }

        protected void cancelTasks_Click(object sender, EventArgs e)
        {
            _bllActiveImagingTask.CancelAll();
            gvMcTasks.DataSource = _bllActiveMulticastSession.GetAllMulticastSessions();
            gvMcTasks.DataBind();
            gvUcTasks.DataSource = _bllActiveImagingTask.ReadUnicasts();
            gvUcTasks.DataBind();
            gvTasks.DataSource = _bllActiveImagingTask.ReadAll();
            gvTasks.DataBind();
        }

        protected void GetMcInfo()
        {
            foreach (GridViewRow row in gvMcTasks.Rows)
            {
                try
                {
                    var listActive = _bllActiveImagingTask.MulticastProgress(Convert.ToInt32(row.Cells[1].Text));
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
            gvTasks.DataSource = _bllActiveImagingTask.ReadAll();
            gvTasks.DataBind();
            gvUcTasks.DataSource = _bllActiveImagingTask.ReadUnicasts();
            gvUcTasks.DataBind();
            UpdatePanel1.Update();
        }

        protected void TimerMC_Tick(object sender, EventArgs e)
        {
            gvMcTasks.DataSource = _bllActiveMulticastSession.GetAllMulticastSessions();
            gvMcTasks.DataBind();
            GetMcInfo();
        }
    }
}