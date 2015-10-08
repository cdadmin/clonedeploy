using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using BasePages;
using Helpers;
using Newtonsoft.Json;
using Partition;

namespace views.images
{
    public partial class ImageSpecs : Images
    {
        protected void btnConfirmChecksum_Click(object sender, EventArgs e)
        {
            var image = BllImage.GetImage(Image.Id);
            image.Checksum = (string) (ViewState["checkSum"]);
            BllImage.UpdateImage(image,image.Name);
            Response.Redirect("~/views/images/edit.aspx?imageid=" + image.Id, true);
        }

        protected void btnPart_Click(object sender, EventArgs e)
        {
       
            var imagePath = Settings.ImageStorePath + Image.Name;

            var selectedHd = (string) (ViewState["selectedHD"]);
            var control = sender as Control;
            if (control == null) return;
            var gvRow = (GridViewRow) control.Parent.Parent;
            var gv = (GridView) gvRow.FindControl("gvFiles");
            var selectedPartition = gvRow.Cells[3].Text;

            string[] partFiles;
            try
            {
                if (selectedHd == "0")
                    partFiles = Directory.GetFiles(imagePath + Path.DirectorySeparatorChar,
                        "part" + selectedPartition + ".*");
                else
                {
                    selectedHd = (Convert.ToInt32(selectedHd) + 1).ToString();
                    partFiles =
                        Directory.GetFiles(
                            imagePath + Path.DirectorySeparatorChar + "hd" + selectedHd + Path.DirectorySeparatorChar,
                            "part" + selectedPartition + ".*");
                }
            }
            catch
            {
                return;
            }
            if (partFiles.Length == 0)
            {
                return;
            }

            var dt = new DataTable();
            dt.Columns.Add(new DataColumn("fileName", typeof (string)));
            dt.Columns.Add(new DataColumn("serverSize", typeof (string)));

            foreach (var file in partFiles)
            {
                DataRow dr;
                try
                {
                    var fi = new FileInfo(file);
                    dr = dt.NewRow();
                    dr["fileName"] = fi.Name;
                    dr["serverSize"] = (fi.Length/1024f/1024f).ToString("#.##") + " MB";
                    dt.Rows.Add(dr);
                }
                catch
                {
                    var fi = new FileInfo(file);
                    dr = dt.NewRow();
                    dr["fileName"] = fi.Name;
                    dt.Rows.Add(dr);
                }
            }

            var btn = (LinkButton) gvRow.FindControl("partClick");

            if (gv.Visible == false)
            {
                gv.Visible = true;
                var td = gvRow.FindControl("tdFile");
                td.Visible = true;
                gv.DataSource = dt;
                gv.DataBind();
                btn.Text = "-";
            }
            else
            {
                gv.Visible = false;
                var td = gvRow.FindControl("tdFile");
                td.Visible = false;
                btn.Text = "+";
            }
        }

