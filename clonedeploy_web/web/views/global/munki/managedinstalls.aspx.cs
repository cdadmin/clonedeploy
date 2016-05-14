using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Helpers;

public partial class views_global_munki_managedinstalls : BasePages.Global
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        PopulateGrid();
    }

    protected void PopulateGrid()
    {
        var availableLimit = ddlLimitAvailable.Text == "All" ? Int32.MaxValue : Convert.ToInt32(ddlLimitAvailable.Text);
        var assignedLimit = ddlLimitAssigned.Text == "All" ? Int32.MaxValue : Convert.ToInt32(ddlLimitAssigned.Text);

        var templateInstalls = BLL.MunkiManagedInstall.GetAllManagedInstallsForMt(ManifestTemplate.Id).Where(s => s.Name.IndexOf(txtSearchAssigned.Text, StringComparison.CurrentCultureIgnoreCase) != -1).OrderBy(x => x.Name).Take(assignedLimit);
        gvTemplateInstalls.DataSource = templateInstalls;
        gvTemplateInstalls.DataBind();

        lblTotalAssigned.Text = gvTemplateInstalls.Rows.Count + " Result(s) / " + BLL.MunkiManagedInstall.TotalCount(ManifestTemplate.Id) + " Total Managed Install(s)";


        var listOfPackages = new List<Models.MunkiPackageInfo>();
        foreach (var pkgInfoFile in GetMunkiResources("pkgsinfo"))
        {
            var pkg = ReadPlist(pkgInfoFile.FullName);
            if(pkg != null)
            listOfPackages.Add(pkg);    
        }

        listOfPackages = listOfPackages.Where(s => s.Name.IndexOf(txtSearchAvailable.Text, StringComparison.CurrentCultureIgnoreCase) != -1).Take(availableLimit).ToList();
        gvPkgInfos.DataSource = listOfPackages;
        gvPkgInfos.DataBind();
 
        lblTotalAvailable.Text = gvPkgInfos.Rows.Count + " Result(s) / " + GetMunkiResources("pkgsinfo").Length + " Total Packages(s)";
    }

    protected void buttonUpdate_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.UpdateGlobal);

        var updateCount = 0;
        foreach (GridViewRow row in gvPkgInfos.Rows)
        {
            var enabled = (CheckBox)row.FindControl("chkSelector");
            if (enabled == null) continue;
            if (!enabled.Checked) continue;

            var dataKey = gvPkgInfos.DataKeys[row.RowIndex];
            if (dataKey == null) continue;

            var managedInstall = new Models.MunkiManifestManagedInstall
            {
                Name = dataKey.Value.ToString(),
                ManifestTemplateId = ManifestTemplate.Id,
            };

            

            var cbUseVersion = (CheckBox)row.FindControl("chkUseVersion");
            if (cbUseVersion.Checked)
            {
                managedInstall.Version = row.Cells[2].Text;
                managedInstall.IncludeVersion = 1;
            }

            if (BLL.MunkiManagedInstall.AddManagedInstallToTemplate(managedInstall)) updateCount++;
        }

        var deleteCount = 0;
        foreach (GridViewRow row in gvTemplateInstalls.Rows)
        {
            var enabled = (CheckBox)row.FindControl("chkSelector");
            if (enabled == null) continue;
            var dataKey = gvTemplateInstalls.DataKeys[row.RowIndex];
            if (dataKey == null) continue;

            if (!enabled.Checked)
            {
                if (BLL.MunkiManagedInstall.DeleteManagedInstallFromTemplate(Convert.ToInt32(dataKey.Value)))
                    deleteCount++;
            }
       
        }

        if (deleteCount > 0 || updateCount > 0)
            EndUserMessage = "Successfully Updated Managed Installs";
        else
            EndUserMessage = "Could Not Update Managed Installs";

        PopulateGrid();
    }

    protected void search_Changed(object sender, EventArgs e)
    {
        PopulateGrid();
    }

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        ChkAll(gvPkgInfos);
    }

    protected void showAssigned_OnClick(object sender, EventArgs e)
    {
        Assigned.Visible = !Assigned.Visible;
    }

    protected void showAvailable_OnClick(object sender, EventArgs e)
    {
        Available.Visible = !Available.Visible;
    }

    protected void ddlLimit_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateGrid();
    }
}