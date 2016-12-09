using System;
using CloneDeploy_Entities;
using CloneDeploy_Web;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

public partial class views_computers_proxy : Computers
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) PopulateForm();
    }

    protected void PopulateForm()
    {
        chkEnabled.Checked = Computer.ProxyReservation == 1;

        var reservation = Call.ComputerApi.GetProxyReservation(Computer.Id);
        if (reservation != null)
        {
            txtTftp.Text = reservation.NextServer;
            ddlBootFile.Text = reservation.BootFile;
        }
    }

    protected void buttonUpdate_OnClick(object sender, EventArgs e)
    {
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
            if (!Call.FilesystemApi.BootSdiExists())
            {
                EndUserMessage =
                    "Cannot Use WinPE.  You Have Not Updated Your tftpboot Folder With CloneDeployPE Builder";
                return;
            }
            
        }

        Call.ComputerApi.ToggleProxyReservation(Computer.Id, chkEnabled.Checked);
        var reservation = new ComputerProxyReservationEntity();
        reservation.ComputerId = Computer.Id;
        reservation.NextServer = txtTftp.Text;
        reservation.BootFile = ddlBootFile.Text;

        Call.ComputerProxyReservationApi.Post(reservation);

        EndUserMessage = "Successfully Updated Computer Reservation";
    }
}