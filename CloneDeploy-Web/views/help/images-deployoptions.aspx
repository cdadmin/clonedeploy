<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#images').addClass("nav-current");
            $('#images-deployoptions').addClass("nav-current-sub");
        });
    </script>
    <h1>Images->Deploy Options</h1>
    <h3>Change Computer Name (LIE WIE)</h3>

    <p>
        When this option is checked the computer’s name will be updated match what you have stored in CloneDeploy either by modifying
        the sysprep file or the registry during the imaging process. This is currently only implemented for Windows.
    </p>
    <h3>Don’t Expand Volumes (LIE)</h3>

    <p>
        During the deployment process ntfs and extfs filesystems are expanded to fill the full partition. Setting this option will
        disable that. I honestly can’t think of reason to do this, but it may be helpful for debugging.
    </p>
    <h3>Update BCD (LIE)</h3>

    <p>
        If your computer does not boot after deployment, try turning this on. CloneDeploy will update the BCD boot file on Windows machines to correct the boot problem.
    </p>
    <h3>Fix Bootloader (LIE)</h3>

    <p>
        This fixes the partition that is set to active / boot in cases where the NTFS geometry is incorrect. This should almost be
        left checked. Only applies to mbr partition tables.
    </p>
    <h3>Don't Update NVRAM (LIE)</h3>
    <p>When deploying an EFI image.  The NVRAM is updated in order to make the machine bootable.  In some cases this may cause problems and can be disabled here.  Many new systems will automatically detect the correct partition to boot even without correct NVRAM values.</p>
       

    <h3>Create Partition Method (LIE WIE)</h3>
    <p>This option selects how the partition tables will be setup on the client machine before image deployment.
        The default option is Dynamic and should work for most situations. This means that CloneDeploy will generate the appropriate
        sized partitions based on many different factors. It could possibly shrink or grow a partition to make them fit the new hard drive.</p>
    <h4><u>LIE</u></h4>
    <b>Use Original MBR / GPT</b><br/>
     <p>Restores the exact same partition table that was used on the original image. This option should
        only be used if you are having problems with the dynamic option. You can also only use this if the new hard drive is the same
        size or larger than the original. If you create an image from 80GB hard drive and you deploy to a 120GB hard drive, the 120GB
        hard drive will effectively become an 80GB hard drive. You would manually need to resize the partitions.</p>
    <b>Dynamic</b><br/>
     <p>Generates / resizes the appropriate partitions to fit on the destination drive based on many different factors.</p>
    <b>Standard</b><br/>
   <p> CloneDeploy does not attempt to calculate appropriate partition sizes.  A simple standard partition table is created, as if a new installation with default partitioning and then only the OS volume 
    image is restored.  Using this option enables you to deploy an EFI image to a legacy bios machine, and vice versa.  When using this option you must select on a single partition to deploy in the modify image schema.</p>
    <b>Custom Script</b><br/>
     <p>Allows you to make your own partitions via a shell script. You can use the partitioning
        tools available in the client boot image. These include fdisk, gdisk, and parted.</p>
   
    <h4><u>WIE</u></h4>
    <b>Standard</b><br/>
  <p> CloneDeploy does not attempt to calculate appropriate partition sizes.  A simple standard partition table is created, as if a new installation with default partitioning and then only the OS volume 
    image is restored.  Using this option enables you to deploy an EFI image to a legacy bios machine, and vice versa.  When using this option you must select on a single partition to deploy in the modify image schema.</p>
    <b>Dynamic</b><br/>
    <p> Generates / resizes the appropriate partitions to fit on the destination drive based on many different factors.</p>
    <b>Custom Script</b><br/>
    <p> Allows you to make your own partitions via a powershell script. You can use the partitioning
        tools available in WinPE, such as diskpart and powershell.</p>
    <p>
        
        There is one exception to the above partition options, if a hard drive you are deploying to exactly matches the size the image was created from,
        the partition method will automatically change to Original MBR / GPT. In this case CloneDeploy does not need to perform any
        calculations, it simply restores the original mbr / gpt.

      

       
    </p>
    <h3>Force Dynamic Partitions For Exact Hdd Match (LIE)</h3>

    <p>
        When deploying an image to a hard drive that is the exact same size as the source.  They dynamic partition method is not used even if selected.
        The Use Original MBR / GPT option is used. This option will disable that and force the Dynamic partition option to be used, for cases where the mbr / gpt
        does not restore properly.
    </p>
    <h3>Modify The Image Schema (LIE WIE)</h3>

    <p>
        Checking this box gives you control over what hard drives and partitions will be deployed, where they will restore to and give
        you custom sizing options. A new table will be displayed with these options. Any hard drive or partition that is checked will
        be deployed. Hard drives are always deployed in the order they are listed in this table. You can modify this by setting
        a value in the Destination box. For example, if the first hard drive listed in the table was originally /dev/sda it will be
        restored to the first hard drive that is found during the imaging process, it may also be /dev/sda or may not be. If you
        wanted to send this hard drive to the second hard drive in the system, you would set the Destination to /dev/sdb or whatever
        the matching hard drive name may be. Next, each partition can be set with a Custom Size. Enter a size you want the partition
        to be and select a unit for the size. Current options are MB, GB, and %. If you are using a percentage they do not need to add
        up to 100. Whatever percentage is remaining will automatically be spread across the remaining partitions. The same is true when
        using MB or GB. Finally each partition has check box called Fixed. When this box is checked the partition will not be adjusted
        to fit the new hard drive size. It will remain the exact same as the original partition that you uploaded. Partitions smaller
        than 5GB use this logic automatically. When you change the deploy schema, the original schema is never modified, a new copy is
        created and stored in the database so don’t worry about messing anything up. There is also an option to export the schema you
        have defined, I may ask for this for debugging info if you are having problems. One final note, if you set a custom size to
        use a percentage, the minimum client size that is displayed in the profile list will show N/A. This is because a minimum
        size cannot be known without knowing the size of the hard you are deploying to.
    </p>
</asp:Content>