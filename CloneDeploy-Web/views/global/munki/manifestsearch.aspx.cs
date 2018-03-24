using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.global.munki
{
    public partial class views_global_munki_manifestsearch : Global
    {
        protected void btnApply_OnClick(object sender, EventArgs e)
        {
            Session["manifestBtnClick"] = "Apply";
            var control = sender as Control;
            if (control != null)
            {
                var gvRow = (GridViewRow) control.Parent.Parent;
                var dataKey = gvManifestTemplates.DataKeys[gvRow.RowIndex];
                if (dataKey != null)
                {
                    Session["manifestTemplateId"] = Convert.ToInt32(dataKey.Value);
                    var confirmStats =
                        Call.MunkiManifestTemplateApi.GetUpdateStats(Convert.ToInt32(dataKey.Value));

                    lblTitle.Text =
                        "Are You Sure?<br>";
                    lblSubTitle.Text = " The Manifest For " + confirmStats.computerCount +
                                       " Computers Will Be Updated.  Applying This Template Will Include Changes From the Following Templates. ";

                    foreach (var munkiTemplate in confirmStats.manifestTemplates)
                    {
                        lblSubTitle.Text += munkiTemplate.Name + " ";
                    }

                    ClientScript.RegisterStartupScript(GetType(), "modalscript",
                        "$(function() {  var menuTop = document.getElementById('confirmbox'),body = document.body;classie.toggle(menuTop, 'confirm-box-outer-open'); });",
                        true);
                }
            }
        }

        protected void btnPreview_OnClick(object sender, EventArgs e)
        {
            var control = sender as Control;
            if (control != null)
            {
                var gvRow = (GridViewRow) control.Parent.Parent;
                var dataKey = gvManifestTemplates.DataKeys[gvRow.RowIndex];
                if (dataKey != null)
                {
                    var effectiveManifest =
                        Call.MunkiManifestTemplateApi.GetEffectiveManifest(Convert.ToInt32(dataKey.Value));
                    Response.Write(effectiveManifest);
                    Response.ContentType = "text/plain";
                    Response.End();
                }
            }
        }

      

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            ChkAll(gvManifestTemplates);
        }

        protected void ConfirmButton_OnClick(object sender, EventArgs e)
        {
            if ((string) Session["manifestBtnClick"] == "Apply")
            {
                Session.Remove("manifestBtnClick");
                var manifestTemplateId = (int)Session["manifestTemplateId"];
                Session.Remove("manifestTemplateId");
                var failedCount = Call.MunkiManifestTemplateApi.Apply(manifestTemplateId);

                PopulateGrid();
                if (failedCount > 0)
                    EndUserMessage = "Failed To Update " + failedCount + "Manifests";
                else

                    EndUserMessage = "Successfully Updated Manifests";
            }
            else
            {
                
                RequiresAuthorization(AuthorizationStrings.DeleteGlobal);
                foreach (GridViewRow row in gvManifestTemplates.Rows)
                {
                    var cb = (CheckBox)row.FindControl("chkSelector");
                    if (cb == null || !cb.Checked) continue;
                    var dataKey = gvManifestTemplates.DataKeys[row.RowIndex];
                    if (dataKey == null) continue;
                    Call.MunkiManifestTemplateApi.Delete(Convert.ToInt32(dataKey.Value));
                }

                PopulateGrid();
            }
           
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
            gvManifestTemplates.DataSource = Call.MunkiManifestTemplateApi.Get(int.MaxValue, txtSearch.Text);
            gvManifestTemplates.DataBind();

            lblTotal.Text = gvManifestTemplates.Rows.Count + " Result(s) / " + Call.MunkiManifestTemplateApi.GetCount() +
                            " Total Manifest Template(s)";

            foreach (GridViewRow row in gvManifestTemplates.Rows)
            {
                var dataKey = gvManifestTemplates.DataKeys[row.RowIndex];
                if (dataKey == null) continue;
                var lblApplied = (Label) row.FindControl("lblApplied");
                var manifestTemplate = Call.MunkiManifestTemplateApi.Get(Convert.ToInt32(dataKey.Value));
                lblApplied.Text = manifestTemplate.ChangesApplied == 0 ? "No" : "Yes";
            }
        }

        protected void search_Changed(object sender, EventArgs e)
        {
            PopulateGrid();
        }
    }
}