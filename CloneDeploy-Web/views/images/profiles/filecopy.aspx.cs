﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.images.profiles
{
    public partial class views_images_profiles_filecopy : Images
    {
        protected void btnUpdateFile_OnClick(object sender, EventArgs e)
        {
            RequiresAuthorizationOrManagedImage(AuthorizationStrings.UpdateProfile, Image.Id);
            var deleteResult = Call.ImageProfileApi.RemoveProfileFileFolders(ImageProfile.Id);
            var checkedCount = 0;
            foreach (GridViewRow row in gvFile.Rows)
            {
                var enabled = (CheckBox) row.FindControl("chkEnabled");
                if (enabled == null) continue;
                if (!enabled.Checked) continue;
                checkedCount++;
                var dataKey = gvFile.DataKeys[row.RowIndex];
                if (dataKey == null) continue;

                var profileFileFolder = new ImageProfileFileFolderEntity
                {
                    FileFolderId = Convert.ToInt32(dataKey.Value),
                    ProfileId = ImageProfile.Id
                };
                var txtPriority = row.FindControl("txtPriority") as TextBox;
                if (txtPriority != null)
                    if (!string.IsNullOrEmpty(txtPriority.Text))
                        profileFileFolder.Priority = Convert.ToInt32(txtPriority.Text);
                var txtPartition = row.FindControl("txtPartition") as TextBox;
                if (txtPartition != null)
                    profileFileFolder.DestinationPartition = txtPartition.Text;
                var txtPath = row.FindControl("txtPath") as TextBox;
                if (txtPath != null)
                    profileFileFolder.DestinationFolder = txtPath.Text;
                var ddlFolderMode = row.FindControl("ddlFolderMode") as DropDownList;
                if (ddlFolderMode != null)
                    profileFileFolder.FolderCopyType = ddlFolderMode.Text;
                EndUserMessage = Call.ImageProfileFileFolderApi.Post(profileFileFolder).Success
                    ? "Successfully Updated Image Profile"
                    : "Could Not Update Image Profile";
            }
            if (checkedCount == 0)
            {
                EndUserMessage = deleteResult.Success
                    ? "Successfully Updated Image Profile"
                    : "Could Not Update Image Profile";
            }
        }

        protected void gridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            PopulateGrid();
            var listFilesFolders = (List<FileFolderEntity>) gvFile.DataSource;
            switch (e.SortExpression)
            {
                case "Name":
                    listFilesFolders = GetSortDirection(e.SortExpression) == "Asc"
                        ? listFilesFolders.OrderBy(s => s.Name).ToList()
                        : listFilesFolders.OrderByDescending(s => s.Name).ToList();
                    break;
            }

            gvFile.DataSource = listFilesFolders;
            gvFile.DataBind();
            PopulateProfileScripts();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            PopulateGrid();
        }

        protected void PopulateGrid()
        {
            gvFile.DataSource = Call.FileFolderApi.Get(int.MaxValue, "");
            gvFile.DataBind();
           
            PopulateProfileScripts();
        }

        protected void PopulateProfileScripts()
        {
            var profileFiles = Call.ImageProfileApi.GetFileFolders(ImageProfile.Id);
            foreach (GridViewRow row in gvFile.Rows)
            {
                var enabled = (CheckBox) row.FindControl("chkEnabled");
                var dataKey = gvFile.DataKeys[row.RowIndex];
                if (dataKey == null) continue;
                foreach (var profileFile in profileFiles)
                {
                    if (profileFile.FileFolderId == Convert.ToInt32(dataKey.Value))
                    {
                        enabled.Checked = true;
                        var txtPriority = row.FindControl("txtPriority") as TextBox;
                        if (txtPriority != null)
                            txtPriority.Text = profileFile.Priority.ToString();
                        var txtPartition = row.FindControl("txtPartition") as TextBox;
                        if (txtPartition != null)
                            txtPartition.Text = profileFile.DestinationPartition;
                        var txtPath = row.FindControl("txtPath") as TextBox;
                        if (txtPath != null)
                            txtPath.Text = profileFile.DestinationFolder;
                        var ddlFolderMode = row.FindControl("ddlFolderMode") as DropDownList;
                        if (ddlFolderMode != null)
                            ddlFolderMode.Text = profileFile.FolderCopyType;
                    }
                }
            }
        }
    }
}