        protected void btnParts_Click(object sender, EventArgs e)
        {
        

            var control = sender as Control;
            if (control == null) return;
            var gvRow = (GridViewRow) control.Parent.Parent;
            var gv = (GridView) gvRow.FindControl("gvParts");

            var selectedHd = gvRow.Cells[3].Text;
            ViewState["selectedHD"] = gvRow.RowIndex.ToString();
            ViewState["selectedHDName"] = selectedHd;
            var specs =
                JsonConvert.DeserializeObject<ImagePhysicalSpecs>(!string.IsNullOrEmpty(Image.ClientSizeCustom)
                    ? Image.ClientSizeCustom
                    : Image.ClientSize);

            var specslist = new List<PartitionPhysicalSpecs>();
            var vgList = new List<VgPhysicalSpecs>();
            foreach (var hd in specs.Hd)
            {
                if (hd.Name != selectedHd) continue;
                foreach (var part in hd.Partition)
                {
                    var logicalBlockSize = Convert.ToInt64(hd.Lbs);
                    if ((Convert.ToInt64(part.Size)*logicalBlockSize) < 1048576000)
                        part.Size = (Convert.ToInt64(part.Size)*logicalBlockSize/1024f/1024f).ToString("#.##") + " MB";
                    else
                        part.Size = (Convert.ToInt64(part.Size)*logicalBlockSize/1024f/1024f/1024f).ToString("#.##") +
                                    " GB";
                    part.Used_Mb = part.Used_Mb + " MB";
                    if (!string.IsNullOrEmpty(part.Size_Override))
                        part.Size_Override =
                            (Convert.ToInt64(part.Size_Override)*logicalBlockSize/1024f/1024f).ToString(
                                CultureInfo.InvariantCulture);
                    if (!string.IsNullOrEmpty(part.Resize))
                        part.Resize = part.Resize + " MB";
                    specslist.Add(part);

                    if (part.Vg == null) continue;
                    if (part.Vg.Name != null)

                        vgList.Add(part.Vg);
                }
            }


            var btn = (LinkButton) gvRow.FindControl("btnParts");
            if (gv.Visible == false)
            {
                gv.Visible = true;

                var td = gvRow.FindControl("tdParts");
                td.Visible = true;
                gv.DataSource = specslist;
                gv.DataBind();

                btn.Text = "-";
            }
            else
            {
                gv.Visible = false;

                var td = gvRow.FindControl("tdParts");
                td.Visible = false;
                btn.Text = "+";
            }

            foreach (GridViewRow row in gv.Rows)
            {
                var gvVg = (GridView) row.FindControl("gvVG");
                foreach (var vg in vgList)
                {
                    if (vg.Pv != selectedHd + row.Cells[3].Text) continue;
                    var vgListBind = new List<VgPhysicalSpecs> {vg};
                    gvVg.DataSource = vgListBind;
                    gvVg.DataBind();
                    gvVg.Visible = true;
                    var td = row.FindControl("tdVG");
                    td.Visible = true;
                }
                var isActive = ((HiddenField) row.FindControl("HiddenActivePart")).Value;
                if (isActive != "1") continue;
                var box = row.FindControl("chkPartActive") as CheckBox;
                if (box != null) box.Checked = true;
            }
        }

        protected void btnRestoreImageSpecs_Click(object sender, EventArgs e)
        {
            var image = Image;
            image.ClientSizeCustom = "";
           
            //FIX ME
            /*
            Message.Text = (BllImage.UpdateImage(image,image.Name)
                ? "Successfully Restored Image Specs.  Reload This Page To View Changes."
                : "Could Not Restore Image Specs");*/
        }

