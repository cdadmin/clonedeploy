using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CloneDeploy_ApiCalls;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.computers
{
    public partial class views_computers_munki : Computers
    {
        protected void btnAddSelected_OnClick(object sender, EventArgs e)
        {
            RequiresAuthorizationOrManagedGroup(AuthorizationStrings.UpdateComputer, Computer.Id);

            var list = new List<ComputerMunkiEntity>();
            foreach (GridViewRow row in gvManifestTemplates.Rows)
            {
                var cb = (CheckBox) row.FindControl("chkSelector");
                if (cb == null || !cb.Checked) continue;
                var dataKey = gvManifestTemplates.DataKeys[row.RowIndex];
                if (dataKey == null) continue;
                var template = new ComputerMunkiEntity
                {
                    ComputerId = Computer.Id,
                    MunkiTemplateId = Convert.ToInt32(dataKey.Value)
                };
                list.Add(template);
            }
            Call.ComputerApi.DeleteMunkiTemplates(Computer.Id);

            if (list.Count > 0)
            {
                var successCount = 0;
                foreach (var template in list)
                {
                    var result = Call.ComputerMunkiApi.Post(template);
                    if (result.Success) successCount++;
                }
                EndUserMessage = string.Format("Successfully Updated {0} Munki Templates", successCount);
            }
        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            ChkAll(gvManifestTemplates);
        }

        protected void effective_OnClick(object sender, EventArgs e)
        {
            var effectiveManifest = new APICall().ComputerApi.GetEffectiveManifest(Computer.Id);

            Response.Write(effectiveManifest);
            Response.ContentType = "text/plain";
            Response.End();
        }

        protected void gridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            PopulateGrid();
            var listManifestTemplates = (List<MunkiManifestTemplateEntity>) gvManifestTemplates.DataSource;
            switch (e.SortExpression)
            {
                case "Name":
                    listManifestTemplates = GetSortDirection(e.SortExpression) == "Asc"
                        ? listManifestTemplates.OrderBy(s => s.Name).ToList()
                        : listManifestTemplates.OrderByDescending(s => s.Name).ToList();
                    break;
            }

            gvManifestTemplates.DataSource = listManifestTemplates;
            gvManifestTemplates.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            PopulateGrid();
        }

        protected void PopulateGrid()
        {
            gvManifestTemplates.DataSource = Call.MunkiManifestTemplateApi.Get(0, "");
            gvManifestTemplates.DataBind();

            var listOfTemplates = Call.ComputerApi.GetMunkiTemplates(Computer.Id);

            foreach (GridViewRow row in gvManifestTemplates.Rows)
            {
                var chkBox = (CheckBox) row.FindControl("chkSelector");
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
    }
}