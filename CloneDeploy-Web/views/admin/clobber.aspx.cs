﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.admin
{
    public partial class views_admin_clobber : Admin
    {
        protected void ButtonConfirm_Click(object sender, EventArgs e)
        {
            RequiresAuthorization(AuthorizationStrings.UpdateAdmin);

            var listSettings = new List<SettingEntity>
            {
                new SettingEntity
                {
                    Name = "Clobber Enabled",
                    Value = chkClobber.Checked ? "1" : "0",
                    Id = Call.SettingApi.GetSetting("Clobber Enabled").Id
                },
                new SettingEntity
                {
                    Name = "Clobber Prompt Computer Name",
                    Value = chkPromptName.Checked ? "1" : "0",
                    Id = Call.SettingApi.GetSetting("Clobber Prompt Computer Name").Id
                },
                new SettingEntity
                {
                    Name = "Clobber ProfileId",
                    Value = ddlImageProfile.SelectedValue,
                    Id = Call.SettingApi.GetSetting("Clobber ProfileId").Id
                }
            };

            var result = Call.SettingApi.UpdateSettings(listSettings);
            if (result)
            {
                if (GetSetting(SettingStrings.ClobberEnabled) == "1")
                {
                    var imageProfile =
                        Call.ImageProfileApi.Get(Convert.ToInt32(GetSetting(SettingStrings.ClobberProfileId)));
                    var promptForName = GetSetting(SettingStrings.ClobberPromptComputerName) == "1";

                    var bootMenuResult = Call.WorkflowApi.CreateClobberBootMenu(imageProfile.Id, promptForName);
                    if (bootMenuResult)
                        EndUserMessage = "Successfully Enabled Clobber Mode";
                }
                else
                {
                    var proxyDhcp = GetSetting(SettingStrings.ProxyDhcp);
                    if (proxyDhcp == "Yes")
                    {
                        CreateProxyMenu();
                    }
                    else
                    {
                        CreateStandardMenu();
                    }
                    EndUserMessage = "Successfully Disabled Clobber Mode";
                }
            }
            else
            {
                EndUserMessage = "Could Not Update Settings";
            }
        }

        protected void CreateProxyMenu()
        {
            var defaultBootMenuOptions = new BootMenuGenOptionsDTO();
            defaultBootMenuOptions.Kernel = SettingStrings.DefaultKernel64;
            defaultBootMenuOptions.BootImage = "initrd.xz";
            defaultBootMenuOptions.Type = "bios";
            Call.WorkflowApi.CreateDefaultBootMenu(defaultBootMenuOptions);

            defaultBootMenuOptions.Kernel = SettingStrings.DefaultKernel32;
            defaultBootMenuOptions.BootImage = "initrd.xz";
            defaultBootMenuOptions.Type = "efi32";
            Call.WorkflowApi.CreateDefaultBootMenu(defaultBootMenuOptions);

            defaultBootMenuOptions.Kernel = SettingStrings.DefaultKernel64;
            defaultBootMenuOptions.BootImage = "initrd.xz";
            defaultBootMenuOptions.Type = "efi64";
            Call.WorkflowApi.CreateDefaultBootMenu(defaultBootMenuOptions);
        }

        protected void CreateStandardMenu()
        {
            var defaultBootMenuOptions = new BootMenuGenOptionsDTO();
            defaultBootMenuOptions.Kernel = SettingStrings.DefaultKernel64;
            defaultBootMenuOptions.BootImage = "initrd.xz";
            defaultBootMenuOptions.Type = "standard";
            Call.WorkflowApi.CreateDefaultBootMenu(defaultBootMenuOptions);
        }

        protected void ddlComputerImage_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateImageProfilesDdl(ddlImageProfile, Convert.ToInt32(ddlComputerImage.SelectedValue));
            try
            {
                ddlImageProfile.SelectedIndex = 1;
            }
            catch
            {
                //ignore
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                PopulateForm();
        }

        protected void PopulateForm()
        {
            ddlComputerImage.DataSource =
                Call.ImageApi.Get(int.MaxValue, "")
                    .Where(x => x.Environment == "linux" || x.Environment == "")
                    .Select(i => new {i.Id, i.Name})
                    .OrderBy(x => x.Name)
                    .ToList();
            ddlComputerImage.DataValueField = "Id";
            ddlComputerImage.DataTextField = "Name";
            ddlComputerImage.DataBind();
            ddlComputerImage.Items.Insert(0, new ListItem("Select Image", "-1"));

            if (GetSetting(SettingStrings.ClobberEnabled) == "1")
                chkClobber.Checked = true;
            if (GetSetting(SettingStrings.ClobberPromptComputerName) == "1")
                chkPromptName.Checked = true;

            try
            {
                var imageProfile = Call.ImageProfileApi.Get(Convert.ToInt32(GetSetting(SettingStrings.ClobberProfileId)));
                ddlComputerImage.SelectedValue = imageProfile.Image.Id.ToString();
                PopulateImageProfilesDdl(ddlImageProfile, Convert.ToInt32(ddlComputerImage.SelectedValue));
                ddlImageProfile.SelectedValue = imageProfile.Id.ToString();
            }
            catch (Exception)
            {
                //ignore
            }
        }
    }
}