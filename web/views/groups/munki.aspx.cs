using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Helpers;

public partial class views_groups_munki : BasePages.Groups
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;

        PopulateGrid();
    }

    protected void PopulateGrid()
    {
        gvManifestTemplates.DataSource = BLL.MunkiManifestTemplate.SearchManifests();
        gvManifestTemplates.DataBind();

     

        var listOfTemplates = BLL.GroupMunki.Get(Group.Id);
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
        List<Models.MunkiManifestTemplate> listManifestTemplates = (List<Models.MunkiManifestTemplate>)gvManifestTemplates.DataSource;
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
        RequiresAuthorizationOrManagedGroup(Authorizations.UpdateGroup, Group.Id);
        var list = new List<Models.GroupMunki>();
        foreach (GridViewRow row in gvManifestTemplates.Rows)
        {
            var cb = (CheckBox)row.FindControl("chkSelector");
            if (cb == null || !cb.Checked) continue;
            var dataKey = gvManifestTemplates.DataKeys[row.RowIndex];
            if (dataKey == null) continue;
            var template = new Models.GroupMunki
            {
                GroupId = Group.Id,
                MunkiTemplateId = Convert.ToInt32(dataKey.Value)
            };
            list.Add(template);

        }

        BLL.GroupMunki.DeleteMunkiTemplates(Group.Id);
        if(list.Count > 0)
        {
            EndUserMessage = BLL.GroupMunki.AddMunkiTemplates(list)
            ? "Successfully Updated Munki Templates"
            : "Could Not Update Munki Templates";
        }
    }

    protected void effective_OnClick(object sender, EventArgs e)
    {
        var effectiveManifest = new BLL.Workflows.EffectiveMunkiTemplate().Group(Group.Id);
        Response.Write(Encoding.UTF8.GetString(effectiveManifest.ToArray()));
        Response.ContentType = "text/plain";
        Response.End();
    }
}