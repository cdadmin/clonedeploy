using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class views_tasks_activeunicast : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        ViewState["clickTracker"] = "1";
        gvUcTasks.DataSource = BLL.ActiveImagingTask.ReadUnicasts();
        gvUcTasks.DataBind();

    }
    protected void Timer_Tick(object sender, EventArgs e)
    {
        gvUcTasks.DataSource = BLL.ActiveImagingTask.ReadUnicasts();
        gvUcTasks.DataBind();
        UpdatePanel1.Update();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        var control = sender as Control;
        if (control != null)
        {
            var gvRow = (GridViewRow)control.Parent.Parent;
            var dataKey = gvUcTasks.DataKeys[gvRow.RowIndex];
            if (dataKey != null)

                BLL.ActiveImagingTask.DeleteActiveImagingTask(Convert.ToInt32(dataKey.Value));

        }
        gvUcTasks.DataSource = BLL.ActiveImagingTask.ReadUnicasts();
        gvUcTasks.DataBind();
    }
}