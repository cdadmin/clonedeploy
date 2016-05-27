using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Helpers;


public partial class views_global_munki_availablemanageduninstalls : BasePages.Global
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        PopulateGrid();
    }

    protected void PopulateGrid()
    {
        var availableLimit = ddlLimitAvailable.Text == "All" ? Int32.MaxValue : Convert.ToInt32(ddlLimitAvailable.Text);
     

        var listOfPackages = new List<Models.MunkiPackageInfo>();
        foreach (var pkgInfoFile in GetMunkiResources("pkgsinfo"))
        {
            var pkg = ReadPlist(pkgInfoFile.FullName);
            if (pkg != null)
                listOfPackages.Add(pkg);
        }

        listOfPackages = listOfPackages.Where(f => f.Name.IndexOf(txtSearchAvailable.Text, StringComparison.CurrentCultureIgnoreCase) != -1).Take(availableLimit).ToList();
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

            var managedUninstall = new Models.MunkiManifestManagedUnInstall()
            {
                Name = dataKey.Value.ToString(),
                ManifestTemplateId = ManifestTemplate.Id,
            };



            var cbUseVersion = (CheckBox)row.FindControl("chkUseVersion");
            if (cbUseVersion.Checked)
            {
                managedUninstall.Version = row.Cells[2].Text;
                managedUninstall.IncludeVersion = 1;
            }

            var condition = (TextBox)row.FindControl("txtCondition");
            managedUninstall.Condition = condition.Text;
            if (BLL.MunkiManagedUninstall.AddManagedUnInstallToTemplate(managedUninstall)) updateCount++;
        }

      

        EndUserMessage = updateCount > 0 ? "Successfully Updated Managed Uninstalls" : "Could Not Update Managed Uninstalls";

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

   
    protected void showAvailable_OnClick(object sender, EventArgs e)
    {
        Available.Visible = !Available.Visible;
    }

    protected void ddlLimit_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateGrid();
    }

    /*protected void Page_Load(object sender, EventArgs e)
    {
        NSDictionary root = new NSDictionary();

        NSArray catalogs = new NSArray(1);       
        catalogs.SetValue(0, "test");

        NSArray conditionalItems = new NSArray(1); 
        NSDictionary condition = new NSDictionary();
        condition.Add("condition", "os_version == 10.1");
        conditionalItems.SetValue(0, condition);

        NSArray includedManifests = new NSArray(1);
        includedManifests.SetValue(0, "manifest1");

        NSArray managedInstalls = new NSArray(1);
        managedInstalls.SetValue(0, "inst1");

        NSArray managedUninstalls = new NSArray(1);
        managedUninstalls.SetValue(0, "uninst1");

        NSArray managedUpdates = new NSArray(1);
        managedUpdates.SetValue(0, "up1");

        NSArray optionalInstalls = new NSArray(1);
        optionalInstalls.SetValue(0, "opt1");

        root.Add("catalogs", catalogs);
        root.Add("conditional_items", conditionalItems);
        root.Add("included_manifests", includedManifests);
        root.Add("managed_installs",managedInstalls);
        root.Add("managed_uninstalls",managedUninstalls);
        root.Add("managed_updates", managedUpdates);
        root.Add("optional_installs",optionalInstalls);

        //Save the propery list

        try
        {
            PropertyListParser.SaveAsXml(root, new FileInfo("C:\\intel\\my.plist"));
        }
        catch (Exception)
        {
           
           
        }
        
    }*/
}