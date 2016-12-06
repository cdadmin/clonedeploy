﻿using System;
using System.Web.UI.WebControls;


public partial class views_global_munki_assignedmanageduninstalls : BasePages.Global
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        PopulateGrid();
    }

    protected void PopulateGrid()
    {
      
        var assignedLimit = ddlLimitAssigned.Text == "All" ? Int32.MaxValue : Convert.ToInt32(ddlLimitAssigned.Text);

        var templateInstalls = BLL.MunkiManagedUninstall.GetAllManagedUnInstallsForMt(ManifestTemplate.Id).Where(s => s.Name.IndexOf(txtSearchAssigned.Text, StringComparison.CurrentCultureIgnoreCase) != -1).OrderBy(x => x.Name).Take(assignedLimit);
        gvTemplateInstalls.DataSource = templateInstalls;
        gvTemplateInstalls.DataBind();

        lblTotalAssigned.Text = gvTemplateInstalls.Rows.Count + " Result(s) / " + BLL.MunkiManagedUninstall.TotalCount(ManifestTemplate.Id) + " Total Managed Uninstall(s)";
     
    }

    protected void buttonUpdate_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.UpdateGlobal);

       

        var updateCount = 0;
        foreach (GridViewRow row in gvTemplateInstalls.Rows)
        {
            var enabled = (CheckBox)row.FindControl("chkSelector");
            if (enabled == null) continue;
            var dataKey = gvTemplateInstalls.DataKeys[row.RowIndex];
            if (dataKey == null) continue;

            if (!enabled.Checked)
            {
                if (BLL.MunkiManagedUninstall.DeleteManagedUnInstallFromTemplate(Convert.ToInt32(dataKey.Value)))
                    updateCount++;
            }

            else
            {
                var managedUnInstall = BLL.MunkiManagedUninstall.GetManagedUnInstall(Convert.ToInt32(dataKey.Value));

                var txtCondition = row.FindControl("txtCondition") as TextBox;
                if (txtCondition != null)
                    if (txtCondition.Text != managedUnInstall.Condition)
                        managedUnInstall.Condition = txtCondition.Text;

                if (BLL.MunkiManagedUninstall.AddManagedUnInstallToTemplate(managedUnInstall)) updateCount++;
            } 

        }

        if (updateCount > 0)
        {
            EndUserMessage = "Successfully Updated Managed Uninstalls";
            ManifestTemplate.ChangesApplied = 0;
            BLL.MunkiManifestTemplate.UpdateManifest(ManifestTemplate);
        }
        else
        {
            EndUserMessage = "Could Not Update Managed Uninstalls";
        }

      
        PopulateGrid();
    }

    protected void search_Changed(object sender, EventArgs e)
    {
        PopulateGrid();
    }

  

    protected void showAssigned_OnClick(object sender, EventArgs e)
    {
        Assigned.Visible = !Assigned.Visible;
    }

   
    protected void ddlLimit_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateGrid();
    }
}