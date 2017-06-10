. x:\wie_global_functions.ps1

function Create-Partition-Layout()
{
    Clear-Disk $hardDrive.Number -RemoveData -RemoveOEM -Confirm:$false
    if($script:bootType -eq "efi")
    {
        Initialize-Disk $hardDrive.Number –PartitionStyle GPT
    }
    else
    {
        Initialize-Disk $hardDrive.Number –PartitionStyle MBR
    }

    if($partition_method -eq "standard")
    {
        log " ** Creating A Standard Windows Partition Layout ** "
        if($script:bootType -eq "efi")
        {
            "select disk $($hardDrive.Number)","create partition efi size=260","format quick fs=fat32 label=`"System`"","assign letter=`"Q`"", `
            "create partition msr size=16", `
            "create partition primary","shrink minimum=500","format quick fs=ntfs label=`"Windows`"", `
            "create partition primary","format quick fs=ntfs label=`"Recovery tools`"","assign letter=`"R`"","set id=`"de94bba4-06d1-4d40-a16a-bfd50179d6ac`"","gpt attributes=0x8000000000000001" `
            | diskpart 2>&1 >> $clientLog    
            $script:targetPartition=$(Get-Partition -DiskNumber $hardDrive.Number -PartitionNumber 3 )
        }
        else
        {
            "select disk $($hardDrive.Number)","create partition primary size=100","format quick fs=ntfs label=`"System`"","assign letter=`"Q`"", "active", `
            "create partition primary","shrink minimum=500","format quick fs=ntfs label=`"Windows`"", `
            "create partition primary","format quick fs=ntfs label=`"Recovery`"","assign letter=`"R`"","set id=27" `
            | diskpart 2>&1 >> $clientLog    
            $script:targetPartition=$(Get-Partition -DiskNumber $hardDrive.Number -PartitionNumber 2 )
        }

    }
    elseif($partition_method -eq "script")
    {
        log " ** Creating Partition Table On $($hardDrive.Number) From Custom Script ** " "true"
        curl.exe $script:curlOptions -H Authorization:$script:userTokenEncoded --data "profileId=$profile_id" ${script:web}GetCustomPartitionScript --connect-timeout 10 --stderr - > x:\newPartLayout.ps1
    }
    else #dynamic
    {
        log "imageProfileId=$profile_id&hdToGet=$script:imageHdToUse&newHDSize=$($hardDrive.Size)&clientHD=$($hardDrive.Number)&taskType=deploy&partitionPrefix=&lbs=$($hardDrive.LogicalSectorSize) ${script:web}GetPartLayout" 
        curl.exe $script:curlOptions -H Authorization:$script:userTokenEncoded --data "imageProfileId=$profile_id&hdToGet=$script:imageHdToUse&newHDSize=$($hardDrive.Size)&clientHD=$($hardDrive.Number)&taskType=deploy&partitionPrefix=&lbs=$($hardDrive.LogicalSectorSize)" ${script:web}GetPartLayout --connect-timeout 10 --stderr - > x:\newPartLayout.ps1
	    if($(Get-Content x:\newPartLayout.ps1) -eq "failed")
        {
	        error "Could Not Dynamically Create Partition Layout"
	    }
	}

    if($partition_method -ne "standard")
    {
	    log " ** Partition Creation Script ** "
        Get-Content x:\newPartLayout.ps1 | Out-File $clientLog -Append
        . x:\newPartLayout.ps1

        #Find and mount system partition
        if($script:bootType -eq "efi")
        {
            #Not sure why but powershell cannot format the system partition as fat32 - use diskpart instead
            $sysPartition=$(Get-Partition -DiskNumber $($hardDrive.Number) | Where-Object {$_.Type -eq "System"})
            "select disk $($hardDrive.Number)","select partition $($sysPartition.PartitionNumber)","format fs=fat32" | diskpart 2>&1 >> $clientLog
            $sysPartition | Set-Partition -NewDriveLetter Q 2>&1 >> $clientLog
        }
        else #legacy bios
        {
            $bootPartition=$(Get-Partition -DiskNumber $hardDrive.Number | Where-Object {$_.IsActive -eq $true})
            $bootPartition | Set-Partition -NewDriveLetter Q 2>&1 >> $clientLog
            log "boot partition is $($bootPartition.PartitionNumber)" "true"
        }
    }
    
    log " ** New Partition Table Is ** "
    Get-Partition -DiskNumber $hardDrive.Number | Out-File $clientLog -Append

    Process-Partitions
}

