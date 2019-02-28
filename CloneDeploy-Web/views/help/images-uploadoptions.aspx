<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#images').addClass("nav-current");
            $('#images-uploadoptions').addClass("nav-current-sub");
        });
    </script>
    <h1>Images->Upload Options</h1>
    <h3>Remove GPT Structures (LIE)</h3>

    <p>
        A hard drive can actually have an mbr and a gpt partition table. Hard drives that have both will not function with CloneDeploy.
        If you are certain that you are using mbr, this will clear out the gpt part of the table before the upload.
    </p>
    <h3>Don’t Shrink Volumes (LIE)</h3>

    <p>
        By default, when a Block image is uploaded, all ntfs or extfs filesystems that are larger than 5GB are shrunk to the smallest
        volume size possible to allow restoring to hard drives that are smaller than the current one being captured. If this is causing
        problems you can disable that here.
    </p>
    <h3>Don’t Shrink LVM Volumes (LIE)</h3>

    <p>Same as above but specifically for LVM. Don’t Shrink Volumes will supersede this setting, but not vice versa.</p>

    <h3>Skip Hibernation Check (LIE)</h3>
    <p>Before an image is uploaded, it checks for the presence of hiberfil.sys and cancels the upload if it exists.  Uploading an image while hibernated can completely break the original image.  Enabling this option will
        skip this check.</p>
    
    <h3>Skip Bitlocker Check (LIE)</h3>
    <p>Bitlocker is not supported and must be disabled before uploading an image.  CloneDeploy will attempt to see if Bitlocker is enabled and error out if so.  Enabling this option skips the bitlocker check.</p>
    <h3>Compression Algorithm (LIE)</h3>

    <p>A few different ways to compress or not compress your image</p>
    <h3>Compression Level (LIE)</h3>
    
  
    <p>Higher is greater compression</p>
    
      <h3>Enable Multicast Support (WIE LIE-File Mode)</h3>
    <p>When using the WinPE Imaging Environment or Linux set to File, if you later want to multicast the image, this must be checked before upload.</p>
    <h3>Only Upload Schema (LIE WIE)</h3>

    <p>
        If you want to control what hard drives or partitions to upload, this is the first step. Turn this setting On and start an
        upload task. Instead of uploading an entire image, it will only upload the schema.
    </p>
    <h3>Use Custom Upload Schema(LIE WIE)</h3>

    <p>
        If you want to control what hard drives or partitions to upload, this is the second step. Check this box and a new table will
        be available to visually pick your partitions but only after you have uploaded the schema. The table will list each hard drive
        and partition that was found, simply check or uncheck the one’s you want. There is also an option for each partition called fixed.
        If this box is checked the filesystem for that partition will not be shrunk. This is a more flexible option than setting the
        Don’t shrink volumes setting which applies to all partitions. The Upload schema only box must be unchecked when use custom
        upload schema is checked. This does not modify the schema that was uploaded, it generates a new one that is stored in the
        database, the original is never modified.
    </p>
</asp:Content>