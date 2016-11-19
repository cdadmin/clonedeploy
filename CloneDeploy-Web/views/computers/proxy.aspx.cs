using System;
using System.IO;
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

        var reservation = BLL.ComputerProxyReservation.GetComputerProxyReservation(Computer.Id);
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

        if (ddlBootFile.Text.Contains("winpe"))
        {
            if (
                !new Helpers.FileOps().FileExists(Settings.TftpPath + Path.DirectorySeparatorChar + "boot" +
                                                  Path.DirectorySeparatorChar + "boot.sdi"))
            {
                EndUserMessage =
                    "Cannot Use WinPE.  You Have Not Updated Your tftpboot Folder With CloneDeploy PE Maker";
                return;
            }
            
        }

        BLL.ComputerProxyReservation.ToggleProxyReservation(Computer, chkEnabled.Checked);
        var reservation = new Models.ComputerProxyReservation();
        reservation.ComputerId = Computer.Id;
        reservation.NextServer = txtTftp.Text;
        reservation.BootFile = ddlBootFile.Text;

        BLL.ComputerProxyReservation.UpdateComputerProxyReservation(reservation);

        EndUserMessage = "Successfully Updated Computer Reservation";
    }
}