function Process-Partitions()
{
    $arrayIndex=-1
  	
    while($($arrayIndex + 1 ) -lt $($hdSchema.PhysicalPartitionCount))
    {
        $arrayIndex++
        clear
        $currentPartition=$($hdSchema.PhysicalPartitions[$arrayIndex])

        if($partition_method -eq "standard")
        {
            #Change the destination partition to be the target Partition defined in the Create-Partition-Layout function
            $script:stdPartSource=$currentPartition.Number
            $currentPartition.Number=$script:targetPartition.PartitionNumber
        }

        log $currentPartition
    

        #No need to check these for a standard partition layout, only 1 partition ever gets deployed
        if($partition_method -ne "standard")
        {
            if($($hdSchema.PartitionType) -eq "gpt")
            {
                if($($currentPartition.Type) -eq "system" -or $($currentPartition.Type) -eq "recovery" -or $($currentPartition.Type) -eq "reserved")
                {
                    continue
                }
            }
            else #mbr
            {
            
                if(($($currentPartition.Number) -eq $bootPartition.PartitionNumber) -and $($hdSchema.PhysicalPartitionCount) -gt 1 )
                {               
                    continue
                }
            }
        }
          
                 
        Download-Image
        
        if($file_copy -eq "True")
        {
            Process-File-Copy
        }

        if(Test-Path c:\Windows)
        {
            $script:windowsPartition=$(Get-Partition -DiskNumber $($hardDrive.Number) -PartitionNumber $($currentPartition.Number))
                
            bcdboot c:\Windows /s q: >> $clientLog 

            if($change_computer_name -eq "true" -and $computer_name)
            {
                Change-Computer-Name
            }

            if($sysprep_tags.trim("`""))
            {
                Process-Sysprep-Tags
            }
        }

        mountvol.exe c:\ /d
    }
	
}
function Reg-Key-Exists($regObject, $value)
{
    try 
    {
        $regObject | Select-Object -ExpandProperty "$value" -ErrorAction Stop | Out-Null
        return $true
    }

    catch 
    {
        return $false
    }

}

function Download-Image()
{
    log " ** Starting Image Download For Hard Drive $($hardDrive.Number) Partition $($currentPartition.Number)" "true"


    curl.exe $script:curlOptions -H Authorization:$script:userTokenEncoded --data "taskId=$script:taskId&partition=$($currentPartition.Number)" ${script:web}UpdateProgressPartition  --connect-timeout 10 --stderr -

    Set-Partition -DiskNumber $($hardDrive.Number) -PartitionNumber $($currentPartition.Number) -NewDriveLetter C 2>&1 >> $clientLog

    $reporterProc=$(Start-Process powershell "x:\wie_reporter.ps1 -web $script:web -taskId $script:taskId -partitionNumber $($currentPartition.Number) -direction Deploying -curlOptions $script:curlOptions -userTokenEncoded $script:userTokenEncoded -isOnDemand $script:isOnDemand" -NoNewWindow -PassThru)
    
    if($multicast -eq "true")
    {
        log "udp-receiver --portbase $multicast_port --no-progress --mcast-rdv-address $server_ip $client_receiver_args | wimapply - 1 C: 2>>$clientLog > x:\wim.progress"
        $udpProc=$(Start-Process cmd "/c udp-receiver --portbase $multicast_port --no-progress --mcast-rdv-address $server_ip $client_receiver_args | wimapply - 1 C: 2>>x:\wim.log > x:\wim.progress" -NoNewWindow -PassThru)
        Start-Sleep 5
        $wimProc=$(Get-Process wimlib-imagex)
        Wait-Process $wimProc.Id
    }
    else
    {
        if($script:stdPartSource)
        {
            $wimSource=$script:stdPartSource
        }
        else
        {
            $wimSource=$currentPartition.Number
        }

        log "wimapply $script:imagePath\part$wimSource.winpe.wim C: 2>>$clientLog > x:\wim.progress"
        wimapply $script:imagePath\part$wimSource.winpe.wim C: 2>>$clientLog > x:\wim.progress
    }
    
    Start-Sleep 5
    Stop-Process $reporterProc
    
}

function Process-Sysprep-Tags()
{
    if(Test-Path c:\Windows\Panther\unattend.xml)
    {
        foreach($tagId in -Split $sysprep_tags.trim("`""))
        {
            $tag=$(curl.exe $script:curlOptions -H Authorization:$script:userTokenEncoded --data "tagId=$tagId&imageEnvironment=win" ${script:web}GetSysprepTag --connect-timeout 10 --stderr -)
	        log " ** Running Custom Sysprep Tag With Id $tagId ** " "true"
            log $tag
	        $tag=$tag | ConvertFrom-Json
            if(!$?)
            {
                $Error[0].Exception.Message
                $tag
                log "Could Not Parse Sysprep Tag"
                continue
            }
            
            $tag.Contents=$($ExecutionContext.InvokeCommand.ExpandString($tag.Contents))
            log $tag.Contents
            sleep 5
            perl -0777 "-i.bak" -pe "s/($($tag.OpeningTag)).*($($tag.ClosingTag))/`${1}$($tag.Contents)`${2}/si" c:\Windows\Panther\unattend.xml   
        }
    }
    
}

function Process-Scripts($scripts)
{
    foreach($script in -Split $scripts.trim("`""))
    {
        curl.exe $script:curlOptions -H Authorization:$script:userTokenEncoded --data "scriptId=$script" ${script:web}GetCustomScript --connect-timeout 10 --stderr - > x:\script$($script).ps1
	    log " ** Running Custom Script With Id $script ** " "true"
	    sleep 5
	    #source script in sub shell so the values of the core script do not get overwritten
	    ( . x:\script$($script).ps1 )
    }
}

function Process-File-Copy()
{
     log " ** Processing File Copy ** " "true"
        foreach($file in $fileCopySchema.FilesAndFolders)
        {
            if(($file.DestinationPartition -eq $currentPartition.Number) -or $partition_method -eq "standard" )
            {
                if(Test-Path "s:\resources\$($file.SourcePath)" -PathType Leaf)
                {
                    Copy-Item -Path "s:\resources\$($file.SourcePath)" -Destination "c:\$($file.DestinationFolder)" -Force
                }
                else
                {
                    if($file.FolderCopyType -eq "Folder")
                    {
                        Copy-Item -Path "s:\resources\$($file.SourcePath)" -Destination "c:\$($file.DestinationFolder)" -Recurse -Force
                    }
                    else
                    {
                        Copy-Item -Path "s:\resources\$($file.SourcePath)\*" -Destination "c:\$($file.DestinationFolder)" -Recurse -Force
                    }
                }
            }
        }
    
}

function Change-Computer-Name()
{
    log " ** Changing Computer Name ** " "true"
    if(Test-Path C:\Windows\Panther\unattend.xml)
    {
        log " ** Sysprep Answer File Found. Updating Computer Name ** " "true"
        perl -0777 "-i.bak" -pe "s/(\<ComputerName\>).*(\<\/ComputerName\>)/`${1}$computer_name`${2}/si" c:\Windows\Panther\unattend.xml
        rm c:\Windows\Panther\unattend.xml.bak
    }
    else
    {
        log " ** Sysprep Answer File Not Found. Loading Registry Hive ** " "true"
        reg load HKLM\CloneDeploy C:\Windows\system32\config\SYSTEM 2>&1 >> $clientLog
        if($?)
        {
            $private:regObj=$(Get-ItemProperty HKLM:\CloneDeploy\ControlSet001\services\Tcpip\Parameters)
            if(Reg-Key-Exists($private:regObj,"NV Hostname"))
            {
                $private:regObj | Set-ItemProperty -Name "NV Hostname" -Value $computer_name
            }
            if(Reg-Key-Exists($private:regObj,"Hostname"))
            {
                $private:regObj | Set-ItemProperty -Name "Hostname" -Value $computer_name
            }

            $private:regObj=$(Get-ItemProperty HKLM:\CloneDeploy\ControlSet001\Control\ComputerName\ComputerName)
            if(Reg-Key-Exists($private:regObj,"ComputerName"))
            {
                $private:regObj | Set-ItemProperty -Name "ComputerName" -Value $computer_name
            }
        }
        reg unload HKLM\CloneDeploy
    }
}

function Process-Hard-Drives()
{
    Get-Hard-Drives("deploy")

    $script:imagedSchemaDrives=""
    $currentHdNumber=-1

    foreach($hardDrive in $script:HardDrives)
    {
        log " ** Processing Hard Drive $($hardDrive.Number)" "true"
        $currentHdNumber++

        log "Get hd_schema:  profileId=$profile_id&clientHdNumber=$currentHdNumber&newHdSize=$($hardDrive.Size)&schemaHds=$script:imagedSchemaDrives&clientLbs=0"
        $script:hdSchema=$(curl.exe $script:curlOptions -H Authorization:$script:userTokenEncoded --data "profileId=$profile_id&clientHdNumber=$currentHdNumber&newHdSize=$($hardDrive.Size)&schemaHds=$script:imaged_schema_drives&clientLbs=$($hardDrive.LogicalSectorSize)" ${script:web}CheckHdRequirements --connect-timeout 10 --stderr -)
       
        log "$script:hdSchema"
	    $script:hdSchema=$script:hdSchema | ConvertFrom-Json
        if(!$?)
        {
            $Error[0].Exception.Message
            $script:hdSchema
            error "Could Not Parse HD Schema"
        }
        $script:imageHdToUse=$script:hdSchema.SchemaHdNumber
        $script:imagePath="s:\images\$image_name\hd$script:imageHdToUse"
        
        if($script:hdSchema.IsValid -eq "true" -or $script:hdSchema.IsValid -eq "original" )
        {
            log " ...... HD Meets The Minimum Sized Required"
        }
        elseif($script:hdSchema.IsValid -eq  "false" )
        {
            log " ...... $($script:hdSchema.Message)" "true"
            Start-Sleep 10
            continue	
        }
        else
        {
            error "Unknown Error Occurred While Determining Minimum HD Size Required.  Check The Exception Log"
        }

	    Create-Partition-Layout
    }
}




    if($script:task -ne "multicast" -and $script:task -ne "ondmulticast")
    {
        Write-Host " ** Checking Current Queue ** " 	
        while($true)
        {
            $queueStatus=$(curl.exe $script:curlOptions -H Authorization:$script:userTokenEncoded --data "taskId=$script:taskId" ${script:web}CheckQueue --connect-timeout 10 --stderr -)
	        $queueStatus=$queueStatus | ConvertFrom-Json
            if(!$?)
            {
                $Error[0].Exception.Message
                $queueStatus
                error "Could Not Parse Queue Status"
            }
            if($queueStatus.Result -eq "true")
            {
                break
            }
            else
            {
                Write-Host "** Queue Is Full, Waiting For Open Slot ** "
		        Write-Host " ...... Current Position $($queueStatus.Position)"
		        Start-Sleep 5
            }	
        }
	    Write-Host " ...... Complete"
		Write-Host	  		
    }


  Start-Sleep 2

  if($pre_scripts.trim("`""))
  {
	Process-Scripts "$pre_scripts"
  }
	
Mount-SMB

 if($file_copy -eq "True")
    {
        log "file_copy_schema: profileId=$profile_id"
	    $fileCopySchema=$(curl.exe $script:curlOptions -H Authorization:$script:userTokenEncoded --data "profileId=$profile_id" ${script:web}GetFileCopySchema --connect-timeout 10 --stderr -)
        $fileCopySchema=$fileCopySchema | ConvertFrom-Json
        if(!$?)
        {
            $Error[0].Exception.Message
            $fileCopySchema
            log "Could Not Parse File Copy Schema"
            $file_copy="False"
        }
        $fileCopySchema | Out-File $clientLog -Append 
    }


Process-Hard-Drives

if($post_scripts.trim("`""))
{
    Process-Scripts "$post_scripts"
}

CheckOut