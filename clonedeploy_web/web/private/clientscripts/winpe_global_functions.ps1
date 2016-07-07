$clientLog="x:\clientlog.log"

function log($message, $isDisplay)
{
	Add-Content $clientLog ""
	if($isDisplay -eq "true")
    {
        Write-Host $message | tee -Append -FilePath $clientLog
    }
	else
    {
		Add-Content $clientLog $message
	}
}

function error($message, $rebootTime)
{
	Write-Host
	log -message " ** An Error Has Occurred ** " -isDisplay "true"
	log -message " ...... $message" -isDisplay "true"
	Write-Host
	Write-Host " ** Rebooting In One Minute ** "
	
	if(!$computer_id)
    {
	  $computer_id="-1"
	}
    else
    {
        curl.exe $env:curlOptions -H Authorization:$env:userTokenEncoded --data "computerId=$computer_id&error=$1" ${web}ErrorEmail  --connect-timeout 10 --stderr -
	}

    $computerIdBytes = [System.Text.Encoding]::UTF8.GetBytes($computer_id)
    $computerIdEncoded =[Convert]::ToBase64String($computerIdBytes)
	$logBytes = [System.IO.File]::ReadAllBytes("x:\clientlog.log")
    $logEncoded =[Convert]::ToBase64String($logBytes)
    $imageDirectionBytes = [System.Text.Encoding]::UTF8.GetBytes($image_direction)
    $imageDirectionEncoded =[Convert]::ToBase64String($imageDirectionBytes)
    $macBytes = [System.Text.Encoding]::UTF8.GetBytes($mac)
    $macEncoded =[Convert]::ToBase64String($macBytes)
    curl.exe $env:curlOptions -H Authorization:$env:userTokenEncoded -F computerId="$computerIdEncoded" -F logContents="$logEncoded" -F subType="$imageDirectionEncoded" -F mac="$macEncoded" "${web}UploadLog" --connect-timeout 10 --stderr -
	
	if($rebootTime)
    {
	  Start-Sleep -s $rebootTime
    }
	else
    {
	  Start-Sleep -s 60
	}
	if($task_completed_action -eq "Power Off")
    {
		shutdown
    }

	elseif($task_completed_action -eq "Exit To Shell")
    {
		exit
    }
	else
    {
		reboot
	} 
}

function Mount-SMB()
{
    log " ** Mounting SMB Share **" "true"
	
	$smbInfo=$(curl.exe $env:curlOptions -H Authorization:$env:userTokenEncoded --data "dpId=$dp_id&task=$image_direction" ${web}DistributionPoint  --connect-timeout 10 --stderr -)
	$smbInfo=$smbInfo | ConvertFrom-Json
    if(!$?)
    {
        $Error[0].Exception.Message
        $smbInfo
        error "Could Not Parse SMB Info"
    }
	#fix path that was originally only used for initrd
	$share_path=$smbInfo.SharePath -replace ("/"),("\")
    Write-Host $share_path $smbInfo.Domain $smbInfo.Username $smbInfo.Password
    net use s: /Delete > $null
    net use s: \\192.168.1.10\jon /user:$($smbInfo.Domain)\jon $smbInfo.Password 2>x:\mntstat
    
		
		if(!$?)
        {
			Get-Content x:\mntstat >> $clientLog
			error -message "Could Not Mount SMB Share: $(Get-Content x:\mntstat)"		
        }
		else
        {
			log " ...... Success" "display"

			cd s:\images\$img_name;
			if(!$?)
            {
				error "Could Not Change Directory To s:\images\$image_name Check Permissions"
			}
		}
	echo
	sleep 2
}