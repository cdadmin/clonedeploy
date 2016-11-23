using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using CloneDeploy_Web.APICalls;
using CloneDeploy_Web.Models;
using Helpers;

public partial class views_computers_munki : BasePages.Computers
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;

        PopulateGrid();
    }

    protected void PopulateGrid()
    {
        var call = new APICall();
        gvManifestTemplates.DataSource = call.MunkiManifestTemplateApi.Get("");
        gvManifestTemplates.DataBind();


        var listOfTemplates = call.ComputerMunkiApi.Get(Computer.Id);
  
        foreach (GridViewRow row in gvManifestTemplates.Rows)
        {
            var chkBox = (CheckBox)row.FindControl("chkSelector");
            var dataKey = gvManifestTemplates.DataKeys[row.RowIndex];
            if (dataKey == null) continue;
            foreach (var template in listOfTemplates)
            {
                if (template.MunkiTemplateId == Convert.ToInt32(dataKey.Value))
                {
                    chkBox.Checked = true;
                }
            }
        }
    }



    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        ChkAll(gvManifestTemplates);
    }



    protected void gridView_Sorting(object sender, GridViewSortEventArgs e)
    {
        PopulateGrid();
        List<MunkiManifestTemplate> listManifestTemplates = (List<MunkiManifestTemplate>)gvManifestTemplates.DataSource;
        switch (e.SortExpression)
        {
            case "Name":
                listManifestTemplates = GetSortDirection(e.SortExpression) == "Asc" ? listManifestTemplates.OrderBy(s => s.Name).ToList() : listManifestTemplates.OrderByDescending(s => s.Name).ToList();
                break;

        }


        gvManifestTemplates.DataSource = listManifestTemplates;
        gvManifestTemplates.DataBind();
    }

    protected void btnAddSelected_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorizationOrManagedGroup(Authorizations.UpdateComputer, Computer.Id);
        var call = new APICall();
        var list = new List<ComputerMunki>();
        foreach (GridViewRow row in gvManifestTemplates.Rows)
        {
            var cb = (CheckBox)row.FindControl("chkSelector");
            if (cb == null || !cb.Checked) continue;
            var dataKey = gvManifestTemplates.DataKeys[row.RowIndex];
            if (dataKey == null) continue;
            var template = new ComputerMunki
            {
                ComputerId = Computer.Id,
                MunkiTemplateId = Convert.ToInt32(dataKey.Value)
            };
            list.Add(template);

        }
        call.ComputerMunkiApi.Delete(Computer.Id);
     
        if (list.Count > 0)
        {
            var successCount = 0;
            foreach (var template in list)
            {
                var result = call.ComputerMunkiApi.Post(template);
                if (result.Success) successCount++;
            }
            EndUserMessage = string.Format("Successfully Updated {0} Munki Templates",successCount);
        }
    }
    protected void effective_OnClick(object sender, EventArgs e)
    {
        var effectiveManifest = new APICall().ComputerMunkiApi.GetEffectiveManifest(Computer.Id);

        Response.Write(effectiveManifest);
        Response.ContentType = "text/plain";
        Response.End();
    }
}