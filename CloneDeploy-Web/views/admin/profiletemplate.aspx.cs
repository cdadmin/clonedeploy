using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CloneDeploy_Common;
using CloneDeploy_Common.Enum;
using CloneDeploy_Entities;

namespace CloneDeploy_Web.views.admin
{
    public partial class profiletemplate : BasePages.Admin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlImageType.DataSource = Enum.GetNames(typeof(EnumProfileTemplate.TemplateType));
                ddlImageType.DataBind();
                DisplayForm();
            }
        }

        protected void btnUpdateSettings_OnClick(object sender, EventArgs e)
        {
            var template = new ImageProfileTemplate();
            template.TemplateType = (EnumProfileTemplate.TemplateType)
                Enum.Parse(typeof (EnumProfileTemplate.TemplateType), ddlImageType.SelectedValue);
            template.Name = txtProfileName.Text;
            template.Description = txtProfileDesc.Text;



            template.Kernel = ddlKernel.Text;
            template.BootImage = ddlBootImage.Text;
            template.WebCancel = Convert.ToInt16(chkWebCancel.Checked); 
            template.TaskCompletedAction = ddlTaskComplete.Text;
            template.OsxTargetVolume = txtTargetVolume.Text;
            template.ChangeName = Convert.ToInt16(chkChangeName.Checked);
            template.SkipExpandVolumes = Convert.ToInt16(chkDownNoExpand.Checked);
            template.FixBcd = Convert.ToInt16(chkAlignBCD.Checked);
            template.RandomizeGuids = Convert.ToInt16(chkRandomize.Checked);
            template.FixBootloader = Convert.ToInt16(chkRunFixBoot.Checked);
            template.SkipNvramUpdate = Convert.ToInt16(chkNvram.Checked);
            template.ErasePartitions = Convert.ToInt16(chkErase.Checked);

            if (ddlImageType.Text == EnumProfileTemplate.TemplateType.LinuxBlock.ToString() || ddlImageType.Text == EnumProfileTemplate.TemplateType.LinuxFile.ToString())
                template.PartitionMethod = ddlPartitionMethodLin.Text;
            else if (ddlImageType.Text == EnumProfileTemplate.TemplateType.MacOS.ToString())
                template.PartitionMethod = ddlPartitionMethodMac.Text;
            else if (ddlImageType.Text == EnumProfileTemplate.TemplateType.WinPE.ToString())
                template.PartitionMethod = ddlPartitionMethodWin.Text;

            template.ForceStandardEfi = Convert.ToInt16(chkForceEfi.Checked);
            template.ForceStandardLegacy = Convert.ToInt16(chkForceLegacy.Checked);
            template.ForceDynamicPartitions = Convert.ToInt16(chkDownForceDynamic.Checked);
            template.RemoveGPT = Convert.ToInt16(chkRemoveGpt.Checked);
            template.SkipShrinkVolumes = Convert.ToInt16(chkUpNoShrink.Checked);
            template.SkipShrinkLvm = Convert.ToInt16(chkUpNoShrinkLVM.Checked);
            template.Compression = ddlCompAlg.Text;
            template.CompressionLevel = ddlCompLevel.Text;
            template.WimMulticastEnabled = Convert.ToInt16(chkWimMulticast.Checked);
            template.SimpleUploadSchema = Convert.ToInt16(chkSimpleSchema.Checked);
            template.UploadSchemaOnly = Convert.ToInt16(chkSchemaOnly.Checked);
            template.SenderArguments = txtSender.Text;
            template.ReceiverArguments = txtReceiver.Text;

            var result = Call.ImageProfileTemplateApi.Put(template);
            if (result.Success) EndUserMessage = "Successfully Updated " + template.TemplateType;
            else
            {
                EndUserMessage = "Could Not Update " + template.TemplateType;
            }
        }

        protected void chkForceDynamic_OnCheckedChanged(object sender, EventArgs e)
        {
            if (chkDownForceDynamic.Checked)
            {
                ddlPartitionMethodLin.Enabled = false;
                ddlPartitionMethodLin.Text = "Dynamic";
            }
            else
            {
                ddlPartitionMethodLin.Enabled = true;
            }
        }

        private void DisplayForm()
        {
            var template = Call.ImageProfileTemplateApi.Get((EnumProfileTemplate.TemplateType)
                    Enum.Parse(typeof (EnumProfileTemplate.TemplateType), ddlImageType.SelectedValue));

            txtProfileName.Text = template.Name;
            txtProfileDesc.Text = template.Description;

            ddlKernel.DataSource = Call.FilesystemApi.GetKernels();
            ddlBootImage.DataSource = Call.FilesystemApi.GetBootImages();
            ddlKernel.DataBind();
            ddlBootImage.DataBind();

            try
            {
                ddlKernel.Text = template.Kernel;
            }
            catch
            {
                //ignored
            }

            try
            {
                 ddlBootImage.Text = template.BootImage;
            }
            catch
            {
                //ignored
            }


            chkWebCancel.Checked = Convert.ToBoolean(template.WebCancel);
            ddlTaskComplete.Text = template.TaskCompletedAction;
            txtTargetVolume.Text = template.OsxTargetVolume;
            chkChangeName.Checked = Convert.ToBoolean(template.ChangeName);
            chkDownNoExpand.Checked = Convert.ToBoolean(template.SkipExpandVolumes);
            chkAlignBCD.Checked = Convert.ToBoolean(template.FixBcd);
            chkRandomize.Checked = Convert.ToBoolean(template.RandomizeGuids);
            chkRunFixBoot.Checked = Convert.ToBoolean(template.FixBootloader);
            chkNvram.Checked = Convert.ToBoolean(template.SkipNvramUpdate);
            chkErase.Checked = Convert.ToBoolean(template.ErasePartitions);

            if (ddlImageType.Text == EnumProfileTemplate.TemplateType.LinuxBlock.ToString() || ddlImageType.Text == EnumProfileTemplate.TemplateType.LinuxFile.ToString())
            ddlPartitionMethodLin.Text = template.PartitionMethod;
            else if (ddlImageType.Text == EnumProfileTemplate.TemplateType.MacOS.ToString())
                ddlPartitionMethodMac.Text = template.PartitionMethod;
            else if (ddlImageType.Text == EnumProfileTemplate.TemplateType.WinPE.ToString())
                ddlPartitionMethodWin.Text = template.PartitionMethod;

            chkForceEfi.Checked = Convert.ToBoolean(template.ForceStandardEfi);
            chkForceLegacy.Checked = Convert.ToBoolean(template.ForceStandardLegacy);
            chkDownForceDynamic.Checked = Convert.ToBoolean(template.ForceDynamicPartitions);
            chkRemoveGpt.Checked = Convert.ToBoolean(template.RemoveGPT);
            chkUpNoShrink.Checked = Convert.ToBoolean(template.SkipShrinkVolumes);
            chkUpNoShrinkLVM.Checked = Convert.ToBoolean(template.SkipShrinkLvm);
            ddlCompAlg.Text = template.Compression;
            ddlCompLevel.Text = template.CompressionLevel;
            chkWimMulticast.Checked = Convert.ToBoolean(template.WimMulticastEnabled);
            chkSimpleSchema.Checked = Convert.ToBoolean(template.SimpleUploadSchema);
            chkSchemaOnly.Checked = Convert.ToBoolean(template.UploadSchemaOnly);
            txtSender.Text = template.SenderArguments;
            txtReceiver.Text = template.ReceiverArguments;


            LinuxAll1.Visible = false;
            LinuxAll2.Visible = false;
            LinuxAll3.Visible = false;
            LinuxAll4.Visible = false;
            LinuxAll5.Visible = false;
            LinuxAll6.Visible = false;
            LinuxBlock1.Visible = false;
            LinuxBlock2.Visible = false;
            LinuxFileWinpe1.Visible = false;
            mac1.Visible = false;
            mac2.Visible = false;
            mac3.Visible = false;
            mac4.Visible = false;
            winpe1.Visible = false;

            if (ddlImageType.Text == EnumProfileTemplate.TemplateType.LinuxBlock.ToString())
            {
                LinuxAll1.Visible = true;
                LinuxAll2.Visible = true;
                LinuxAll3.Visible = true;
                LinuxAll4.Visible = true;
                LinuxAll5.Visible = true;
                LinuxAll6.Visible = true;
                LinuxBlock1.Visible = true;
                LinuxBlock2.Visible = true;
            }
            else if (ddlImageType.Text == EnumProfileTemplate.TemplateType.LinuxFile.ToString())
            {
                LinuxAll1.Visible = true;
                LinuxAll2.Visible = true;
                LinuxAll3.Visible = true;
                LinuxAll4.Visible = true;
                LinuxAll5.Visible = true;
                LinuxAll6.Visible = true;
                LinuxFileWinpe1.Visible = true;
            }
            else if (ddlImageType.Text == EnumProfileTemplate.TemplateType.MacOS.ToString())
            {
                mac1.Visible = true;
                mac2.Visible = true;
                mac3.Visible = true;
                mac4.Visible = true;
            }
            else if (ddlImageType.Text == EnumProfileTemplate.TemplateType.WinPE.ToString())
            {
                LinuxFileWinpe1.Visible = true;
                winpe1.Visible = true;
            }
        }

        protected void ddlImageType_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayForm();
        }
    }
}