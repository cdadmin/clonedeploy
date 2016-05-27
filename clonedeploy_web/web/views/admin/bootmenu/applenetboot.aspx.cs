using System;
using System.Data;
using System.Net;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class views_admin_bootmenu_applenetboot : System.Web.UI.Page
{
    /*BSDP Options
     *Code      Length      Values                              Name                        Client Or Server
    -1-         1           1-List, 2-Select,3-Failed           Message Type                Both
    -2-         2           uint16 encoding                     Version                     Client
    -3-         4           ipaddr encoding                     Server Identifier           Server
    -4-         2           uint16 0-65535                      Server Priority             Server
    -5-         2           uint16 < 1024                       Reply Port                  Client
    -7-         4           single:1-4095 cluster:4096-65535    Default Image Id            Server
    -8-         4                                               Selected Image Id           Both
    -9-         255 Max                                         Image List                  Server
    */

    /*Example Vendor Option String Serving two netboot images. 1 named net and 1 named boot
    01:01:01:03:04:A2:33:4B:A0:04:02:FF:FF:07:04:01:00:00:89:09:11:01:00:00:89:03:6E:65:74:01:00:00:88:04:62:6F:6F:74
    
    01:01:                                        option 1
          01:                                     List
    03:04:                                        option 3
          A2:33:4B:A0:                            Ip address
    04:02:                                        option 4
          FF:FF:                                  Max Value
    07:04:                                        Option 7
          01:00:00:89:                            image id always starts with 01:00 for Netboot OSX
    09:11:                                        Option 9 Length = 5 * [number of images] + [sum of image names]
          01:00:00:89:                            [image id]
                          03:                     [image name length]
                                6E:65:74:         [image name]
          01:00:00:88:                            [image id]
                          04:                     [image name length]
                                62:6f:6f:74       [image name]
    */

    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        BindGrid();
    }

    protected void BindGrid()
    {
        var obj = new DataTable();
        obj.Columns.Add("ImageId");
        obj.Columns.Add("Name");
        DataRow dataRow = obj.NewRow();
        obj.Rows.Add(dataRow);
        gvNetBoot.DataSource = obj;
        gvNetBoot.DataBind();

        gvNetBoot.Rows[0].Cells.Clear();
        gvNetBoot.Rows[0].Cells.Add(new TableCell());
        gvNetBoot.Rows[0].Cells[0].Text = "";
    }

    protected void btnSubmitDefault_OnClick(object sender, EventArgs e)
    {
        var vendorOptions = new StringBuilder();
        vendorOptions.Append("01:01:01:03:04:");

        IPAddress ip = IPAddress.Parse(txtServerIp.Text);

        foreach (byte i in ip.GetAddressBytes())
        {
            vendorOptions.Append(i.ToString("X2") + ":");
        }

        vendorOptions.Append("04:02:FF:FF:07:04:");

        int rowCount = 0;
        int totalNameLength = 0;
        foreach (GridViewRow row in gvNetBoot.Rows)
        {
            rowCount++;
            if (rowCount == 1)
            {
                var defaultId = (Label) row.FindControl("lblImageId");
                vendorOptions.Append("01:00:");
                vendorOptions.Append(AddHexColons(Convert.ToInt32(defaultId.Text).ToString("X4")));
                vendorOptions.Append(":");
            }
            var name = (Label)row.FindControl("lblName");
            totalNameLength += name.Text.Length;
        }

        vendorOptions.Append("09:" + (5 * rowCount + totalNameLength).ToString("X2"));
        vendorOptions.Append(":");
        foreach (GridViewRow row in gvNetBoot.Rows)
        {

            var imageId = (Label) row.FindControl("lblImageId");
            vendorOptions.Append("01:00:");
            vendorOptions.Append(AddHexColons(Convert.ToInt32(imageId.Text).ToString("X4")));
            vendorOptions.Append(":");

            var name = (Label) row.FindControl("lblName");
            vendorOptions.Append(name.Text.Length.ToString("X2"));
            vendorOptions.Append(":");
            vendorOptions.Append(StringToHex(name.Text));
        }

        txtOut.Text = vendorOptions.ToString();
    }

    protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        var dt = (DataTable)ViewState["VendorOptions"];
        dt.Rows[e.RowIndex].Delete();
        gvNetBoot.DataSource = dt;
        gvNetBoot.DataBind();
        if(gvNetBoot.Rows.Count == 0)
            BindGrid();
    }

    protected void btnAdd1_OnClick(object sender, EventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        var id = ((TextBox)gvRow.FindControl("txtIdAdd")).Text;
        var name = ((TextBox)gvRow.FindControl("txtNameAdd")).Text;
        DataTable dt;
        if(ViewState["VendorOptions"] != null)
        dt = (DataTable) ViewState["VendorOptions"];
        else
        {
            dt =new DataTable();
            dt.Columns.Add("ImageId");
            dt.Columns.Add("Name");
        }
        
        DataRow dataRow = dt.NewRow();
        dataRow[0] = id;
        dataRow[1] = name;
        dt.Rows.Add(dataRow);

        ViewState["VendorOptions"] = dt;

        gvNetBoot.DataSource = dt;
        gvNetBoot.DataBind();
    }

    public static string AddHexColons(string hex)
    {
        var sb = new StringBuilder();
        for (var i = 0; i < hex.Length; i++)
        {
            if (i % 2 == 0 && i != 0)
                sb.Append(':');
            sb.Append(hex[i]);
        }
        return sb.ToString();
    }

    private string StringToHex(string hexstring)
    {
        var sb = new StringBuilder();
        foreach (char t in hexstring)
            sb.Append(Convert.ToInt32(t).ToString("X2"));
        return AddHexColons(sb.ToString());
    }
}