$clientLog="x:\clientlog.log"

function log($message, $isDisplay)
{
	Add-Content $clientLog ""
	if($isDisplay -eq "true")
    {
        Write-Host $message | tee -Append -FilePath $clientLog
    }
    Add-Content $clientLog $message
}

function Checkout()
{
    log " ** Closing Active Task ** " "true"
    $computerIdBytes = [System.Text.Encoding]::UTF8.GetBytes($script:computer_id)
    $computerIdEncoded =[Convert]::ToBase64String($computerIdBytes)
    (Get-Content $clientLog) -replace ("`0","") | Set-Content $clientLog
	$logBytes = [System.IO.File]::ReadAllBytes("$clientLog")
    $logEncoded =[Convert]::ToBase64String($logBytes)
    $imageDirectionBytes = [System.Text.Encoding]::UTF8.GetBytes($script:task)
    $imageDirectionEncoded =[Convert]::ToBase64String($imageDirectionBytes)
    $macBytes = [System.Text.Encoding]::UTF8.GetBytes($script:mac)
    $macEncoded =[Convert]::ToBase64String($macBytes)
    curl.exe $script:curlOptions -H Authorization:$script:userTokenEncoded -F computerId="$computerIdEncoded" -F logContents="$logEncoded" -F subType="$imageDirectionEncoded" -F mac="$macEncoded" "${script:web}UploadLog" --connect-timeout 10 --stderr -

    if($script:task -eq "multicast" -or $script:task -eq "ondmulticast" )
    {
        curl.exe $script:curlOptions -H Authorization:$script:userTokenEncoded --data "portBase=$multicast_port" "${script:web}MulticastCheckOut" --connect-timeout 10 --stderr -
	}

    if($script:task -eq "permanentdeploy")
    {
        curl.exe $script:curlOptions -H Authorization:$script:userTokenEncoded --data "taskId=$script:taskId" "${script:web}PermanentTaskCheckOut" --connect-timeout 10 --stderr -      
    }
    else
    {
        curl.exe $script:curlOptions -H Authorization:$script:userTokenEncoded --data "taskId=$script:taskId" "${script:web}CheckOut" --connect-timeout 10 --stderr -
    }
    

    if($task_completed_action.trim("`"") -eq "Power Off")
    {
		wpeutil shutdown
    }

	elseif($task_completed_action.trim("`"") -eq "Exit To Shell")
    {
		[Environment]::Exit(0)
    }
	else
    {
		wpeutil reboot
	} 
}

function error($message, $rebootTime)
{
	Write-Host
	log -message " ** An Error Has Occurred ** " -isDisplay "true"
	log -message " ...... $message" -isDisplay "true"
	Write-Host
	Write-Host " ** Rebooting In One Minute ** "
	
	
    curl.exe $script:curlOptions -H Authorization:$script:userTokenEncoded --data "taskId=$script:taskId&error=$1" ${script:web}ErrorEmail  --connect-timeout 10 --stderr -
	

    $computerIdBytes = [System.Text.Encoding]::UTF8.GetBytes($script:computer_id)
    $computerIdEncoded =[Convert]::ToBase64String($computerIdBytes)
    (Get-Content $clientLog) -replace ("`0","") | Set-Content $clientLog
	$logBytes = [System.IO.File]::ReadAllBytes("$clientLog")
    $logEncoded =[Convert]::ToBase64String($logBytes)
    $imageDirectionBytes = [System.Text.Encoding]::UTF8.GetBytes($script:task)
    $imageDirectionEncoded =[Convert]::ToBase64String($imageDirectionBytes)
    $macBytes = [System.Text.Encoding]::UTF8.GetBytes($script:mac)
    $macEncoded =[Convert]::ToBase64String($macBytes)
    curl.exe $script:curlOptions -H Authorization:$script:userTokenEncoded -F computerId="$computerIdEncoded" -F logContents="$logEncoded" -F subType="$imageDirectionEncoded" -F mac="$macEncoded" "${script:web}UploadLog" --connect-timeout 10 --stderr -
	
	if($rebootTime)
    {
	  Start-Sleep -s $rebootTime
    }
	else
    {
	  Start-Sleep -s 60
	}
	if($task_completed_action.trim("`"") -eq "Power Off")
    {
		shutdown
    }

	elseif($task_completed_action.trim("`"") -eq "Exit To Shell")
    {
		[Environment]::Exit(0)
    }
	else
    {
		reboot

	} 
}

function Mount-SMB()
{
    log " ** Mounting SMB Share ** " "true"
	
	$smbInfo=$(curl.exe $script:curlOptions -H Authorization:$script:userTokenEncoded --data "dpId=$script:dp_id&task=$script:task" ${script:web}DistributionPoint  --connect-timeout 10 --stderr -)
	$smbInfo=$smbInfo | ConvertFrom-Json
    if(!$?)
    {
        $Error[0].Exception.Message
        $smbInfo
        log -message "Could Not Parse SMB Info" -isDisplay "true"
    }
    log " ...... Connecting To $($smbInfo.DisplayName)" -isDisplay "true"
	Mount-SMB-Sub($smbInfo)

    if($script:smbSuccess)
    {
        Start-Sleep 2
	    log -message " ...... Success" -isDisplay "true"
    }
    else
    {
        #look for other distribution points
        log -message " ...... Looking For Other Distribution Points"
        $allClusterDps=$(curl.exe $script:curlOptions -H Authorization:$script:userTokenEncoded --data "computerId=$script:computer_id" ${script:web}GetAllClusterDps  --connect-timeout 10 --stderr -)
        log $allClusterDps
        if(!$?)
        {
            $Error[0].Exception.Message
            $allClusterDps
            error "Could Not Find Additional Distribution Points"
        }
        else
        {
            if($allClusterDps -eq "single")
            {
                error "Could Not Mount SMB Share.  Server Is Not Clustered."
            }
            elseif($allClusterDps -eq "false")
            {
                error "Could Not Mount SMB Share and An Unknown Error Occurred While Looking For Others"
            }
            else
            {
                foreach($localDpId in $allClusterDps.Split(' '))
                {
                    log $localDpId
                    Start-Sleep 2
                    $smbInfo=$(curl.exe $script:curlOptions -H Authorization:$script:userTokenEncoded --data "dpId=$localDpId&task=$script:task" ${script:web}DistributionPoint  --connect-timeout 10 --stderr -)
	                $smbInfo=$smbInfo | ConvertFrom-Json
                    if(!$?)
                    {
                        $Error[0].Exception.Message
                        $smbInfo
                        log -message "Could Not Parse SMB Info" -isDisplay "true"
                    }
                    log " ...... Connecting To $($smbInfo.DisplayName)" -isDisplay "true"
	                Mount-SMB-Sub($smbInfo)
                    if($script:smbSuccess)
                    {
                        Start-Sleep 2
	                    log -message " ...... Success" -isDisplay "true"
                        $script:dp_id=$localDpId
                        break
                    }
                }
            }
        }
    }

    if(!$script:smbSuccess)
    {
        error "Could Not Mount Any Available SMB Shares"
    }

	Start-Sleep 2
}

function Mount-SMB-Sub($smbInfo)
{
    #fix path that was originally only used for initrd
	$sharePath=$smbInfo.SharePath -replace ("/"),("\")
    net use s: /Delete /Yes 2>&1 >> $null
    Start-Sleep 2
    net use s: $sharePath /user:$($smbInfo.Domain)\$($smbInfo.Username) $smbInfo.Password 2>x:\mntstat >> $clientLog
    
	if(!$?)
    {
	    Get-Content x:\mntstat | Out-File $clientLog -Append
		log -message " ...... Could Not Mount SMB Share: $(Get-Content x:\mntstat)"	-isDisplay "true"	
        $script:smbSuccess=$false
    }
	else
    {      
        $script:smbSuccess=$true
        Write-Host
		cd s:\images\$image_name
	    if(!$?)
        {
            $script:smbSuccess=$false
            net use s: /Delete /Yes 2>&1 >> $null
	        log -message " ...... Could Not Change Directory To s:\images\$image_name Verify The Directory Exists And Permissions Are Correct" -isDisplay "true"
	    }
    }
}

function Get-Hard-Drives($taskType)
{
    log " ** Looking For Hard Drive(s) **" "true"
    log " ** Displaying Available Devices ** "
    Get-Disk | Out-File $clientLog -Append
    if($custom_hard_drives)
    {
        if($custom_hard_drives.trim("`""))
        {
            #Todo - finish custom hard drives
            log " ...... Hard Drive(s) Set By Image Profile: $hard_drives"
        }
        else
        {
            if($taskType -eq "upload")
            {
                $script:HardDrives=$(get-disk | where-object {$_.NumberOfPartitions -gt 0 -and $_.BusType -ne "USB"} | Sort-Object Number)
            }
            else
            {
                $script:HardDrives=$(get-disk | where-object {$_.BusType -ne "USB"} | Sort-Object Number)
            }
        }
    }
    else
    {
        if($taskType -eq "upload")
        {
            $script:HardDrives=$(get-disk | where-object {$_.NumberOfPartitions -gt 0 -and $_.BusType -ne "USB"} | Sort-Object Number)
        }
        else
        {
            $script:HardDrives=$(get-disk | where-object {$_.BusType -ne "USB"} | Sort-Object Number)
        }
    }

    if(@($script:HardDrives).count -eq 0)
    {
        error "Could Not Find A Hard Drive Attached To This Computer."
    }

    log " ...... Found $(@($script:HardDrives).count) Drives" "true"

    foreach($hardDrive in $script:HardDrives)
    {
        log " ** Displaying Current Partition Table On $($hardDrive.Number)"
        Get-Partition -DiskNumber $hardDrive.Number | Out-File $clientLog -Append
    }
    Write-Host
}

