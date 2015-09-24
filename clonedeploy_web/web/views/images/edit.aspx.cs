/*  
    CrucibleWDS A Windows Deployment Solution
    Copyright (C) 2011  Jon Dolny

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/.
 */

using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Global;
using Image = Models.Image;

namespace views.images
{
    public partial class ImageEdit : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) PopulateForm();
        }

        protected void btnFixImage_Click(object sender, EventArgs e)
        {
            var needsFixed = false;
            var currentName = (string) (ViewState["currentName"]);

            if ((string) (ViewState["storeCheck"]) == "false")
            {
                needsFixed = true;
                try
                {
                    Directory.CreateDirectory(Settings.ImageStorePath + currentName);
                    Utility.Message = "Successfully Created Directory In Image Store Path. ";
                }
                catch (Exception ex)
                {
                    Logger.Log("Could Not Create Directory In Image Store Path. " + ex.Message);
                    Utility.Message =
                        "Could Not Create Directory In Image Store Path.  Check The Exception Log For More Info. ";
                }
            }
            if ((string) (ViewState["holdCheck"]) == "false")
            {
                needsFixed = true;
                try
                {
                    Directory.CreateDirectory(Settings.ImageHoldPath + currentName);
                    Utility.Message += "Successfully Created Directory In Image Hold Path. ";
                }
                catch (Exception ex)
                {
                    Logger.Log("Could Not Create Directory In Image Hold Path. " + ex.Message);
                    Utility.Message +=
                        "Could Not Create Directory In Image Hold Path.  Check The Exception Log For More Info. ";
                }
            }

            if ((string) (ViewState["holdCheck"]) == "true" && (string) (ViewState["storeCheck"]) == "true")
            {
                if ((string) (ViewState["storeEmpty"]) == "true" && (string) (ViewState["holdEmpty"]) == "false")
                {
                    needsFixed = true;
                    try
                    {
                        new FileOps().MoveFolder(Settings.ImageHoldPath + currentName,
                            Settings.ImageStorePath + currentName);

                        try
                        {
                            Directory.CreateDirectory(Settings.ImageHoldPath + currentName);
                            // for next upload
                            Utility.Message = "Successfully Moved Image From Hold To Store";
                        }
                        catch (Exception ex)
                        {
                            Logger.Log("Could Not Recreate Directory " + ex.Message);
                            Utility.Message =
                                "Could Not Recreate Directory,  You Must Create It Before You Can Upload Again";
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Log("Could Not Move Image From Hold Path To Store Path " + ex.Message);
                        Utility.Message =
                            "Could Not Move Image From Hold Path To Store Path.  Check The Exception Log For More Info.";
                    }
                }
            }

            if (!needsFixed)
                Utility.Message = "No Fixes Are Needed For This Image";
    }

        protected void btnUpdateImage_Click(object sender, EventArgs e)
        {
            var image = Master.Image;

            var currentName = (string) (ViewState["currentName"]);
            image.Name = txtImageName.Text;
            image.Os = "";
            image.Description = txtImageDesc.Text;
            image.Protected = chkProtected.Checked ? 1 : 0;
            image.IsVisible = chkVisible.Checked ? 1 : 0;

            var bllImage = new BLL.Image();
            
            if (bllImage.ValidateImageData(image))
                bllImage.UpdateImage(image, currentName);
        }

        protected void PopulateForm()
        {
            ViewState["currentName"] = Master.Image.Name;
            var currentName = (string) (ViewState["currentName"]);

            txtImageName.Text = Master.Image.Name;
            txtImageDesc.Text = Master.Image.Description;
            ddlImageOS.Text = Master.Image.Os;
            ddlImageType.Text = Master.Image.Type;
            if (Master.Image.Protected == 1)
                chkProtected.Checked = true;
            if (Master.Image.IsVisible == 1)
                chkVisible.Checked = true;

            try
            {
                if (Directory.Exists(Settings.ImageHoldPath + currentName))
                {
                    lblImageHold.Text = currentName + " Exists In Image Hold Path : Pass";
                    ViewState["holdCheck"] = "true";
                }
                else
                {
                    lblImageHold.Text = currentName + " Exists In Image Hold Path : Fail";
                    ViewState["holdCheck"] = "false";
                }
            }
            catch (Exception ex)
            {
                lblImageHold.Text = currentName + " Exists In Image Hold Path : Error " + ex.Message;
                ViewState["holdCheck"] = "false";
            }

            try
            {
                if (Directory.Exists(Settings.ImageStorePath + currentName))
                {
                    lblImage.Text = currentName + " Exists In Image Store Path : Pass";
                    ViewState["storeCheck"] = "true";
                }
                else
                {
                    lblImage.Text = currentName + " Exists In Image Store Path : Fail";
                    ViewState["storeCheck"] = "false";
                }
            }
            catch (Exception ex)
            {
                lblImage.Text = currentName + " Exists In Image Store Path : Error " + ex.Message;
                ViewState["storeCheck"] = "false";
            }

            try
            {
                if (Directory.GetFiles(Settings.ImageHoldPath + currentName).Length > 0)
                {
                    lblImageHoldStatus.Text = currentName + " Image Hold Is Not Empty." +
                                              "This Is Normal If You Are Currently Uploading This Image.";
                    ViewState["holdEmpty"] = "false";
                }
                else
                {
                    lblImageHoldStatus.Text = currentName + " Image Hold Is Empty";
                    ViewState["holdEmpty"] = "true";
                }
            }
            catch (Exception ex)
            {
                lblImageHoldStatus.Text = "Could Not Determine If " + currentName + " Image Hold Is Empty " + ex.Message;
                ViewState["holdEmpty"] = "true";
            }

            try
            {
                if (Directory.GetFiles(Settings.ImageStorePath + currentName).Length > 0)
                {
                    lblImageStatus.Text = currentName + " Image Store Is Not Empty.";
                    ViewState["storeEmpty"] = "false";
                }
                else
                {
                    lblImageStatus.Text = currentName + " Image Store Is Empty";
                    ViewState["storeEmpty"] = "true";
                }
            }
            catch (Exception ex)
            {
                lblImageStatus.Text = "Could Not Determine If " + currentName + " Image Hold Is Empty " + ex.Message;
                ViewState["storeEmpty"] = "false";
            }
        }
    }
}