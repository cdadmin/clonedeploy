using System;
using System.Collections.Generic;
using BasePages;
using CloneDeploy_Web.Models;
using Helpers;

public partial class views_admin_multicast : Admin
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
     
        txtSenderArgs.Text = Settings.SenderArgs;  
        txtStartPort.Text = Settings.StartPort;
        txtEndPort.Text = Settings.EndPort;   
        txtRecClientArgs.Text = Settings.ClientReceiverArgs;
        ddlDecompress.Text = Settings.MulticastDecompression;
      
        ViewState["startPort"] = txtStartPort.Text;
        ViewState["endPort"] = txtEndPort.Text;
    }

    protected void btnUpdateSettings_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.UpdateAdmin);
        if (ValidateSettings())
        {
            List<Setting> listSettings = new List<Setting>
            {
                new Setting
                {
                    Name = "Sender Args",
                    Value = txtSenderArgs.Text,
                    Id = BLL.Setting.GetSetting("Sender Args").Id
                },
                new Setting
                {
                    Name = "Udpcast Start Port",
                    Value = txtStartPort.Text,
                    Id = BLL.Setting.GetSetting("Udpcast Start Port").Id
                },
                new Setting
                {
                    Name = "Udpcast End Port",
                    Value = txtEndPort.Text,
                    Id = BLL.Setting.GetSetting("Udpcast End Port").Id
                },
                new Setting
                {
                    Name = "Client Receiver Args",
                    Value = txtRecClientArgs.Text,
                    Id = BLL.Setting.GetSetting("Client Receiver Args").Id
                },
                new Setting
                {
                    Name = "Multicast Decompression",
                    Value = ddlDecompress.Text,
                    Id = BLL.Setting.GetSetting("Multicast Decompression").Id
                },
            };

            if (BLL.Setting.UpdateSetting(listSettings))
            {
                EndUserMessage = "Successfully Updated Settings";
                if ((string) (ViewState["startPort"]) != txtStartPort.Text)
                {
                    var startPort = Convert.ToInt32(txtStartPort.Text);
                    startPort = startPort - 2;
                    var port = new Port {Number = startPort};
                    BLL.Port.AddPort(port);
                }
            }
            else
            {
                EndUserMessage = "Could Not Update Settings";
            }
        }

    }

    protected bool ValidateSettings()
    {
        var startPort = Convert.ToInt32(txtStartPort.Text);
        var endPort = Convert.ToInt32(txtEndPort.Text);

        if (startPort % 2 != 0)
        {
            startPort++;
            txtStartPort.Text = startPort.ToString();
        }
        if (endPort % 2 != 0)
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
            EndUserMessage = "End Port Must Be At Least 2 More Than Starting Port";
            return false;
        }
        catch (Exception ex)
        {
            Logger.Log(ex.Message);
            return false;
        }
    }
}