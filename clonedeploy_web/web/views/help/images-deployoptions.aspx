<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
     <script type="text/javascript">
         $(document).ready(function () {
             $('#images').addClass("nav-current");
             $('#images-deployoptions').addClass("nav-current-sub");
         });
        </script>
    <h1>Images->Deploy Options</h1>
   <h3>Change Computer Name</h3>

<p>When this option is checked the computer’s name will be updated match what you have stored in CloneDeploy either by modifying 
    the sysprep file or the registry during the imaging process.  This is currently only implemented for Windows.</p>
<h3>Don’t Expand Volumes</h3>

<p>During the deployment process ntfs and extfs filesystems are expanded to fill the full partition.  Setting this option will 
    disable that.  I honestly can’t think of reason to do this, but it may be helpful for debugging.</p>
<h3>Update BCD</h3>

<p>If your computer does not boot after deployment, try turning this on.  If during the deployment process your Windows partition 
    starting sector changes from the original image, this will update the BCD to the correct location.  This only applies to mbr 
    partition tables.  GPT partitions are handled in a different way where this should never be needed.</p>
<h3>Fix Bootloader</h3>

<p>This fixes the partition that is set to active / boot in cases where the NTFS geometry is incorrect.  This should almost be 
    left checked.  Only applies to mbr partition tables.</p>
<h3>Create Partition Method</h3>

<p>This option selects how the partition tables will be setup on the client machine before the imaging is done.  
    The default option is Dynamic and should work for most situations.  This means that CloneDeploy will generate the appropriate 
    sized partitions based on many different factors.  It could possibly shrink or grow a partition to make them fit the new hard drive.
      There is one exception to this, if a hard drive you are deploying to exactly matches the size the image was created from, 
    the partition method will automatically change to Original MBR / GPT.  In this case CloneDeploy does not need to perform any 
    calculations, it simply restores the original mbr / gpt.

The Use Original MBR / GPT option will restore the same partition table that was used on the original image.  This option should
     only be used if you are having problems with the dynamic option.  You can also only use this if the new hard drive is the same 
    size or larger than the original.  If you create an image from 80GB hard drive and you deploy to a 120GB hard drive, the 120GB 
    hard drive will effectively become an 80GB hard drive.  You would manually need to resize the partitions.

Finally the Custom Script option allows you to make your own partitions via a shell script.  You can use the partitioning 
    tools available in the client boot image.  These include fdisk, gdisk, and parted.</p>
<h3>Force Dynamic Partitions For Exact Hdd Match</h3>

<p>In the previous topic I mentioned how the Original MBR /GPT is always used if a hard drive size is an exact match to the 
    original image.  This option will disable that and force the Dynamic partition option to be used, for cases where the mbr / gpt 
    does not restore properly.</p>
<h3>Modify The Image Schema</h3>

<p>Checking this box gives you control over what hard drives and partitions will be deployed, where they will restore to and give 
    you custom sizing options.  A new table will be displayed with these options.  Any hard drive or partition that is checked will
     be deployed.  Hard drives are always deployed in the order they are listed in, in this table.  You can modify this by setting 
    a value in the Destination box.  For example, if the first hard drive listed in the table was originally /dev/sda it will be 
    restored to the first hard drive that is found during the imaging process, it may also be /dev/sda or may not be.  If you 
    wanted to send this hard drive to the second hard drive in the system, you would set the Destination to /dev/sdb or whatever
     the matching hard drive name may be.  Next, each partition can be set with a Custom Size.  Enter a size you want the partition
     to be and select a unit for the size.  Current options are MB, GB, and %.  If you are using a percentage they do not need to add 
    up to 100.  Whatever percentage is remaining will automatically be spread across the remaining partitions.  The same is true when 
    using MB or GB.  Finally each partition has check box called Fixed.  When this box is checked the partition will not be adjusted 
    to fit the new hard drive size.  It will remain the exact same as the original partition that you uploaded.  Partitions smaller
     than 5GB use this logic automatically.  When you change the deploy schema the original schema is never modified, a new copy is 
    created and stored in the database so don’t worry about messing anything up.  There is also an option to export the schema you 
    have defined, I may ask for this for debugging info if you are having problems.  One final note, if you set a custom size to
     use a percentage, the minimum client size that is displayed in the profile list will show N/A.  This is because a minimum 
    size cannot be known without knowing the size of the hard you are deploying to.</p>
</asp:Content>