        protected void btnUpdateImageSpecs_Click(object sender, EventArgs e)
        {
            var image = Image;
           
            ImagePhysicalSpecs specs;
            try
            {
                specs = JsonConvert.DeserializeObject<ImagePhysicalSpecs>(image.ClientSize);
            }
            catch
            {
                return;
            }

            var rowCounter = 0;
            foreach (GridViewRow row in gvHDs.Rows)
            {
                var box = row.FindControl("chkHDActive") as CheckBox;
                specs.Hd[rowCounter].Active = box != null && box.Checked ? "1" : "0";

                var gvParts = (GridView) row.FindControl("gvParts");

                var partCounter = 0;
                foreach (GridViewRow partRow in gvParts.Rows)
                {
                    var boxPart = partRow.FindControl("chkPartActive") as CheckBox;
                    if (boxPart != null && boxPart.Checked)
                        specs.Hd[rowCounter].Partition[partCounter].Active = "1";
                    else
                        specs.Hd[rowCounter].Partition[partCounter].Active = "0";

                    var txtCustomSize = partRow.FindControl("txtCustomSize") as TextBox;
                    if (txtCustomSize != null && !string.IsNullOrEmpty(txtCustomSize.Text))
                    {
                        var customSizeBlk =
                            (Convert.ToInt64(txtCustomSize.Text)*1024*1024/Convert.ToInt32(specs.Hd[rowCounter].Lbs))
                                .ToString
                                ();
                        specs.Hd[rowCounter].Partition[partCounter].Size_Override = customSizeBlk;
                    }


                    var gvVg = (GridView) partRow.FindControl("gvVG");
                    foreach (GridViewRow vg in gvVg.Rows)
                    {
                        var gvLvs = (GridView) vg.FindControl("gvLVS");
                        var lvCounter = 0;
                        foreach (GridViewRow lv in gvLvs.Rows)
                        {
                            var boxLv = lv.FindControl("chkPartActive") as CheckBox;
                            if (boxLv != null && boxLv.Checked)
                                specs.Hd[rowCounter].Partition[partCounter].Vg.Lv[lvCounter].Active = "1";
                            else
                                specs.Hd[rowCounter].Partition[partCounter].Vg.Lv[lvCounter].Active = "0";

                            var txtCustomSizeLv = lv.FindControl("txtCustomSize") as TextBox;
                            if (txtCustomSizeLv != null && !string.IsNullOrEmpty(txtCustomSizeLv.Text))
                            {
                                var customSizeBlk =
                                    (Convert.ToInt64(txtCustomSizeLv.Text)*1024*1024/
                                     Convert.ToInt32(specs.Hd[rowCounter].Lbs))
                                        .ToString();
                                specs.Hd[rowCounter].Partition[partCounter].Vg.Lv[lvCounter].Size_Override =
                                    customSizeBlk;
                            }
                            lvCounter++;
                        }
                    }
                    partCounter++;
                }
                rowCounter++;
            }
            image.ClientSizeCustom = JsonConvert.SerializeObject(specs);
            //FIX ME
            /*
            Message.Text = (BllImage.UpdateImage(image,image.Name)
                ? "Successfully Updated Image Specs"
                : "Could Not Update Image Specs");*/
        }

