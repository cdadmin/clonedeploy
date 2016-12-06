using System;
using System.Web.UI.WebControls;

public partial class views_global_munki_catalogs : BasePages.Global
{
    protected void Page_Load(object sender, EventArgs e)
    {
       

        if (IsPostBack) return;
        PopulateGrid();
    }

    protected void PopulateGrid()
    {
        var templateCatalogs = BLL.MunkiCatalog.GetAllCatalogsForMt(ManifestTemplate.Id).Where(f => f.Name.IndexOf(TextBox1.Text, StringComparison.CurrentCultureIgnoreCase) != -1).OrderBy(x => x.Priority);
        gvTemplateCatalogs.DataSource = templateCatalogs;
        gvTemplateCatalogs.DataBind();
      
     
    }

    protected void search_Changed(object sender, EventArgs e)
    {
        PopulateGrid();
    }

    protected void buttonUpdate_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.UpdateGlobal);

        var updateCount = 0;
        foreach (GridViewRow row in gvTemplateCatalogs.Rows)
        {
            var enabled = (CheckBox)row.FindControl("chkSelector");
            if (enabled == null) continue;
            var dataKey = gvTemplateCatalogs.DataKeys[row.RowIndex];
            if (dataKey == null) continue;

            if (!enabled.Checked)
            {
                if(BLL.MunkiCatalog.DeleteCatalogFromTemplate(Convert.ToInt32(dataKey.Value)))
                updateCount++;
            }
            else
            {
                var catalog = BLL.MunkiCatalog.GetCatalog(Convert.ToInt32(dataKey.Value));
              
                var txtPriority = row.FindControl("txtPriority") as TextBox;
                if (txtPriority != null)
                    if (!string.IsNullOrEmpty(txtPriority.Text))
                        catalog.Priority = Convert.ToInt32(txtPriority.Text);

                if (BLL.MunkiCatalog.AddCatalogToTemplate(catalog)) updateCount++;
            }       
        }

        if (updateCount > 0)
        {
            EndUserMessage = "Successfully Updated Catalogs";
            ManifestTemplate.ChangesApplied = 0;
            BLL.MunkiManifestTemplate.UpdateManifest(ManifestTemplate);
        }
        else
        {
            EndUserMessage = "Could Not Update Catalogs";
        }


        PopulateGrid();
    }


    protected void showAssigned_OnClick(object sender, EventArgs e)
    {
        Assigned.Visible = !Assigned.Visible;
    }

  
}