using System;
using System.IO;
using CloneDeploy_Web.APICalls;
using CloneDeploy_Web.Models;
using Helpers;

public partial class views_computers_proxy : BasePages.Computers
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) PopulateForm();
    }

    protected void PopulateForm()
    {
        chkEnabled.Checked = Computer.ProxyReservation == 1;

        var reservation = new APICall().ComputerProxyReservationApi.Get(Computer.Id);
        if (reservation != null)
        {
            txtTftp.Text = reservation.NextServer;
            ddlBootFile.Text = reservation.BootFile;
        }
    }

    protected void buttonUpdate_OnClick(object sender, EventArgs e)
    {
        var call = new APICall();
        if (chkEnabled.Checked)
        {
            if (Settings.ProxyDhcp == "No")
            {
                EndUserMessage = "Proxy DHCP Mode Must Be Enabled To Use Proxy Reservations";
                return;
            }

        }

        //Cluster Issue
        if (ddlBootFile.Text.Contains("winpe"))
        {
            if (!call.FilesystemApi.BootSdiExists().Value)
            {
                EndUserMessage =
                    "Cannot Use WinPE.  You Have Not Updated Your tftpboot Folder With CloneDeployPE Builder";
                return;
            }
            
        }

        call.ComputerProxyReservationApi.Toggle(Computer.Id, chkEnabled.Checked);
        var reservation = new ComputerProxyReservation();
        reservation.ComputerId = Computer.Id;
        reservation.NextServer = txtTftp.Text;
        reservation.BootFile = ddlBootFile.Text;

        call.ComputerProxyReservationApi.Put(0,reservation);

        EndUserMessage = "Successfully Updated Computer Reservation";
    }
}