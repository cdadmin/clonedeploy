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
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using Global;
using Models;
using Pxe;
using Security;

namespace views.admin
{
    public partial class AdminSettings : Page
    {
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            txtServerKey.Text = Utility.GenerateKey();
        }

        protected void btnTestMessage_Click(object sender, EventArgs e)
        {
            var mail = new Mail
            {
                Subject = "Test Message",
                Body = HttpContext.Current.User.Identity.Name
            };
            mail.Send("Test Message");

        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!ValidateSettings())
                Master.Master.Msgbox(Utility.Message);
            else
            {
                List<Setting> listSettings = new List<Setting>
                {
                    new Setting {Name = "Server IP", Value = txtIP.Text},
                    new Setting {Name = "Web Server Port", Value = txtPort.Text},
                    new Setting {Name = "Image Store Path", Value = txtImagePath.Text},
                    new Setting {Name = "Queue Size", Value = txtQSize.Text},
                    new Setting {Name = "Sender Args", Value = txtSenderArgs.Text},
                    new Setting {Name = "Nfs Upload Path", Value = txtNFSPath.Text},
                    new Setting {Name = "Tftp Path", Value = txtTFTPPath.Text},
                    new Setting {Name = "Web Path", Value = txtWebService.Text},
                    new Setting {Name = "PXE Mode", Value = ddlPXEMode.Text},
                    new Setting {Name = "Proxy Dhcp", Value = ddlProxyDHCP.Text},
                    new Setting {Name = "Proxy Bios File", Value = ddlProxyBios.Text},
                    new Setting {Name = "Proxy Efi32 File", Value = ddlProxyEfi32.Text},
                    new Setting {Name = "Proxy Efi64 File", Value = ddlProxyEfi64.Text},
                    new Setting {Name = "Compression Algorithm", Value = ddlCompAlg.Text},
                    new Setting {Name = "Compression Level", Value = ddlCompLevel.Text},
                    new Setting {Name = "AD Login Domain", Value = txtADLogin.Text},
                    new Setting {Name = "Image Transfer Mode", Value = ddlImageXfer.Text},
                    new Setting {Name = "Image Checksum", Value = ddlImageChecksum.Text},
                    new Setting {Name = "Default Host View", Value = ddlHostView.Text},
                    new Setting {Name = "On Demand", Value = ddlOnd.Text},
                    new Setting {Name = "Receiver Args", Value = txtRecArgs.Text},
                    new Setting {Name = "Udpcast Start Port", Value = txtStartPort.Text},
                    new Setting {Name = "Udpcast End Port", Value = txtEndPort.Text},
                    new Setting {Name = "Server Key", Value = txtServerKey.Text},
                    new Setting {Name = "Image Hold Path", Value = txtImageHoldPath.Text},
                    new Setting {Name = "Nfs Deploy Path", Value = txtNFSDeploy.Text},
                    new Setting {Name = "Force SSL", Value = ddlSSL.Text},
                    new Setting {Name = "SMB Path", Value = txtSMBPath.Text},
                    new Setting {Name = "SMB User Name", Value = txtSMBUser.Text},
                    new Setting {Name = "Client Receiver Args", Value = txtRecClientArgs.Text},
                    new Setting {Name = "Global Host Args", Value = txtGlobalHostArgs.Text},
                    new Setting {Name = "Web Task Requires Login", Value = ddlWebTasksLogin.Text},
                    new Setting {Name = "Smtp Server", Value = txtSmtpServer.Text},
                    new Setting {Name = "Smtp Port", Value = txtSmtpPort.Text},
                    new Setting {Name = "Smtp Username", Value = txtSmtpUsername.Text},
                    new Setting {Name = "Smtp Mail From", Value = txtSmtpFrom.Text},
                    new Setting {Name = "Smtp Mail To", Value = txtSmtpTo.Text},
                    new Setting {Name = "Smtp Ssl", Value = ddlSmtpSsl.Text}
                };


                if (!string.IsNullOrEmpty(txtSmtpPassword.Text))
                    listSettings.Add(new Setting {Name = "Smtp Password", Value = txtSmtpPassword.Text});

                if (!string.IsNullOrEmpty(txtSMBPass.Text))
                    listSettings.Add(new Setting {Name = "SMB Password", Value = txtSMBPass.Text});

                var chkValue = "0";
                chkValue = chkLoginSuccess.Checked ? "1" : "0";
                listSettings.Add(new Setting {Name = "Notify Successful Login", Value = chkValue});

                chkValue = chkLoginFailed.Checked ? "1" : "0";
                listSettings.Add(new Setting {Name = "Notify Failed Login", Value = chkValue});

                chkValue = chkTaskStarted.Checked ? "1" : "0";
                listSettings.Add(new Setting {Name = "Notify Task Started", Value = chkValue});

                chkValue = chkTaskCompleted.Checked ? "1" : "0";
                listSettings.Add(new Setting {Name = "Notify Task Completed", Value = chkValue});

                chkValue = chkImageApproved.Checked ? "1" : "0";
                listSettings.Add(new Setting {Name = "Notify Image Approved", Value = chkValue});

                chkValue = chkResizeFailed.Checked ? "1" : "0";
                listSettings.Add(new Setting {Name = "Notify Resize Failed", Value = chkValue});


                if (ddlWebTasksLogin.Text == "Yes")
                {
                    listSettings.Add(new Setting {Name = "On Demand Requires Login", Value = "Yes"});
                    listSettings.Add(new Setting {Name = "Debug Requires Login", Value = "Yes"});
                    listSettings.Add(new Setting {Name = "Register Requires Login", Value = "Yes"});
                }
                else
                {
                    listSettings.Add(new Setting {Name = "On Demand Requires Login", Value = ddlOndLogin.Text});
                    listSettings.Add(new Setting {Name = "Debug Requires Login", Value = ddlDebugLogin.Text});
                    listSettings.Add(new Setting {Name = "Register Requires Login", Value = ddlRegisterLogin.Text});
                }


                var newBootMenu = false;
                var newClientIso = false;
                if (new Setting().Update(listSettings))
                {
                    if ((string) ViewState["webTaskRequiresLogin"] != ddlWebTasksLogin.Text)
                        newBootMenu = true;
                    if ((string) ViewState["proxyDHCP"] != ddlProxyDHCP.Text)
                        newBootMenu = true;
                    if ((string) ViewState["proxyBios"] != ddlProxyBios.Text)
                        newBootMenu = true;
                    if ((string) ViewState["proxyEfi32"] != ddlProxyEfi32.Text)
                        newBootMenu = true;
                    if ((string) ViewState["proxyEfi64"] != ddlProxyEfi64.Text)
                        newBootMenu = true;
                    if ((string) ViewState["serverIP"] != txtIP.Text)
                    {
                        newBootMenu = true;
                        newClientIso = true;
                    }
                    if ((string) ViewState["serverPort"] != txtPort.Text)
                    {
                        newBootMenu = true;
                        newClientIso = true;
                    }
                    if ((string) ViewState["serverKey"] != txtServerKey.Text)
                    {
                        newBootMenu = true;
                        newClientIso = true;
                    }

                    if ((string) ViewState["webService"] != txtWebService.Text)
                    {
                        newBootMenu = true;
                        newClientIso = true;
                    }
                    if ((string) ViewState["pxeMode"] != ddlPXEMode.Text)
                    {
                        newBootMenu = true;
                    }

                    if ((string) ViewState["forceSSL"] != ddlSSL.Text)
                    {
                        newBootMenu = true;
                        newClientIso = true;
                    }
                    if ((string) (ViewState["startPort"]) != txtStartPort.Text)
                    {
                        var startPort = Convert.ToInt32(txtStartPort.Text);
                        startPort = startPort - 2;
                        var port = new Port {Number = startPort};
                        port.Create();
                    }
                }
                new PxeFileOps().CopyPxeFiles(ddlPXEMode.Text);
                if (!newBootMenu)
                    Master.Master.Msgbox(Utility.Message);
                else
                {
                    lblTitle.Text = Utility.Message;
                    lblTitle.Text +=
                        "<br> Your Settings Changes Require A New PXE Boot File Be Created.  <br>Create It Now?";
                    if (newClientIso)
                        lblClientISO.Text = "If You Are Using The Client ISO, It Must Also Be Manually Updated.";
                    ClientScript.RegisterStartupScript(GetType(), "modalscript",
                        "$(function() {  var menuTop = document.getElementById('confirmbox'),body = document.body;classie.toggle(menuTop, 'confirm-box-outer-open'); });",
                        true);
                    Session.Remove("Message");
                }

                ViewState["serverKey"] = "";
            }
        }

        protected void ImageXfer_Changed(object sender, EventArgs e)
        {
            ShowXferMode();
        }

        protected void OkButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/views/admin/bootmenu.aspx?defaultmenu=true");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!new Authorize().IsInMembership("Administrator"))
                Response.Redirect("~/views/dashboard/dash.aspx?access=denied");


            if (IsPostBack) return;
            txtIP.Text = Settings.ServerIp;
            txtPort.Text = Settings.WebServerPort;
            txtImagePath.Text = Settings.ImageStorePath;
            txtQSize.Text = Settings.QueueSize;
            txtSenderArgs.Text = Settings.SenderArgs;
            txtNFSPath.Text = Setting.GetValueForAdminView(Settings.NfsUploadPath);
            txtTFTPPath.Text = Settings.TftpPath;
            txtWebService.Text = Setting.GetValueForAdminView(Settings.WebPath);
            ddlPXEMode.SelectedValue = Settings.PxeMode;
            ddlProxyDHCP.SelectedValue = Settings.ProxyDhcp;
            ddlProxyBios.SelectedValue = Settings.ProxyBiosFile;
            ddlProxyEfi32.SelectedValue = Settings.ProxyEfi32File;
            ddlProxyEfi64.SelectedValue = Settings.ProxyEfi64File;
            ddlCompAlg.SelectedValue = Settings.CompressionAlgorithm;
            ddlCompLevel.SelectedValue = Settings.CompressionLevel;
            txtADLogin.Text = Settings.AdLoginDomain;
            ddlImageXfer.SelectedValue = Settings.ImageTransferMode;
            ddlImageChecksum.SelectedValue = Settings.ImageChecksum;
            ddlHostView.SelectedValue = Settings.DefaultHostView;
            ddlOnd.SelectedValue = Settings.OnDemand;
            txtRecArgs.Text = Settings.ReceiverArgs;
            txtStartPort.Text = Settings.StartPort;
            txtEndPort.Text = Settings.EndPort;
            txtServerKey.Text = Settings.ServerKey;
            txtImageHoldPath.Text = Settings.ImageHoldPath;
            txtNFSDeploy.Text = Setting.GetValueForAdminView(Settings.NfsDeployPath);
            ddlSSL.SelectedValue = Settings.ForceSsL;
            txtSMBPath.Text = Setting.GetValueForAdminView(Settings.SmbPath);
            txtSMBUser.Text = Settings.SmbUserName;
            txtSMBPass.Text = Settings.SmbPassword;
            txtRecClientArgs.Text = Settings.ClientReceiverArgs;
            txtGlobalHostArgs.Text = Settings.GlobalHostArgs;
            ddlWebTasksLogin.Text = Settings.WebTaskRequiresLogin;
            ddlOndLogin.Text = Settings.OnDemandRequiresLogin;
            ddlDebugLogin.Text = Settings.DebugRequiresLogin;
            ddlRegisterLogin.Text = Settings.RegisterRequiresLogin;
            txtSmtpServer.Text = Settings.SmtpServer;
            txtSmtpPort.Text = Settings.SmtpPort;
            ddlSmtpSsl.SelectedValue = Settings.SmtpSsl;
            txtSmtpUsername.Text = Settings.SmtpUsername;
            txtSmtpPassword.Text = Settings.SmtpPassword;
            txtSmtpFrom.Text = Settings.SmtpMailFrom;
            txtSmtpTo.Text = Settings.SmtpMailTo;

            if (Settings.NotifySuccessfulLogin == "1")
                chkLoginSuccess.Checked = true;
            if (Settings.NotifyFailedLogin == "1")
                chkLoginFailed.Checked = true;
            if (Settings.NotifyTaskStarted == "1")
                chkTaskStarted.Checked = true;
            if (Settings.NotifyTaskCompleted == "1")
                chkTaskCompleted.Checked = true;
            if (Settings.NotifyImageApproved == "1")
                chkImageApproved.Checked = true;
            if (Settings.NotifyResizeFailed == "1")
                chkResizeFailed.Checked = true;

            ViewState["startPort"] = txtStartPort.Text;
            ViewState["endPort"] = txtEndPort.Text;

            //These require pxe boot menu or client iso to be recreated
            ViewState["serverIP"] = txtIP.Text;
            ViewState["serverPort"] = txtPort.Text;
            ViewState["serverKey"] = txtServerKey.Text;
            ViewState["webService"] = txtWebService.Text;
            ViewState["pxeMode"] = ddlPXEMode.Text;
            ViewState["forceSSL"] = ddlSSL.Text;
            ViewState["proxyDHCP"] = ddlProxyDHCP.SelectedValue;
            ViewState["proxyBios"] = ddlProxyBios.SelectedValue;
            ViewState["proxyEfi32"] = ddlProxyEfi32.SelectedValue;
            ViewState["proxyEfi64"] = ddlProxyEfi64.SelectedValue;
            ViewState["webTaskRequiresLogin"] = ddlWebTasksLogin.SelectedValue;

            ShowXferMode();
            ShowProxyMode();
        }

        protected void ProxyDhcp_Changed(object sender, EventArgs e)
        {
            ShowProxyMode();
        }

        protected void ShowProxyMode()
        {
            ddlProxyBios.BackColor = Color.White;
            ddlProxyEfi32.BackColor = Color.White;
            ddlProxyEfi64.BackColor = Color.White;
            ddlProxyBios.Font.Strikeout = false;
            ddlProxyEfi32.Font.Strikeout = false;
            ddlProxyEfi64.Font.Strikeout = false;
            ddlPXEMode.BackColor = Color.White;
            ddlPXEMode.Font.Strikeout = false;
            if (ddlProxyDHCP.Text == "No")
            {
                ddlProxyBios.BackColor = Color.LightGray;
                ddlProxyEfi32.BackColor = Color.LightGray;
                ddlProxyEfi64.BackColor = Color.LightGray;
                ddlProxyBios.Font.Strikeout = true;
                ddlProxyEfi32.Font.Strikeout = true;
                ddlProxyEfi64.Font.Strikeout = true;
            }
            else
            {
                ddlPXEMode.BackColor = Color.LightGray;
                ddlPXEMode.Font.Strikeout = true;
            }
        }

        protected void ShowXferMode()
        {
            txtNFSDeploy.BackColor = Color.White;
            txtNFSDeploy.Font.Strikeout = false;
            txtNFSPath.BackColor = Color.White;
            txtNFSPath.Font.Strikeout = false;

            txtSMBPath.BackColor = Color.White;
            txtSMBPath.Font.Strikeout = false;
            txtSMBUser.BackColor = Color.White;
            txtSMBUser.Font.Strikeout = false;
            txtSMBPass.BackColor = Color.White;
            txtSMBPass.Font.Strikeout = false;

            if (ddlImageXfer.Text == "smb" || ddlImageXfer.Text == "smb+http")
            {
                txtNFSDeploy.BackColor = Color.LightGray;
                txtNFSDeploy.Font.Strikeout = true;
                txtNFSPath.BackColor = Color.LightGray;
                txtNFSPath.Font.Strikeout = true;
            }

            if (ddlImageXfer.Text != "smb" && ddlImageXfer.Text != "smb+http")
            {
                txtSMBPath.BackColor = Color.LightGray;
                txtSMBPath.Font.Strikeout = true;
                txtSMBUser.BackColor = Color.LightGray;
                txtSMBUser.Font.Strikeout = true;
                txtSMBPass.BackColor = Color.LightGray;
                txtSMBPass.Font.Strikeout = true;
            }

            if (ddlImageXfer.Text == "nfs+http")
            {
                txtNFSDeploy.BackColor = Color.LightGray;
                txtNFSDeploy.Font.Strikeout = true;
            }

            if (ddlImageXfer.Text == "udp+http")
            {
                txtNFSDeploy.BackColor = Color.LightGray;
                txtNFSDeploy.Font.Strikeout = true;
                txtNFSPath.BackColor = Color.LightGray;
                txtNFSPath.Font.Strikeout = true;

                txtSMBPath.BackColor = Color.LightGray;
                txtSMBPath.Font.Strikeout = true;
                txtSMBUser.BackColor = Color.LightGray;
                txtSMBUser.Font.Strikeout = true;
                txtSMBPass.BackColor = Color.LightGray;
                txtSMBPass.Font.Strikeout = true;
            }
        }

        protected bool ValidateSettings()
        {
            if (ActiveTask.ReadAll().Count > 0)
            {
                Utility.Message = "Settings Cannot Be Changed While Tasks Are Active";
                return false;
            }
            if (txtPort.Text != "80" && txtPort.Text != "443" && !string.IsNullOrEmpty(txtPort.Text))
            {
                txtWebService.Text = "http://[server-ip]:" + txtPort.Text + "/cruciblewds/service/client.asmx/";
            }
            if (txtPort.Text == "80" || txtPort.Text == "443" || string.IsNullOrEmpty(txtPort.Text))
            {
                txtWebService.Text = "http://[server-ip]/cruciblewds/service/client.asmx/";
            }
            if (ddlSSL.Text == "Yes")
            {
                if (txtWebService.Text.ToLower().Contains("http://"))
                    txtWebService.Text = txtWebService.Text.Replace("http://", "https://");
            }
            else if (txtWebService.Text.ToLower().Contains("https://"))
                txtWebService.Text = txtWebService.Text.Replace("https://", "http://");


            if (!txtImagePath.Text.Trim().EndsWith(Path.DirectorySeparatorChar.ToString()))
                txtImagePath.Text += Path.DirectorySeparatorChar;

            if (!txtImageHoldPath.Text.Trim().EndsWith(Path.DirectorySeparatorChar.ToString()))
                txtImageHoldPath.Text += Path.DirectorySeparatorChar;

            if (!txtTFTPPath.Text.Trim().EndsWith(Path.DirectorySeparatorChar.ToString()))
                txtTFTPPath.Text += Path.DirectorySeparatorChar;

            if (!txtNFSPath.Text.Trim().EndsWith("/"))
                txtNFSPath.Text += "/";

            if (!txtNFSDeploy.Text.Trim().EndsWith("/"))
                txtNFSDeploy.Text += ("/");

            if (!txtWebService.Text.Trim().EndsWith("/"))
                txtWebService.Text += ("/");

            if (txtSMBPath.Text.Contains("\\"))
                txtSMBPath.Text = txtSMBPath.Text.Replace("\\", "/");

            var startPort = Convert.ToInt32(txtStartPort.Text);
            var endPort = Convert.ToInt32(txtEndPort.Text);

            if (startPort%2 != 0)
            {
                startPort++;
                txtStartPort.Text = startPort.ToString();
            }
            if (endPort%2 != 0)
            {
                endPort++;
                txtEndPort.Text = endPort.ToString();
            }

            try
            {
                if ((startPort >= 2) && (endPort - startPort >= 2))
                {
                    return true;
                }
                Utility.Message = "End Port Must Be At Least 2 More Than Starting Port";
                return false;
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return false;
            }
        }
    }
}