        protected void btnVG_Click(object sender, EventArgs e)
        {
            var image = Image;


            var control = sender as Control;
            if (control == null) return;
            var gvRow = (GridViewRow) control.Parent.Parent;
            var gv = (GridView) gvRow.FindControl("gvLVS");

            var selectedHd = (string) (ViewState["selectedHD"]);

            var specs =
                JsonConvert.DeserializeObject<ImagePhysicalSpecs>(!string.IsNullOrEmpty(image.ClientSizeCustom)
                    ? image.ClientSizeCustom
                    : image.ClientSize);


            var lvList = new List<LvPhysicalSpecs>();

            foreach (var partition in specs.Hd[Convert.ToInt32(selectedHd)].Partition)
            {
                if (partition.Vg.Lv == null) continue;
                if (partition.Vg.Name == null) continue;
                foreach (var lv in partition.Vg.Lv)
                {
                    if (gvRow.Cells[1].Text != lv.Vg) continue;
                    var logicalBlockSize = Convert.ToInt64(specs.Hd[Convert.ToInt32(selectedHd)].Lbs);
                    if ((Convert.ToInt64(lv.Size)*logicalBlockSize) < 1048576000)
                        lv.Size = (Convert.ToInt64(lv.Size)*logicalBlockSize/1024f/1024f).ToString("#.##") +
                                  " MB";
                    else
                        lv.Size =
                            (Convert.ToInt64(lv.Size)*logicalBlockSize/1024f/1024f/1024f).ToString("#.##") +
                            " GB";
                    lv.Used_Mb = lv.Used_Mb + " MB";
                    if (!string.IsNullOrEmpty(lv.Size_Override))
                        lv.Size_Override =
                            (Convert.ToInt64(lv.Size_Override)*logicalBlockSize/1024f/1024f).ToString(
                                CultureInfo.InvariantCulture);
                    if (!string.IsNullOrEmpty(lv.Resize))
                        lv.Resize = lv.Resize + " MB";


                    lvList.Add(lv);
                }
            }

            var btn = (LinkButton) gvRow.FindControl("vgClick");
            if (gv.Visible == false)
            {
                gv.Visible = true;

                var td = gvRow.FindControl("tdLVS");
                td.Visible = true;
                gv.DataSource = lvList;
                gv.DataBind();
                btn.Text = "-";
            }

            else
            {
                gv.Visible = false;
                var td = gvRow.FindControl("tdLVS");
                td.Visible = false;
                btn.Text = "+";
            }

            foreach (GridViewRow row in gv.Rows)
            {
                var isActive = ((HiddenField) row.FindControl("HiddenActivePart")).Value;
                if (isActive != "1") continue;
                var box = row.FindControl("chkPartActive") as CheckBox;
                if (box != null) box.Checked = true;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
         
            if (!IsPostBack) PopulateSpecs();
        }

        protected void PopulateSpecs()
        {
         

            try
            {
                var specs =
                    JsonConvert.DeserializeObject<ImagePhysicalSpecs>(!string.IsNullOrEmpty(Image.ClientSizeCustom)
                        ? Image.ClientSizeCustom
                        : Image.ClientSize);


                var specslist = new List<HdPhysicalSpecs>();

                foreach (var hd in specs.Hd)
                {
                    var logicalBlockSize = Convert.ToInt64(hd.Lbs);
                    hd.Size = (Convert.ToInt64(hd.Size)*logicalBlockSize/1000f/1000f/1000f).ToString("#.##") + " GB" +
                              " / " + (Convert.ToInt64(hd.Size)*logicalBlockSize/1024f/1024f/1024f).ToString("#.##") +
                              " GB";
                    specslist.Add(hd);
                }

                gvHDs.DataSource = specslist;
                gvHDs.DataBind();

                foreach (GridViewRow row in gvHDs.Rows)
                {
                    var isActive = ((HiddenField) row.FindControl("HiddenActive")).Value;
                    if (isActive != "1") continue;
                    var box = row.FindControl("chkHDActive") as CheckBox;
                    if (box != null) box.Checked = true;
                }
            }
            catch
            {
                lblSpecsUnavailable.Text = "Image Specifications Will Be Available After The Image Is Uploaded";
                lblSpecsUnavailable.Visible = true;
            }

            if (Settings.ImageChecksum != "On" || lblSpecsUnavailable.Visible) return;
            try
            {
                var listPhysicalImageChecksums = new List<HdChecksum>();
                var path = Settings.ImageStorePath + Image.Name;
                var imageChecksum = new HdChecksum
                {
                    HdNumber = "hd1",
                    Path = path
                };
                listPhysicalImageChecksums.Add(imageChecksum);
                for (var x = 2;; x++)
                {
                    imageChecksum = new HdChecksum();
                    var subdir = path + Path.DirectorySeparatorChar + "hd" + x;
                    if (Directory.Exists(subdir))
                    {
                        imageChecksum.HdNumber = "hd" + x;
                        imageChecksum.Path = subdir;
                        listPhysicalImageChecksums.Add(imageChecksum);
                    }
                    else
                        break;
                }

                foreach (var hd in listPhysicalImageChecksums)
                {
                    var listChecksums = new List<FileChecksum>();

                    var files = Directory.GetFiles(hd.Path, "*.*");
                    foreach (var file in files)
                    {
                        var fc = new FileChecksum
                        {
                            FileName = Path.GetFileName(file),
                            Checksum = BllImage.Calculate_Hash(file)
                        };
                        listChecksums.Add(fc);
                    }
                    hd.Path = string.Empty;
                    hd.Fc = listChecksums.ToArray();
                }


                var physicalImageJson = JsonConvert.SerializeObject(listPhysicalImageChecksums);
                if (physicalImageJson == Image.Checksum) return;
                incorrectChecksum.Visible = true;
                ViewState["checkSum"] = physicalImageJson;
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " This can be safely ignored if the image has not been uploaded yet");
                incorrectChecksum.Visible = false;
            }
        }
    }
}