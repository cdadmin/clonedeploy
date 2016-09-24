using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Helpers;

public partial class views_admin_clobber : BasePages.Admin
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        PopulateForm();
    }

    protected void PopulateForm()
    {
        ddlComputerImage.DataSource = BLL.Image.SearchImagesForUser(CloneDeployCurrentUser.Id).Where(x => x.Environment == "linux" || x.Environment == "").Select(i => new { i.Id, i.Name }).OrderBy(x => x.Name).ToList();
        ddlComputerImage.DataValueField = "Id";
        ddlComputerImage.DataTextField = "Name";
        ddlComputerImage.DataBind();
        ddlComputerImage.Items.Insert(0, new ListItem("Select Image", "-1"));
       
        if (Settings.ClobberEnabled == "1")
            chkClobber.Checked = true;
        if (Settings.ClobberPromptComputerName == "1")
            chkPromptName.Checked = true;

        try
        {
            var imageProfile = BLL.ImageProfile.ReadProfile(Convert.ToInt32(Settings.ClobberProfileId));
            ddlComputerImage.SelectedValue = imageProfile.Image.Id.ToString();
            PopulateImageProfilesDdl(ddlImageProfile, Convert.ToInt32(ddlComputerImage.SelectedValue));
            ddlImageProfile.SelectedValue = imageProfile.Id.ToString();
        }
        catch (Exception)
        {
            
            //ignore
        }
     
    }

    protected void btnUpdateSettings_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.UpdateAdmin);

        List<Models.Setting> listSettings = new List<Models.Setting>
        {
            new Models.Setting
            {
                Name = "Clobber Enabled",
                Value = chkClobber.Checked ? "1" : "0",
                Id = Setting.GetSetting("Clobber Enabled").Id
            },
            new Models.Setting
            {
                Name = "Clobber Prompt Computer Name",
                Value = chkPromptName.Checked ? "1" : "0",
                Id = Setting.GetSetting("Clobber Prompt Computer Name").Id
            },
            new Models.Setting
            {
                Name = "Clobber ProfileId",
                Value = ddlImageProfile.SelectedValue,
                Id = Setting.GetSetting("Clobber ProfileId").Id
            },
        };

        var result = Setting.UpdateSetting(listSettings);
        if (result)
        {
            if (Settings.ClobberEnabled == "1")
            {
                var imageProfile = BLL.ImageProfile.ReadProfile(Convert.ToInt32(Settings.ClobberProfileId));
                bool promptForName = Settings.ClobberPromptComputerName == "1";

                var bootMenuResult = new BLL.Workflows.ClobberBootMenu(imageProfile, promptForName).CreatePxeBootFiles();
                if (bootMenuResult)
                    EndUserMessage = "Successfully Enabled Clobber Mode";
            }
            else
            {
                var proxyDhcp = Settings.ProxyDhcp;
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

    protected void CreateProxyMenu()
    {
        var defaultBootMenu = new BLL.Workflows.DefaultBootMenu();
        defaultBootMenu.Kernel = "4.5";
        defaultBootMenu.BootImage = "initrd.xz";
        defaultBootMenu.Type = "bios";
        defaultBootMenu.CreateGlobalDefaultBootMenu();

        defaultBootMenu.Kernel = "4.5";
        defaultBootMenu.BootImage = "initrd.xz";
        defaultBootMenu.Type = "efi32";
        defaultBootMenu.CreateGlobalDefaultBootMenu();

        defaultBootMenu.Kernel = "4.5x64";
        defaultBootMenu.BootImage = "initrd.xz";
        defaultBootMenu.Type = "efi64";
        defaultBootMenu.CreateGlobalDefaultBootMenu();
    }

    protected void CreateStandardMenu()
    {
        var defaultBootMenu = new BLL.Workflows.DefaultBootMenu();
        defaultBootMenu.Kernel = "4.5x64";
        defaultBootMenu.BootImage = "initrd.xz";
        defaultBootMenu.Type = "standard";
        defaultBootMenu.CreateGlobalDefaultBootMenu();


    }

   
}