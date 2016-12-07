using System;
using System.Collections.Generic;
using BasePages;
using CloneDeploy_Entities;
using CloneDeploy_Web;

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
            List<SettingEntity> listSettings = new List<SettingEntity>
            {
                new SettingEntity
                {
                    Name = "Sender Args",
                    Value = txtSenderArgs.Text,
                    Id = Call.SettingApi.GetSetting("Sender Args").Id
                },
                new SettingEntity
                {
                    Name = "Udpcast Start Port",
                    Value = txtStartPort.Text,
                    Id = Call.SettingApi.GetSetting("Udpcast Start Port").Id
                },
                new SettingEntity
                {
                    Name = "Udpcast End Port",
                    Value = txtEndPort.Text,
                    Id = Call.SettingApi.GetSetting("Udpcast End Port").Id
                },
                new SettingEntity
                {
                    Name = "Client Receiver Args",
                    Value = txtRecClientArgs.Text,
                    Id = Call.SettingApi.GetSetting("Client Receiver Args").Id
                },
                new SettingEntity
                {
                    Name = "Multicast Decompression",
                    Value = ddlDecompress.Text,
                    Id = Call.SettingApi.GetSetting("Multicast Decompression").Id
                },
            };

            if (Call.SettingApi.UpdateSettings(listSettings))
            {
                EndUserMessage = "Successfully Updated Settings";
                if ((string) (ViewState["startPort"]) != txtStartPort.Text)
                {
                    var startPort = Convert.ToInt32(txtStartPort.Text);
                    startPort = startPort - 2;
                    var port = new PortEntity {Number = startPort};
                    Call.PortApi.Post(port);
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