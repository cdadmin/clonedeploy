using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using CloneDeploy_Web.BasePages;

namespace views.computers
{
    public partial class ComputerLog : Computers
    {
        protected void btnExport_OnClick(object sender, EventArgs e)
        {
            var control = sender as Control;
            if (control == null) return;
            var gvRow = (GridViewRow) control.Parent.Parent;
            var dataKey = gvLogs.DataKeys[gvRow.RowIndex];
            if (dataKey == null) return;
            var log = Call.ComputerLogApi.Get(Convert.ToInt32(dataKey.Value));
            Export(Computer.Name + "-" + log.SubType + ".txt", log.Contents);
        }

        protected void btnView_OnClick(object sender, EventArgs e)
        {
            var control = sender as Control;
            if (control == null) return;
            var gvRow = (GridViewRow) control.Parent.Parent;
            var dataKey = gvLogs.DataKeys[gvRow.RowIndex];
            if (dataKey == null) return;
            var log = Call.ComputerLogApi.Get(Convert.ToInt32(dataKey.Value));

            SearchLogs.Visible = false;
            ViewLog.Visible = true;

            // I didn't want a textbox for this, that's why it seems strange.
            var text = new List<string>();
            using (var reader = new StringReader(log.Contents))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                    text.Add(line);
            }
            gvLogView.DataSource = text;
            gvLogView.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) PopulateLogs();
        }

        protected void PopulateLogs()
        {
            gvLogs.DataSource = Call.ComputerApi.GetComputerLogs(Computer.Id);
            gvLogs.DataBind();
        }
    }
}